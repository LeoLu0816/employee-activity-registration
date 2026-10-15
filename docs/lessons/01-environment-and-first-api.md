# C01：環境確認與第一支 API

狀態：教材已準備，練習尚未執行。預估 4～6 小時，可拆成數次進行。

## 本堂成果

能自行啟動 ASP.NET Core API，取得三筆活動資料，修改回傳欄位，並說明 SDK、專案、Controller 與 DTO 的用途。

本堂先使用記憶體資料，資料庫在 C03 加入。以下命令是課堂練習，尚未替學員執行。

## 1. 環境確認

在本機專案根目錄依序執行：

```powershell
git --version
dotnet --list-sdks
dotnet --info
```

完成條件：Git 可執行，SDK 清單有 `10.0.x`。如果只有 Runtime，仍需要安裝 SDK；缺少工具時先處理，再繼續。

編輯器可沿用習慣的 VS Code 或支援 .NET 10 的 Visual Studio。本堂不要求 Docker 或 Azure 帳號。

官方入口：[下載 .NET 10](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)。選擇作業系統對應的 SDK，使用當時最新受支援的 10.0 修補版本。

回報時提供版本與錯誤摘要即可，不貼包含帳號、憑證或個人目錄的完整環境資訊。

## 2. 先補必要概念

- SDK：建置與開發程式需要的工具；Runtime 負責執行程式。
- ASP.NET Core：處理 HTTP 請求的網站框架。
- `.csproj`：定義目標框架、套件與建置設定的專案檔。
- `Program.cs`：組合服務與請求流程的啟動入口。
- Controller：把 HTTP 請求連到對應的處理方法。
- DTO：API 輸入或輸出的資料形狀，本堂先練輸出。

對照舊版 MVC，先由學員找出熟悉與陌生的部分，再補 nullable、record 與集合語法。`async/await` 在 C02 說明，C03 用於真實資料庫 I/O；本堂不為了非同步而包裝記憶體資料。

## 3. 建立最小 API 專案

在專案根目錄執行：

```powershell
dotnet new webapi --use-controllers --framework net10.0 --output src/ActivityRegistration.Api
dotnet build src/ActivityRegistration.Api/ActivityRegistration.Api.csproj
dotnet run --project src/ActivityRegistration.Api/ActivityRegistration.Api.csproj --no-launch-profile --urls http://localhost:5080
```

完成條件：建置成功，終端顯示正在監聽 `http://localhost:5080`。如果連接埠被占用，換一個可用連接埠並記錄。

這裡先用本機 HTTP 排除開發憑證問題；部署課使用 HTTPS。當時範本若包含額外範例，先辨認用途，稍後只移除確定不需要的範例。

練習：找出專案檔的目標框架、Controller 註冊與路由映射；先解釋用途，再修改。

## 4. 自己寫活動列表

建立 `ActivitySummaryDto` 與 `ActivitiesController`，提供 `GET /api/activities`。

輸出是 JSON 陣列，包含三筆虛構活動，每筆有：

| 欄位 | 本堂用途 |
|---|---|
| `id` | 整數識別值 |
| `title` | 活動名稱 |
| `category` | 講座、運動或手作等分類文字 |
| `capacity` | 正整數名額 |

資料可使用「職場溝通分享／30 人」「下班羽球／16 人」「咖啡沖煮／12 人」。本堂不提供整段後端答案；先由學員嘗試，再依卡點給局部提示。

完成後在另一個 PowerShell 視窗執行：

```powershell
Invoke-RestMethod -Uri 'http://localhost:5080/api/activities'
```

完成條件：HTTP 200、三筆活動、欄位正確；修改其中一個活動名稱後，重新執行可看到變更。停止並重啟 API 後，資料仍回到程式內的預設值，能解釋原因。

## 5. 驗收與口頭解釋

- [ ] `dotnet build` 成功，能自行啟動與停止 API。
- [ ] 列表回傳三筆活動，能說明路由、HTTP 狀態碼與 JSON。
- [ ] 能修改 DTO，說明 C# 屬性與 JSON 欄位的對應。
- [ ] 能說明 SDK 與 Runtime、Controller 與 DTO 的差別。
- [ ] 能描述請求從 `Program.cs` 到 Controller 再回傳資料的流程。
- [ ] 知道這個版本沒有資料庫與身分驗證，只供本機學習。

本堂以建置與實際 HTTP 請求驗收，不新增只確認固定三筆資料的單元測試；C03 起建立整合測試。

完成後在 [學習進度](../progress.md) 記錄實際版本、指令結果、尚未理解的概念，以及下一個小步驟。學員能解釋後，再準備 C02 詳細教材。
