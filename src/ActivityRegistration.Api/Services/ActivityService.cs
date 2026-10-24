using ActivityRegistration.Api.Data;
using ActivityRegistration.Api.Dtos;
using Microsoft.EntityFrameworkCore;

namespace ActivityRegistration.Api.Services
{
    public class ActivityService(AppDbContext db)
    {
        public async Task<List<ActivitySummaryDto>> GetActivitiesAsync()
        {
            return await db.Activities
                .Select(x => new ActivitySummaryDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Category = x.Category,
                    Capacity = x.Capacity
                })
                .ToListAsync();
        }

        public async Task<ActivitySummaryDto?> GetActivityByIdAsync(int id)
        {
            return await db.Activities
                .Where(x => x.Id == id)
                .Select(x => new ActivitySummaryDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Category = x.Category,
                    Capacity = x.Capacity
                })
                .FirstOrDefaultAsync();
        }
    }
}
