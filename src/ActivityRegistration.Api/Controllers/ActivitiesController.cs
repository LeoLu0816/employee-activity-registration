using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using ActivityRegistration.Api.Dtos;
using ActivityRegistration.Api.Services;

namespace ActivityRegistration.Api.Controllers
{
    [ApiController]
    [Route("api/activities")]
    public class ActivitiesController(
        ActivityService activityService
    ) : ControllerBase
    {
        [HttpGet]
        public async Task<List<ActivitySummaryDto>> Get()
        {
            return await activityService.GetActivitiesAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ActivitySummaryDto>> GetById(
            [Range(1, int.MaxValue)] int id
        )
        {
            var activity = await activityService.GetActivityByIdAsync(id);

            if (activity is null)
            {
                return NotFound();
            }

            return Ok(activity);
        }

    }
}
