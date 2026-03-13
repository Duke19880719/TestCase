using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestCase.Server.Context;
using TestCase.Server.Model;

namespace TestCase.Server.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly AssetDbContext _context;

        // 透過建構函式注入 DbContext
        public ApiController(AssetDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetAssets")]
        public async Task<IActionResult> GetAssets([FromQuery] int year)
        {
            // 使用 LINQ Join ，轉回前端的 Asset 格式 
            var result = await _context.AssetEvaluations
                .Include(e => e.AssetItem)
                .Include(e => e.Owner)
                .Where(e => e.Year == year)
                .Select(e => new Asset
                {
                    Id = e.AssetItemId,
                    Type = e.AssetItem.Type,
                    Name = e.AssetItem.Name,
                    C = e.C,
                    I = e.I,
                    A = e.A,
                    Value = e.Value,
                    Level = e.Level,
                    Threat = e.Threat,
                    Weakness = e.Weakness,
                    Risk = e.Risk,
                    Owner = e.Owner.Name, 
                    Year = e.Year
                })
                .ToListAsync();

            return Ok(result);
        }

        [HttpPost("CreateAsset")]
        public async Task<IActionResult> CreateAsset([FromBody] Asset newAsset)
        {
            if (newAsset == null) return BadRequest("資料不完整");

            // 1. 處理自動產號邏輯 (從 AssetItems 表找最後一個 ID)
            string? lastId = await _context.AssetItems
                .OrderByDescending(a => a.Id)
                .Select(a => a.Id)
                .FirstOrDefaultAsync();

            int number = 1;
            string prefix = "I-";
            if (!string.IsNullOrEmpty(lastId) && lastId.StartsWith(prefix))
            {
                if (int.TryParse(lastId.Substring(prefix.Length), out int lastNumber))
                    number = lastNumber + 1;
            }
            newAsset.Id = $"{prefix}{number:D3}";

            // 2. 處理 Owner (檢查人名是否已存在)
            var owner = await _context.Owners.FirstOrDefaultAsync(o => o.Name == newAsset.Owner);
            if (owner == null)
            {
                owner = new Owner { Name = newAsset.Owner };
                _context.Owners.Add(owner);
                await _context.SaveChangesAsync();
            }

            // 3. 處理 AssetItem (主項)
            var item = await _context.AssetItems.FindAsync(newAsset.Id);
            if (item == null)
            {
                item = new AssetItem { Id = newAsset.Id, Name = newAsset.Name, Type = newAsset.Type };
                _context.AssetItems.Add(item);
            }

            // 4. 建立評估紀錄 (Evaluation)
            var evaluation = new AssetEvaluation
            {
                AssetItemId = newAsset.Id,
                Year = newAsset.Year,
                C = newAsset.C,
                I = newAsset.I,
                A = newAsset.A,
                Value = newAsset.Value,
                Level = newAsset.Level,
                Threat = newAsset.Threat,
                Weakness = newAsset.Weakness,
                Risk = newAsset.Risk,
                OwnerId = owner.Id
            };

            _context.AssetEvaluations.Add(evaluation);
            await _context.SaveChangesAsync();

            return Ok(newAsset);
        }

        [HttpPut("UpdateAsset")]
        public async Task<IActionResult> UpdateAsset([FromBody] Asset updateData)
        {
            // 找到該年份的該項資產評估
            var evaluation = await _context.AssetEvaluations
                .Include(e => e.AssetItem)
                .FirstOrDefaultAsync(e => e.AssetItemId == updateData.Id && e.Year == updateData.Year);

            if (evaluation == null) return NotFound("找不到對應年份的資產評估");

            // 更新主項資料 (AssetItem)
            evaluation.AssetItem.Name = updateData.Name;
            evaluation.AssetItem.Type = updateData.Type;

            // 更新評估數值
            evaluation.C = updateData.C;
            evaluation.I = updateData.I;
            evaluation.A = updateData.A;
            evaluation.Value = updateData.Value;
            evaluation.Level = updateData.Level;
            evaluation.Threat = updateData.Threat;
            evaluation.Weakness = updateData.Weakness;
            evaluation.Risk = updateData.Risk;

            // 處理負責人變更
            if (evaluation.Owner.Name != updateData.Owner)
            {
                var newOwner = await _context.Owners.FirstOrDefaultAsync(o => o.Name == updateData.Owner);
                if (newOwner == null)
                {
                    newOwner = new Owner { Name = updateData.Owner };
                    _context.Owners.Add(newOwner);
                    await _context.SaveChangesAsync();
                }
                evaluation.OwnerId = newOwner.Id;
            }

            await _context.SaveChangesAsync();
            return Ok(updateData);
        }

        [HttpDelete("DeleteAsset/{id}")]
        public async Task<IActionResult> DeleteAsset(string id, [FromQuery] int year)
        {
            // 刪除「該年度的評估紀錄」
            var evaluation = await _context.AssetEvaluations
                .FirstOrDefaultAsync(e => e.AssetItemId == id && e.Year == year);

            if (evaluation == null) return NotFound();

            _context.AssetEvaluations.Remove(evaluation);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
