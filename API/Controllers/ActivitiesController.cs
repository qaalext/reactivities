using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application;
using Application.Activities;
using Application.Core;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ActivitiesController : BaseApiController
    {


        [HttpGet]
        public async Task<ActionResult<List<Activity>>> GetActivities([FromQuery]ActivityParams param) 
        {
            return HandlePagedResult(await Mediator.Send(new List.Query{Params = param}));
        }
        
        //[Authorize] // due to the new setup from the startup we don't need this anymore
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAcivity(Guid id) 
        {
            var result = await Mediator.Send(new Details.Query{Id = id});
            return HandleResult(result);
        }
        [HttpPost]
        public async Task<IActionResult> CreateActivity(Activity activity) 
        {
            return HandleResult(await Mediator.Send(new Create.Command{Activity = activity}));
        }
        [Authorize(Policy ="IsActivityHost")]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditActivity(Guid id, Activity activity) 
        {
            activity.Id = id;
            return HandleResult(await Mediator.Send(new Edit.Command{Activity = activity}));
        }

        [Authorize(Policy ="IsActivityHost")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivity(Guid id) 
        {
            return HandleResult(await Mediator.Send(new Delete.Command{Id = id}));
        }


        [HttpPost("{id}/attend")]
        public async Task<IActionResult> Attend(Guid id)
        {
            return HandleResult(await Mediator.Send(new UpdateAttendance.Command{Id = id}));
        }

    }
}