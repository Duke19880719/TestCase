碰到的問題:
1.就是後端再回傳JSON 的時候，將Model Name 轉小寫，
我在後端Model Name是大寫名字，害我在前端Mapping 不起來，program.cs加入
builder.Services.AddControllers()
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.PropertyNamingPolicy = null;
    });
同樣大寫，解決問題。

2.因為json id 打算後端自行增加，所以前端在傳資料時，沒有id這個欄位，
也沒有值，後端在接收資料的時候，會報錯說HTTP 400 (Bad Request) 的問題，
前端資料給預設值解決問題

設計概念:
前端json 架構採扁平的data model 設計，就是Model 資料夾下面的Asset.cs，
專門用來蒐集前端資料，DB 畫erd 看各表的關係，寫功能相依，採正規化做到第三正規化，建立DB Context，
放在Context資料夾，拆分三張表，在appsetting 設定連線字串，db連線採di 注入。

前端主要顯示欄位內容，獨立拆出來當一個ui 元件AssetTable，再用父元件App.vue 傳值給它
，傳年份資料給它，讓觸發fetchAssets func，去撈資料，顯示在畫面上。

使用 watch 監聽年份資料變化，當年份資料變化的時候，去撈資料庫，顯示在畫面上。
使if 判斷當前狀態，決定切換編輯 或  新增，來改變欄位顯示。

先安裝sqllite 的套件，建立三張表的model 和 dbcontext的撰寫，下cli指令，更新到資料庫。
下載sqllite browser，在sql 畫面，輸入insert 資料指令，在下select join 三張表，
確定資料有成功進去。

先撰寫前端，依據設計稿，去修改，再到後端撰寫api，宣告 list array 模擬資料庫資料，
前端看是否能成功從後端拿到資料，成功拿取後，再依序寫crud api。

後端完成後，才去弄資料庫的安裝，還有撰寫連線資料庫，跟後端程式碼的改寫成連線資料庫版本。
