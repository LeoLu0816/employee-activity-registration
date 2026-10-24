using ActivityRegistration.Api.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ActivityRegistration.Api.Tests;

/// <summary>
/// 整合測試用的 API 啟動器：在記憶體中啟動整個 API，並改連獨立的測試資料庫。
/// 流程：1.沿用開發環境連線字串，只把資料庫名稱換成測試用 2.測試開始前刪庫並跑 Migration 重建
/// </summary>
public class ApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    // 測試專用資料庫名稱，與開發用的 ActivityRegistration 分開，避免弄亂開發資料
    private const string TestDatabaseName = "ActivityRegistration_Test";

    /// <summary>
    /// 覆寫 API 的設定：把連線字串的資料庫換成測試資料庫。
    /// </summary>
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, config) =>
        {
            // 先讀出 API 原本合併後的設定（含 appsettings.Development.json），只替換資料庫名稱
            var original = config.Build().GetConnectionString("Default");
            var connection = new SqlConnectionStringBuilder(original)
            {
                InitialCatalog = TestDatabaseName,
            };

            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:Default"] = connection.ConnectionString,
            });
        });
    }

    /// <summary>
    /// 所有測試開始前執行一次：刪除測試資料庫，再從空資料庫套用全部 Migration（含種子資料）。
    /// </summary>
    public async Task InitializeAsync()
    {
        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await db.Database.EnsureDeletedAsync();
        await db.Database.MigrateAsync();
    }

    /// <summary>
    /// xUnit 的非同步清理；資源由 WebApplicationFactory 自己釋放，這裡不需額外處理。
    /// </summary>
    Task IAsyncLifetime.DisposeAsync()
    {
        return Task.CompletedTask;
    }
}

/// <summary>
/// 共用同一個 ApiFactory 的測試集合：集合內的測試類別依序執行，避免同時改同一個測試資料庫。
/// 用法：測試類別加上 [Collection(ApiCollection.Name)]，建構函式接收 ApiFactory。
/// </summary>
[CollectionDefinition(Name)]
public class ApiCollection : ICollectionFixture<ApiFactory>
{
    public const string Name = "Api";
}
