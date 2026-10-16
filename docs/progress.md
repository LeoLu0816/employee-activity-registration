# 學習進度

更新日期：2026-10-16（Asia/Taipei）。

## Checkpoint

- phase：C01 API 基線與差異概念導讀已完成；學員要求略過已懂的實驗，接續 C02。
- current_module：C02，DI 生命週期導讀。
- next_step：新 session 先讀 learning-guide.md 與本 checkpoint；DI 三種生命週期及 Singleton 注意事項已導讀，接著引導學員以實例 ID 觀察同請求與跨請求的物件重用；尚未開始服務拆分實作。不要重考 HTTP、JSON、DTO 或一般前後端資料流。
- 教材入口：[課綱 C02](curriculum.md#c02請求流程與框架基礎)；C02 詳細教材尚未建立，先依課綱逐步教學。
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
| C02 | 進行中 | DI 生命週期導讀；服務拆分與實例 ID 實驗尚未執行 |
| C03 | 未開始 | — |
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

## 尚未確認

- 學員每週可投入時間；課綱先依單元驗收推進。
- Azure 訂用帳戶、部署區域與月費上限；於 C07 建立資源前確認。
- Docker 狀態；於 C03 依需要確認。

## 更新規則

每堂課結束更新 current_module、next_step、驗收指令與結果，以及學員仍需解釋的問題。只記實際執行的結果；規劃、AI 已產碼、學員已完成與已部署分開標示。
