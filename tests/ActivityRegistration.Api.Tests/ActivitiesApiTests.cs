using ActivityRegistration.Api.Dtos;
using System.Net.Http.Json;

namespace ActivityRegistration.Api.Tests;

[Collection(ApiCollection.Name)]          // 跟其他測試共用同一個 ApiFactory
public class ActivitiesApiTests(ApiFactory factory)
{
    [Fact]
    public async Task GetActivities_ReturnsThreeSeededActivities()
    {
        var client = factory.CreateClient();   // 會打到記憶體裡的 API 的 HttpClient

        // 1. GET /api/activities
        var res = await client.GetAsync("/api/activities");
        // 2. 確認狀態碼是 200
        Assert.Equal(200, ((int)res.StatusCode));
        // 3. 把 JSON 讀成 List<ActivitySummaryDto>（C02 用過 GetFromJsonAsync）
        var data = await res.Content.ReadFromJsonAsync<List<ActivitySummaryDto>>();
        // 4. 確認有 3 筆
        Assert.Equal(3, data?.Count);
    }

    [Fact]
    public async Task GetActivities_ReturnsError()
    {
        var client = factory.CreateClient();   // 會打到記憶體裡的 API 的 HttpClient

        // 1. GET /api/activities
        var res1 = await client.GetAsync("/api/activities/999");
        // 2. 確認狀態碼是 404
        Assert.Equal(404, ((int)res1.StatusCode));

        // 1. GET /api/activities
        var res2 = await client.GetAsync("/api/activities/0");
        // 2. 確認狀態碼是 400
        Assert.Equal(400, ((int)res2.StatusCode));
    }
}