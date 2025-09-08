using Elsa.Workflows;
using Rts.Common;
using System.Text.Json;

namespace ElsaServer.Activities
{
    public class TafahomDecision : Activity
    {
        private JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

        protected override async ValueTask ExecuteAsync(ActivityExecutionContext context)
        {
            var a = context.WorkflowInput["RunTaskInput"];
            var wfConfig = JsonSerializer.Deserialize<UserWorkflowConfig>(JsonSerializer.Serialize(a, JsonSerializerOptions), JsonSerializerOptions);
            var outcome = Convert.ToString(wfConfig!.ActivityConfig.PossibleRequiredData!)!;

            switch (outcome)
            {
                case ("approved"):
                    await context.CompleteActivityAsync("NeedsApproval");
                    break;
            }

        }
    }
}
