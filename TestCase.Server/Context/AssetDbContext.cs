using Microsoft.EntityFrameworkCore;
using TestCase.Server.Model;

namespace TestCase.Server.Context
{
    public class AssetDbContext:DbContext
    {
        public AssetDbContext(DbContextOptions<AssetDbContext> options) : base(options) { }
        public DbSet<AssetItem> AssetItems { get; set; }
        public DbSet<AssetEvaluation> AssetEvaluations { get; set; }
        public DbSet<Owner> Owners { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 設定 AssetItem 的 Id 不由資料庫自動產生 
            modelBuilder.Entity<AssetItem>().Property(a => a.Id).ValueGeneratedNever();
        }
    }
}
