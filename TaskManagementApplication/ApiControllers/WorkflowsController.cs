using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rts.Common;
using System.Threading.Tasks;
using TaskManagementApplication.ApiControllers.ApiModels;
using TaskManagementApplication.Data;
using TaskManagementApplication.Entities;
using TaskManagementApplication.Services;

namespace TaskManagementApplication.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkflowsController(IElsaClient elsaClient, TaskManagementDbContext dbContext) : ControllerBase
    {

        [HttpGet]
        public List<WorkflowEntities> Index()
        {
            return new();
        }

        [HttpPost("{id:long}")]
        public async Task<ActionResult> RunWorkflow([FromRoute] long id, [FromBody] RunWorkflowRequest runWorkflowRequest)
        {
            var workFlow = dbContext.Workflows.Find(id);
            if (workFlow is null)
            {
                return NotFound();
            }

            if (string.IsNullOrWhiteSpace(workFlow.ElsaWorkflowDefinitionId)) 
            {
                return NotFound("One required data is missing in system. Contact the system admisitrator.");
            }

            UserWorkflowConfig workflowConfig = new(); //Todo fill it from database and request

            await elsaClient.RunWorkflowAsync(workFlow.ElsaWorkflowDefinitionId, workflowConfig);

            return Ok();
        }
    }
}
