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
        public IEnumerable<ActivitySummaryDto> Get()
        {
            return activityService.GetActivities();
        }

        [HttpGet("{id:int}")]
        public ActionResult<ActivitySummaryDto> GetById(
            [Range(1, int.MaxValue)] int id
        )
        {
            var activity = activityService.GetActivityById(id);

            if (activity is null)
            {
                return NotFound();
            }

            return Ok(activity);
        }

    }
}
