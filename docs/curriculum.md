# 課程大綱

範圍與核心規則以 [專案說明](README.md) 為準；教學執行方式見 [學習方式](learning-guide.md)。這是教學課綱，不是授權 AI 一次完成整個系統的實作指令。

## 節奏與里程碑

共 12 個單元。2026-10-16 依履歷調整為有 Node.js 後端與舊版 .NET 經驗的轉換課程；背景依據與略過範圍見 [學習方式](learning-guide.md)。下列時間只保留作單元工作量參考，已會內容直接略過，總時數於 C02／C03 後依實際練習重新評估，不再以原本的入門時數要求重做。

## 轉換時優先處理的誤解與習慣

| 既有習慣或容易套用的類比 | 本課程要建立的區別 | 單元 |
|---|---|---|
| TypeScript 型別註記、`as`、Zod 驗證 | C# 型別存在於執行期；nullable 是靜態分析，`required`、JSON 反序列化與業務驗證責任不同 | C01、C02 |
| JS 物件／spread、TS readonly | class／record、參考共享、`with` 淺層複製與 `init` 的限制；不把 record 當深層不可變物件 | C01 |
| 陣列 `filter/map` 得到結果集合 | LINQ 的 `Where/Select` 常延遲執行；`IEnumerable` 不等於 List，重複列舉可能重算，`IQueryable` 還涉及 SQL 翻譯 | C01、C03、C08 |
| Express middleware、module 單例／自行 new service | ASP.NET Core Middleware 順序、DI 生命週期與作用域；Singleton 不保證執行緒安全 | C02 |
| JS event loop、Promise、`Promise.all` | .NET 請求可在不同執行緒同時執行；Task 不代表新執行緒，避免 `.Result/.Wait()`，不把所有 I/O 包進 Task.Run | C02、C06 |
| 同一 ORM client／連線池、平行發查詢 | DbContext 是短生命週期工作單元且非執行緒安全；同一 Context 不可平行操作；追蹤、SaveChanges 與交易分開理解 | C03 |
| JWT／HMAC 與自訂驗證流程 | Identity、Cookie、Claims、授權 Policy、CSRF 的框架整合；不把驗證成功當成資料所有權正確 | C04 |
| 已熟悉 CAS、冪等、Redis 與壓測概念 | 改練 SQL Server 限制、交易／鎖定行為、EF Core 錯誤處理與 .NET client；保留原本併發正確性實驗 | C05、C06、C08～C10 |
| Lambda invocation／Express 程序、AWS 維運 | ASP.NET Core host 啟停、取消與資源釋放；Azure 不與 AWS 服務強行一對一對應 | C02、C07、C11、C12 |


| 階段 | 單元 | 產出 |
|---|---|---|
| A：.NET 基礎 | C01～C03 | 可查詢真實資料庫的 API |
| B：完整流程 | C04～C06 | 可登入、報名、取消，且不超收的本機後端 |
| C：第一次上線 | C07 | AI 前端串接，Azure 上可操作的第一版 |
| D：效能實驗 | C08～C10 | 查詢優化、Redis 與負載測試的實測證據 |
| E：作品交付 | C11～C12 | 自動部署、維運紀錄與面試展示素材 |

後半段增加技術深度，沿用同一組業務功能。

## C01：現代 C# 與第一支 ASP.NET Core API

- 預估：4～6 小時。
- 學習：快速對照 SDK、TargetFramework、NuGet 與舊 MVC 啟動方式；重點為 nullable／required／init、class／record、參考與值相等、集合與 LINQ 執行時機。
- 實作：沿用學員完成的活動 API；用局部實驗觀察 required 與驗證的界線、LINQ 延遲執行及 ToList 快照，不重建專案。
- 驗收：活動 API 建置與 HTTP 基線；能從實驗指出 C# 與 TypeScript／Array 操作不同的地方，不重考 DTO 或 JSON 基礎。
- 差異練習：`Where` 定義後才變動來源資料，列舉結果何時改變？nullable 警告是否等於執行期驗證？
- 詳細教材：[第一堂課](lessons/01-environment-and-first-api.md)。

## C02：請求流程與框架基礎

- 預估：6～8 小時。
- 學習：路由、Model Binding、驗證、DI（由框架提供與管理物件）、Transient / Scoped / Singleton、Middleware（請求依序經過的處理步驟）、Options、環境設定與日誌。
- 學習：`Task`、`async/await`、`CancellationToken`；理解非同步 I/O 與另外開執行緒的差別。
- 轉換重點：對照 Express 的 next／錯誤傳遞與 ASP.NET Core 的 await next；以實例 ID 觀察 Transient／Scoped／Singleton。學習 IDisposable／using、IHttpClientFactory、RequestAborted，避免把請求作用域物件交給未等待的背景任務；不額外建立背景服務。
- 習慣調整：Options 取代散落讀設定；檢查設定來源優先序。區分 required、DataAnnotations／自動 400、業務驗證與 Zod 的責任；對照舊 MVC 的 System.Web、Global.asax、web.config 與現代 host。
- 實作：以簡單活動服務拆出查詢，建立統一錯誤處理、ProblemDetails 與結構化日誌。
- 驗收：找不到活動、參數錯誤與非預期錯誤有合理回應；日誌能追查單次請求，不暴露堆疊與秘密。
- 面試練習：DbContext 為什麼通常使用 Scoped？Middleware 順序會影響什麼？

## C03：SQL Server、EF Core 與資料建模

- 預估：6～8 小時。
- 學習：Docker Compose、DbContext、Entity 設定、關聯、Migration、種子資料、非同步查詢與連線設定。
- 轉換重點：對照 Drizzle／TypeORM／EF6，觀察 EF Core change tracker、Add 與 SaveChangesAsync 的差別、AsNoTracking、投影、IQueryable 的 SQL 執行點及不可翻譯的表達式。
- 習慣調整：同一 DbContext 不用 Task.WhenAll 平行查詢；分清 Context、資料庫連線與連線池生命週期。Docker 不重教基本指令，實際練習 Compose／volume／連線與 SQL Server 特有設定。
- 實作：建立活動與報名資料結構；以 SQL Server 取代記憶體資料，員工身分暫由本機伺服器固定。
- 驗收：從空資料庫執行 Migration 與種子資料；資料重啟後仍存在；檢查 EF 產生的 SQL。
- 驗收：先建立 API 整合測試入口；資料庫行為使用真實 SQL Server，不以 EF InMemory 代替。
- 面試練習：EF Core 與以前 EF 的使用方式有哪些差異？Migration 與建置有何不同？

## C04：登入、授權與資料所有權

- 預估：6～8 小時。
- 學習：Authentication（確認你是誰）、Authorization（確認你能做什麼）、Identity、Cookie、Claims、密碼雜湊、CSRF 防護與 HTTPS。
- 轉換重點：沿用 JWT／HMAC 經驗，直接實作 AddAuthentication／AddAuthorization、Middleware 順序、Claims／Policy 與 Identity 的職責；只補 Cookie 同源流程和框架防護差異。
- 實作：預建兩個展示帳號；登入、登出、查詢自己的報名；員工 ID 從登入資訊取得。
- 驗收：未登入無法報名；修改請求中的識別值不能讀取或取消別人的資料；狀態變更請求具備 CSRF 防護。
- 驗收：只在 Development 啟用的固定身分機制，不會在公開環境生效。
- 面試練習：登入成功為什麼仍需要檢查資料所有權？Cookie 的安全設定各自解決什麼？

## C05：報名與取消，先完成業務流程

- 預估：6～8 小時。
- 學習：服務邊界、有效與已取消狀態、DTO 驗證、單元測試與整合測試的分工。
- 轉換重點：以既有狀態機與冪等經驗寫 .NET 服務；練 ActionResult<T>、ProblemDetails、xUnit 與 WebApplicationFactory，不重講一般 CRUD 或測試分類。
- 實作：報名、取消、我的紀錄；取消後可以重新報名；用少量活動與員工資料驗證。
- 驗收：成功、額滿、重複報名、取消、重複取消與重新報名皆有案例。
- 驗收：單一請求流程正確；本單元尚不宣稱通過併發驗證，C06 完成後才具備上線條件。
- 面試練習：哪些測試要連真正的資料庫？為什麼不只測 Controller 回傳的狀態碼？

## C06：交易、併發與重複請求

- 預估：8～10 小時。
- 學習：競爭條件、交易原子性、條件式 UPDATE、受影響列數、唯一限制、回滾、鎖定、死鎖與有限重試。
- 轉換重點：沿用計費、CAS 與冪等經驗，實測 SQL Server 條件更新、唯一限制與交易失敗；區分跨 await 的競爭與 .NET 多執行緒共享狀態，lock／SemaphoreSlim 都不能保護多實例資料庫業務不變量。
- 實作：在可控制的測試環境重現超收，再修正；讓名額變動與報名狀態在同一交易完成。
- 實作：用唯一限制保護同人同活動；重送報名取得一致結果，重送取消不重複還名額。
- 驗收：100 個不同員工競爭 20 個名額，正好 20 個成功；有效紀錄與名額計數一致。
- 驗收：同員工同時重送、同筆報名同時取消，以及取消與重新報名競爭，都維持核心規則。
- 驗收：名額更新後注入失敗，交易回滾，不留下已扣名額卻沒有報名的狀態。
- 面試練習：為什麼先查名額再新增會超收？為什麼單一程序的鎖不足以保護多個 API 實例？

## C07：接上前端，第一次部署 Azure

- 預估：8～10 小時。
- 學習：OpenAPI、環境設定、Docker 多階段建置、健康檢查、Azure 資源群組、Container Apps、Azure SQL、秘密設定與連線重試。
- 轉換重點：沿用聊天室前後端同容器交付經驗，實作 dotnet publish、容器監聽、反向代理與 host 關閉；以 AWS 經驗對照 Azure 的配置、權限及計費差異，保留 Dockerfile 動手練習。
- 實作：AI 依 API 契約製作 Vue 3 + TypeScript 畫面；由同一個 .NET 應用提供靜態檔與 API。
- 實作：先估價、設定資源限制，再部署低流量展示版；僅建立本單元需要的服務。
- 驗收：HTTPS 網站能完成登入、查詢、報名、取消與切換展示帳號；API 掛掉時畫面有錯誤狀態。
- 驗收：瀏覽器檢查手機版、空資料、額滿、重複送出與載入狀態；記錄冷啟動、資料庫恢復時間及用量。
- 面試練習：為什麼選 Container Apps？如何以成本與等待時間決定是否常駐？

## C08：量測與資料庫查詢優化

- 預估：6～8 小時。
- 學習：建立基準、產生測試資料、執行計畫、邏輯讀取、複合索引、篩選、分頁、投影、AsNoTracking、N+1 與連線池。
- 轉換重點：不重教索引概念；用 SQL Server 執行計畫與 EF Core SQL 日誌追查提前 ToList、重複列舉、過度載入與連線池等待。
- 實作：本機建立至少 10 萬筆報名紀錄，比較活動查詢與個人紀錄查詢；逐項改善，不一次改完。
- 驗收：同一組資料與查詢條件，保存 SQL、執行計畫、讀取量與延遲的改善前後結果；沒有改善也據實分析。
- 面試練習：索引為什麼不是越多越好？如何分辨慢在資料庫或 API？

## C09：Redis 快取與故障處理

- 預估：6～8 小時。
- 學習：Cache-aside（先讀快取，沒有再查資料庫）、Key、TTL、失效、命中率、快取擊穿與逾時。
- 轉換重點：沿用 ioredis／Valkey 與故障排查經驗，對照 .NET Redis client 的連線重用、序列化與逾時；區分記憶體快取、多實例快取與請求合併的邊界，不重做 Redis 名詞入門。
- 實作：以真實 Redis 快取活動介紹與分類列表；取消、報名與名額判斷仍以資料庫為準。
- 實作：更新測試活動後清除相關快取；大量同時未命中時，觀察資料庫負載並練習合併重建請求。
- 驗收：比較有無快取的資料庫查詢數與延遲；測試過期、修改後失效與 Redis 中斷時的退回查詢。
- 驗收：README 與設定清楚區分 Redis、無快取模式；雲端未啟用 Redis 時不宣稱使用託管快取。
- 面試練習：哪些資料值得快取？快取失效與更新競爭會造成什麼結果？

## C10：壓力測試與多實例驗證

- 預估：6～8 小時。
- 學習：k6、虛擬使用者與請求速率、吞吐量、p95、錯誤率、CPU / 記憶體、限流與背壓。
- 轉換重點：從既有真實 API／DB 驗證習慣出發，增加 .NET ThreadPool、GC、連線池與兩實例觀察，區分 blocking、CPU 運算與資料庫等待。
- 實作：分開測活動瀏覽與熱門活動報名；使用固定種子、不同員工及獨立測試資料庫。
- 實作：本機啟動兩個 API 實例，對共享 SQL Server / Redis 測試；理解單機限流與單機鎖的範圍。
- 驗收：分開統計成功、額滿等預期業務回應、限流與非預期錯誤；壓測結束後核對資料一致性。
- 驗收：記錄硬體、資料量、持續時間、負載、冷熱啟動、快取狀態與延遲；說明瓶頸與量測限制。
- 面試練習：p95 是什麼？RPS 與同時使用人數有何不同？為什麼免費雲端方案不適合直接拿來證明高容量？

## C11：GitHub Actions 與可重現部署

- 預估：8～10 小時。
- 學習：CI / CD、容器映像、OIDC 短期身分、Azure 最小權限、設定與程式分離、資料庫 Migration 的發布順序。
- 轉換重點：沿用 GitLab Flow、版本 tag 與 release 文件經驗，補 .NET build／test／publish、EF Migration 發布與 Azure OIDC／Bicep；不要求為本課改採新分支流程。
- 實作：GitHub Actions 執行建置與必要測試，發布容器映像；部署流程可手動觸發並檢查健康狀態。
- 實作：以小份 Bicep 描述實際使用的 Azure 資源，讓人工建立過的環境能重建；避免額外加入服務。
- 驗收：提交新版本後，CI 可驗證；授權執行部署後可確認版本。故意失敗的測試會阻止發布。
- 驗收：曾實際演練退回舊映像；資料庫採相容性 Migration，回復應用不等同自動回滾資料庫。
- 面試練習：為什麼避免把長期 Azure 憑證放在 repo？程式回復與資料回復有何差別？

## C12：維運、成本與履歷作品整理

- 預估：8～10 小時。
- 學習：結構化日誌、關聯 ID、健康檢查、錯誤與延遲觀察、資料備份與還原、成本檢視、展示資料重建。
- 轉換重點：對照 CloudWatch／事故 runbook 與 AWS 成本控管經驗，實作 .NET 日誌查詢、Azure 用量與復原；作品說明區分既有經驗與本課新增證據。
- 實作：在非正式環境演練資料重建與一次還原；以管理腳本維護展示資料，不提供公開重置端點。
- 實作：記錄停用／啟用 Redis 的差異；預算允許時短期建立 Azure Managed Redis，驗證 TLS、身分與連線，記帳後移除測試資源。
- 實作：整理架構圖、本機啟動、部署與復原文件、效能報告、三分鐘展示與五分鐘技術說明。
- 驗收：依 [專案完成標準](README.md#完成作品的標準) 逐項附上證據；託管 Redis 若未練習，標示未完成，不影響本機 Redis 核心課驗收。
- 驗收：以實際觀察期間的用量與帳單記錄成本；預算通知不當成自動停費機制。
- 面試練習：哪個決策最重要？遇過什麼錯誤？如果使用量增加十倍，會先量測與調整哪裡？

## 後續教材如何產出

- 先提供 C01 詳細教材，其餘單元於開始前依當時程式碼展開，避免預先寫出整套答案。
- 每堂課包含 Node.js／舊 MVC 對照、誤用案例、小步驟練習、驗收與實驗紀錄；理解以 .NET 差異的實作或除錯說明確認，不固定安排基礎口試。
- 單元成果與證據寫進 [學習進度](progress.md)，尚未執行的項目保持未完成。
- 效能、部署與維運報告在完成實驗後新增於 `docs/reports/`；不預填測試數字。
- 業務規則調整先更新範圍文件，再調整同一份課綱；不另開競爭版本。

## 版本與官方參考

2026-10-15 已核對 .NET 10 為 LTS，官方支援至 2028-11-14。安裝時使用當時最新的受支援 10.0 修補版本，EF Core 與 .NET 主版本一致。

- [.NET 支援政策](https://dotnet.microsoft.com/en-us/platform/support/policy/dotnet-core)
- [ASP.NET Core 基礎](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/?view=aspnetcore-10.0)
- [EF Core 文件](https://learn.microsoft.com/en-us/ef/core/)
- [ASP.NET Core 非同步與效能慣例](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/best-practices?view=aspnetcore-10.0)
- [DbContext 生命週期與執行緒安全](https://learn.microsoft.com/en-us/ef/core/dbcontext-configuration/)
- Azure 選型與成本來源統一見 [Azure 部署規劃](azure-deployment.md)。
