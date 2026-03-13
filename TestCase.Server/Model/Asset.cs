namespace TestCase.Server.Model
{
    //前端蒐集資料扁平模型
    public class Asset
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public int C { get; set; }
        public int I { get; set; }
        public int A { get; set; }
        public int Value { get; set; }
        public int Level { get; set; }
        public string Threat { get; set; }
        public string Weakness { get; set; }
        public int Risk { get; set; }
        public string Owner { get; set; }
        public int Year { get; set; }   
    }

    // 資產基本資訊 (AssetItem.cs)
    public class AssetItem
    {
        public string Id { get; set; } // 例如: I-001
        public string Name { get; set; }
        public string Type { get; set; }
        // 一個資產可以有多個年份的評估
        public List<AssetEvaluation> Evaluations { get; set; }
    }

    //年度評估與風險 (AssetEvaluation.cs)
    public partial class AssetEvaluation
    {
        public int Id { get; set; } // 資料庫自增主鍵
        public string AssetItemId { get; set; } // 外鍵
        public int Year { get; set; }
        public int C { get; set; }
        public int I { get; set; }
        public int A { get; set; }
        public int Value { get; set; }
        public int Level { get; set; }
        public string Threat { get; set; }
        public string Weakness { get; set; }
        public int Risk { get; set; }

        public int OwnerId { get; set; } // 外鍵
        public Owner Owner { get; set; }
        public AssetItem AssetItem { get; set; }
    }

    //負責人 (Owner.cs)
    public class Owner
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
