# C01：既有 API 與現代 C# 習慣轉換

2026-10-16 調整。活動 API 基本練習已完成並由學員回報可呼叫；現代 C# 差異練習尚未執行。證據以 [學習進度](../progress.md) 為準。

## 本堂成果與起點

沿用 `src/ActivityRegistration.Api`，不重建專案、不重教 HTTP／JSON／DTO。透過既有 TypeScript 與舊 MVC 經驗，讀懂 nullable、required、class／record 與 LINQ 的行為差異。本堂使用記憶體資料；資料庫在 C03 加入，非同步流程在 C02 說明。

目前檔案為 `Controllers/ActivitiesController.cs`、`Dtos/ActivitySummaryDto.cs`。三筆活動已包含分類；Title／Category 已加上 required，路由為 `api/activities`。

## 1. 已完成的環境與框架對照

已確認 SDK `10.0.402`、Git、Visual Studio 2026 與 `net10.0` 專案；不重做安裝。環境改變或發生錯誤時才重查。

| 既有經驗 | 本專案要記住的差異 |
|---|---|
| package.json、npm script | `.csproj` 管理框架與 NuGet 相依；dotnet build／run／publish 分別負責建置、啟動、發布 |
| Node 執行環境與建置工具 | TargetFramework 指定目標框架，並非指定實際執行的 SDK 版本 |
| Express 註冊 route | AddControllers 註冊服務、MapControllers 映射 attribute 路由；Route 定義路徑，HttpGet 的 Name 不是網址 |
| 舊 MVC Global.asax／web.config | 現代 host 在 Program.cs 組合；DI、設定與 Middleware 在 C02 動手對照 |
| JS 欄位名稱 | C# 屬性常用 PascalCase；目前 Web JSON 預設用 camelCase，並非全部轉小寫 |

本機重跑方式：

```powershell
dotnet build src/ActivityRegistration.Api/ActivityRegistration.Api.csproj
dotnet run --project src/ActivityRegistration.Api/ActivityRegistration.Api.csproj --launch-profile http
```

依現有 launchSettings.json，另開終端呼叫：

```powershell
Invoke-RestMethod -Uri 'http://localhost:5152/api/activities'
```

使用 `--no-launch-profile --urls ...` 時，以實際指定網址為準。本機 HTTP 僅供學習，不代表已部署。

## 2. 型別與輸入驗證不要混為一談

| 寫法 | 實際保證與限制 |
|---|---|
| `string`／`string?` | nullable 分析告知是否允許 null；不是自動插入執行期防護 |
| `required string Title` | 一般 C# 建立物件時要求賦值；不可為 null 另由 nullable 分析處理，不保證不是空字串或空白 |
| `init` | 限制一般 C# 程式只能在初始化階段設定屬性；不會讓屬性指向的 List 自動不可變 |
| `var` | 由編譯器推斷靜態型別，不等於 JS 可改成另一種型別的變數，也不是 dynamic |
| TypeScript `as` 與 C# cast | TS assertion 不會轉換執行期物件；C# 轉型可能檢查相容性或執行轉換，不可直接視為同一操作 |

已練過 required 的編譯要求；不再重做拿掉 Title 的練習。下一次遇到 API 輸入時再區分：JSON 反序列化、Model Binding／DataAnnotations 與業務驗證，各自如何拒絕缺漏或不合法資料，不能把 required 當成 Zod 的完整替代。

## 3. 下一個練習：LINQ 不是直接換名字的 Array.filter

LINQ 是 C# 的資料查詢工具。`Where`／`Select` 對記憶體集合常採延遲執行：先建立查詢，到列舉時才處理資料。它與 JS Array.filter／map 立即建立結果陣列的習慣不同；`ToList()` 會在該時間點實際列舉並建立清單。

沿用目前 Get() 方法，在三筆活動都加入 list 後：

1. 建立查詢 `var query = list.Where(activity => activity.Capacity >= 20);`。
2. 接著將原本 16 人活動的 Capacity 改成 25，再回傳 query；實際呼叫並觀察包含哪些活動。
3. 只把第 1 步改成 `var query = list.Where(activity => activity.Capacity >= 20).ToList();`，其餘順序不變，再觀察結果。
4. 說明差異來自哪個執行時間點，完成後還原診斷用的名額變更與篩選，維持三筆列表基線。

只比較這一個因素，不新增 API、套件或測試專案。ToList 建立的是集合快照，元素仍可指向原物件，並非深層複製。C03 再觀察 IQueryable 如何把篩選轉成 SQL，以及提早 ToList 為什麼會把工作搬回應用程式。

## 4. 後續短練習：class、record 與參考共享

完成 LINQ 練習後才進行，每次只做一個：

- class 變數指定給另一個變數，兩者可指向同一物件；對照 JS 物件與現有 DTO，不重教前後端資料傳輸。
- 比較相同欄位的兩個 class 與兩個 record 的預設相等行為；record 通常用於資料值比較，並非一律取代 class。
- 觀察 record 的 `with` 淺層複製；若含 List，修改複本的 List 可能影響原物件。它與 JS spread 一樣不代表深層複製。
- 分清 record class／record struct；目前只需能閱讀，不為了練語法重構現有 DTO。

## 5. 驗收與接續

- 基線：能建置並呼叫三筆活動；已完成部分不重做通用後端問答。
- 差異：透過 LINQ 實驗說明執行時間點；能讀懂 nullable／required／init 的責任與限制，以及 class／record／淺層複製的差異。
- 以實作、除錯或精簡差異說明驗收；不把履歷使用過或教材已寫好標成已練會。
- 本堂不新增只測固定三筆資料的單元測試；C03 起建立整合測試。
- 完成後更新 progress.md，再開始 C02 的 DI 生命週期與非同步模型，不先展開整套答案。

## 官方參考

- [required 的保證與限制](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/required)
- [LINQ 延遲執行](https://learn.microsoft.com/en-us/dotnet/standard/linq/deferred-execution-lazy-evaluation)
- [record 與淺層複製](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/record)
