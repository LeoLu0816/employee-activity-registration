using ActivityRegistration.Api.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace ActivityRegistration.Api.Controllers
{
    [ApiController]
    [Route("api/activities")]
    public class ActivitiesController : ControllerBase
    {

        [HttpGet]
        public IEnumerable<ActivitySummaryDto> Get()
        {
            var list = new List<ActivitySummaryDto>();
            list.Add(new ActivitySummaryDto()
            {
                Id = 1,
                Title = "技術分享",
                Category = "講座",
                Capacity = 30,
            });

            list.Add(new ActivitySummaryDto()
            {
                Id = 2,
                Title = "下班羽球",
                Category = "運動",
                Capacity = 16,
            });

            list.Add(new ActivitySummaryDto()
            {
                Id = 3,
                Title = "咖啡沖煮",
                Category = "手作",
                Capacity = 12,
            });

            return list;
        }
    }
}
