# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## 這個 repo 是什麼

**教學專案**，不是一般產品開發。學員（有 Node.js／TS 後端與舊版 ASP.NET MVC／EF6 經驗）透過「企業內部活動報名系統」學現代 .NET 後端。AI 是教練與審查者：

- **後端核心（Controller／Service／SQL／交易／快取）由學員自己寫**。AI 給提示、局部範例、審查；學員明確要求時才代寫，並補閱讀／修改練習。
- 前端（Vue 3 + TS）、重複樣板、測試資料、環境設定可由 AI 產出。
- 一次只推進目前單元，不展開後續單元，不一次做完整系統。
- 教學格式：既有經驗（Node.js／舊 MVC）→ .NET 差異 → 常見誤用 → 一個實作或除錯驗證。不重講 HTTP／JSON／CRUD／Git 等通識。

## 每次開場先讀

1. `docs/progress.md` — checkpoint，從 `next_step` 接續，不重做已完成單元。
2. `docs/learning-guide.md` — 分工、教學流程、git 授權。
3. 當前單元在 `docs/curriculum.md` 的段落（C01～C12）；有詳細教材的在 `docs/lessons/`。

產品範圍、業務規則、API 與技術方向以 `docs/README.md` 為準；設計規則屬教學內容，寫在 `docs/`，不寫進本檔。程式與文件規劃不一致時，先跟學員確認，不自行改。Azure 規劃見 `docs/azure-deployment.md`（C07 前不建任何雲端資源）。

## 指令

專案檔在 `src/ActivityRegistration.Api/`（.NET 10，`net10.0`），`.slnx` 也在同目錄。

```bash
# 建置
dotnet build src/ActivityRegistration.Api/ActivityRegistration.Api.csproj --no-restore

# 學員的 API 程序在跑時，一般 build 會因 apphost 被鎖出 MSB3026 警告；不要停學員程序，改用獨立輸出：
dotnet build src/ActivityRegistration.Api/ActivityRegistration.Api.csproj --no-restore -p:UseAppHost=false -o "$TEMP/employee-activity-registration-check"

# 執行（http profile：http://localhost:5152，Development）
dotnet run --project src/ActivityRegistration.Api
```

- 手動呼叫範例：`src/ActivityRegistration.Api/ActivityRegistration.Api.http`。
- 提交前跑 `git diff --check`。

## 紀錄規則

- `progress.md` 只記**實際執行**的結果，並分開標示：AI 產碼、AI 實跑驗證、學員回報、未驗證。不能把「AI 寫了」記成「學員學會」。
- 詳細實驗寫 `docs/reports/`，progress 只放連結。進度只留目前定案，歷史交給 git。
- 改既有檔案時保留該檔原本的編碼（有無 BOM）與換行；`.md` 一律 LF。

## Git

- 學員已授權 AI **自行決定 commit 與 push 時機**（以單元或可獨立驗證的成果為單位），不用逐次問。Conventional Commits + 中文描述。
- 不 force push、不開分支、不部署。不提交秘密、`.vs/`、建置產物。
