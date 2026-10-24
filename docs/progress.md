# 學習進度

更新日期：2026-10-24（Asia/Taipei）。

## Checkpoint

- phase：C03 進行中；SQL Server 容器、Entity／DbContext、Migration、種子資料、活動查詢改 EF Core、不可翻譯表達式與 change tracker 實驗已完成。
- current_module：C03，SQL Server／EF Core 與資料建模。
- next_step：同一 DbContext 平行查詢（Task.WhenAll）實驗；之後建立 API 整合測試入口（真實 SQL Server，不用 EF InMemory），並做「從空資料庫 Migration＋種子」驗收。報名資料結構尚未建立。
- 教材入口：[課綱 C03](curriculum.md#c03sql-serveref-core-與資料建模)；C03 詳細教材尚未建立，先依課綱逐步教學。
- 接續方式：先讀本文件與 [學習方式](learning-guide.md)，再讀當前教材；不直接展開後續單元。

## 已完成

- 確定題目為企業內部活動報名系統，維持四個核心流程。
- 建立課程大綱、教學方式、Azure 規劃與 C01 詳細教材。
- 核對官方 .NET 支援與 Azure 免費額度／服務退場資訊，來源見各文件。
- 學員已實作 ActivitiesController 與 ActivitySummaryDto，修正 Route／HttpGet Name 混淆，並回報 API 可呼叫。
- 學員已修改活動名稱與分類、加入 required，回報建置無警告；AI 本次重新建置確認成功，0 個警告、0 個錯誤。
- 已閱讀學員指定的 LeoInfo 履歷與技能資料，調整課綱、學習方式、C01 教材與接續點；背景與來源見 learning-guide.md。
- 學員授權 AI 自行決定本專案 commit／push 時機；範圍與新 session 接續方式已記錄於 learning-guide.md。

## 單元狀態

| 單元 | 狀態 | 成果與證據 |
|---|---|---|
| C01 | API 基線與概念導讀完成 | 學員確認理解 LINQ、record、required／init／nullable；差異實驗略過，未宣稱實作驗收 |
| C02 | 實作驗收完成 | 學員已完成各練習；授權 AI 清理實驗後，AI 建置與清單／200／404／400／實驗路由移除驗證通過；Middleware 例外順序仍需熟悉，Options 動態更新未實驗 |
| C03 | 進行中 | 活動查詢已改 SQL Server；AI 實跑 200／200／404／400 與 SQL 日誌通過；整合測試、報名結構、平行查詢實驗未完成 |
| C04 | 未開始 | — |
| C05 | 未開始 | — |
| C06 | 未開始 | — |
| C07 | 未開始 | 尚未建立 Azure 資源或部署網站 |
| C08 | 未開始 | — |
| C09 | 未開始 | — |
| C10 | 未開始 | — |
| C11 | 未開始 | — |
| C12 | 未開始 | — |

## C01 環境與驗證紀錄

2026-10-16 由 AI 在本機執行，非學員操作驗收：

- `git --version`：`2.54.0.windows.1`。
- `dotnet --list-sdks`：已安裝 `8.0.418`、`10.0.402`。
- Visual Studio Installer 查詢：Visual Studio Community 2026，`18.11.12306.390`。
- 既有 `src/ActivityRegistration.Api/ActivityRegistration.Api.csproj` 目標為 `net10.0`；沿用目前專案，不重跑建立範本命令。
- `dotnet build src/ActivityRegistration.Api/ActivityRegistration.Api.csproj --no-restore`：成功，0 個警告、0 個錯誤。
- 路由診斷時 AI 實測：原設定 `/api/activities` 為 404、`/activities` 為 200 並回傳三筆活動；診斷程序已停止。此證據屬修正前，不當成最新路由通過證據。
- 學員修正後回報 `/api/activities` 可呼叫；已理解 camelCase、路由路徑及 required。回應資料是否寫回伺服器的回答是題意誤會，不列為後端基礎不足。
- 本次讀檔確認 Route 為 `api/activities`、三筆活動名稱／分類已修改、Title／Category 使用 required；重新執行相同 build 指令成功，0 警告、0 錯誤。最新 HTTP 成功由學員回報，本次未再啟動服務驗證。
- 學員已說明 LINQ 延遲執行／實體化並要求跳過；record／淺層複製與 required／init／nullable 導讀後，學員確認理解。依學員節奏接續 C02；未執行上述差異實驗，不標成完整實作驗收。

## C02 驗證紀錄

- 學員實測：Scoped 同請求 ID 相同、跨請求不同；Transient 同請求 ID 不同；Singleton 同請求與跨請求 ID 均相同。已討論實際用途、共享狀態風險與註冊位置的除錯限制，學員確認理解。
- AI 讀檔確認：ActivityService.GetActivities() 保留原三筆活動；Program.cs 使用 AddScoped<ActivityService>()；ActivitiesController 透過主要建構函式注入服務，Get() 委派查詢；已移除 Probe action 與 LifetimeProbe 註冊，保留實驗類別檔。
- 學員回報服務拆分後 API 正確；AI 本次未重新呼叫 HTTP。
- AI 查碼確認 GetActivityById(int id)、{id:int} 路由、Range(1, int.MaxValue) 與 200／404 分支已實作。學員貼出查無活動的 404 ProblemDetails，已說明 ApiController 自動轉換與路由未匹配的差異；Range 400 尚無獨立 HTTP 回報。
- 學員已加入請求日誌 Middleware，並確認 1／999／abc 請求均有開始、結束日誌與預期狀態。AI 查碼確認結構化日誌與 await next(context) 順序；再次以獨立輸出建置成功，0 警告、0 錯誤，AI 未重跑 HTTP。
- 學員確認例外實驗：500 ProblemDetails、不洩漏測試例外／堆疊、伺服器例外日誌與結束請求 500，並於移除故障程式後恢復 404。AI 查碼確認 AddProblemDetails、日誌之後的 UseExceptionHandler 與測試例外已移除；獨立輸出建置成功，0 警告、0 錯誤，AI 未重跑 HTTP。
- 學員確認 404 JSON 的 traceId 與同次開始／結束日誌一致，跨請求識別碼不同。AI 查碼確認 Middleware 使用 Activity.Current?.Id ?? context.TraceIdentifier，兩筆日誌均包含 TraceId；獨立輸出建置成功，0 警告、0 錯誤，AI 未重跑 HTTP。
- 學員回報已完成設定覆蓋實驗。AI 查碼確認基本設定有 Application:DisplayName、開發設定已移除該 key，啟動日誌讀取環境與合併後的設定；獨立輸出建置成功，0 警告、0 錯誤，git diff --check 通過。啟動顯示與覆蓋行為由學員驗證，AI 未重新啟動服務。
- 學員回報 Options 練習完成。AI 查碼確認 ApplicationOptions、Configure 綁定與 IOptions<ApplicationOptions>.Value 讀取均已實作，獨立輸出建置成功，0 警告、0 錯誤；啟動顯示由學員確認。仍保留舊版 Configuration 字串索引啟動日誌，會重複輸出；已列入下一步清理，AI 未代改後端程式。
- 學員回報設定驗證故障／恢復練習完成。AI 查碼確認已改用 AddOptions、Bind、非空白 Validate、ValidateOnStart，舊版啟動日誌已移除，JSON 已恢復正常名稱；獨立輸出建置成功，0 警告、0 錯誤，git diff --check 通過。啟動失敗與恢復由學員驗證，AI 未重新啟動服務。
- 已導讀 IOptions／Snapshot／Monitor。學員對 Singleton 不能直接持有 Scoped Snapshot 提問，已用長期物件留住短期物件的例子補充；學員要求繼續。未執行動態更新實驗，不標為實作驗收；現有 IOptions 用法維持。
- 學員回報 async-probe 等待練習完成。AI 查碼確認 action 使用 async Task<IActionResult> 與 await Task.Delay(3000)，獨立輸出建置成功，0 警告、0 錯誤，git diff --check 通過；約三秒回應由學員確認，AI 未重跑 HTTP。已說明 Task 不等於新執行緒、Task.Delay 為計時實驗，正常記憶體查詢仍保持同步。
- 學員確認請求取消實驗，含伺服器取消日誌。AI 查碼確認 action 接收 CancellationToken、傳入 Task.Delay(10000, token)、僅於對應 token 取消時記錄並 rethrow；獨立輸出建置成功，0 警告、0 錯誤，git diff --check 通過。AI 未重跑取消 HTTP。實驗 action 與 ILogger 參數目前仍在，待學員清理。
- 學員確認清理 async-probe 後，既有 1／999／0 查詢分別回應 200／404／400；Range 400 已補學員實測證據。AI 查碼確認實驗 action 與 ILogger 參數均移除，原查詢保留；獨立輸出建置成功，0 警告、0 錯誤，git diff --check 通過，AI 未重跑 HTTP。
- AI 查碼發現 LocalProbe 命名 client、factory 注入與 http-probe action 已由學員加入，但未收到 HTTP 成功回報，不列為驗收。學員要求改用公開外網，避免呼叫自己造成理解負擔；已透過官方網站核對 JSONPlaceholder 不需註冊／API key，並讀取 https://jsonplaceholder.typicode.com/todos/1 的四個 JSON 欄位。後續教學以具體外部 API 情境為優先。
- 學員回報公開 API 精簡版已完成。AI 查碼確認 JsonPlaceholder 命名 client、TodoDto、http-probe 使用 GetFromJsonAsync<TodoDto> 並傳 CancellationToken、using var 管理 client；獨立輸出建置成功，0 警告、0 錯誤，git diff --check 通過。HTTP 成功依學員回報，AI 未重跑本機 HTTP。
- 教學調整：先給最小可用寫法，再依需求補 DI／型別／資源管理；與 Node.js 比較時對齊相同責任範圍，避免用裸 fetch 對比完整 .NET 樣板。外部 HTTP 示範使用具體公開 API，不呼叫自己。
- 觀念確認：學員正確回答 using var 在方法離開時釋放；對 Exception Handler 移到日誌外層時結束日誌仍會執行的回答有誤，已用例外往外傳遞的流程補充。學員表示大概了解，記為已導讀、仍需熟悉，不宣稱完全掌握。
- 學員明確授權 AI 直接清理。AI 已移除 HttpProbeAsync、IHttpClientFactory 建構函式參數、JsonPlaceholder 註冊，刪除 TodoDto.cs 與 LifetimeProbe.cs；兩個修改的 C# 檔保留 UTF-8 無 BOM／CRLF。以獨立輸出目錄 `employee-activity-registration-c02-final` 建置成功，0 警告、0 錯誤。
- AI 使用最新 DLL 在動態 loopback 連接埠啟動獨立驗證程序，載入原專案設定；清單為 200／三筆資料、活動 1 為 200／資料正確、999 為 404、0 為 400／errors.id、http-probe 為 404，全部通過。驗證程序已停止，未停止學員原程序；git diff --check 通過。未新增自動化測試檔，未修改正式查詢邏輯。
- 一般建置因執行中的 ActivityRegistration.Api 鎖定 apphost 複製而產生 MSB3026 警告；未停止學員程序。改用 `dotnet build src/ActivityRegistration.Api/ActivityRegistration.Api.csproj --no-restore -p:UseAppHost=false -o "$env:TEMP\employee-activity-registration-c02-check"`，建置成功，0 個警告、0 個錯誤。

## C03 驗證紀錄

- 學員要求後，AI 清除 Docker Desktop 內所有前公司殘留（容器、映像、volume、網路、build cache，均為 0）。
- AI 建立 `docker-compose.yml`（SQL Server 2022 Developer、具名 volume、健康檢查），容器 healthy。學員完成 down／down -v 持久化實驗；對 -v 的理解原為「刪容器」，已更正為「多刪 volume」。
- 學員選擇本機開發密碼與連線字串提交至 `appsettings.Development.json`（learning-guide 規則已同步）；移除 .env 與 user-secrets。
- AI 加入 EF Core SqlServer／Design 10.0.13 與 dotnet-ef 工具；直接引用 System.Security.Cryptography.Pkcs 10.0.13 消除間接弱點警告。
- 學員撰寫 Activity Entity、AppDbContext、AddDbContext 註冊。AI 審查發現 `using System.Diagnostics` 造成 DbSet 指向 System.Diagnostics.Activity、缺 options 建構函式、Capacity 為 string、nullable 警告，學員已修正。
- 學員以 Fluent API 設定 Title 100／Category 50，remove 後重建 InitialCreate；以 HasData 建立 SeedActivities。AI 查資料庫確認兩筆 Migration 歷史與三筆活動。學員正確說明 __EFMigrationsHistory、HasData 需指定主鍵、已套用 Migration 應新增 Migration 修改。
- 學員將 ActivityService 改為注入 AppDbContext、投影 DTO、ToListAsync／FirstOrDefaultAsync；曾漏 `using Microsoft.EntityFrameworkCore` 導致 .NET 10 內建 AsyncEnumerable.ToListAsync 誤判，及單筆查詢先撈全表，均已修正。AI 以獨立連接埠實跑：清單 200、1 為 200、999 為 404、0 為 400；SQL 為 `SELECT TOP(1) ... WHERE [a].[Id] = @id`。
- 學員實測自訂方法放入 Where 於執行期報翻譯錯誤；已說明 EF Core 3 起不再自動用戶端評估。
- 學員完成 change tracker 實驗：DebugView 顯示 Originally 值、AsNoTracking 修改不寫入。學員不知道 AsNoTracking 的用途，已說明追蹤成本，以及投影 DTO 本來就不追蹤；DebugView 狀態未經 DetectChanges 故顯示 Unchanged。實驗程式已由學員清除，AI 建置 0 警告 0 錯誤。

## 尚未確認

- 學員每週可投入時間；課綱先依單元驗收推進。
- Azure 訂用帳戶、部署區域與月費上限；於 C07 建立資源前確認。

## 更新規則

每堂課結束更新 current_module、next_step、驗收指令與結果，以及學員仍需解釋的問題。只記實際執行的結果；規劃、AI 已產碼、學員已完成與已部署分開標示。
