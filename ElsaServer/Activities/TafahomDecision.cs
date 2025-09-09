using Elsa.Extensions;
using Elsa.Workflows;
using Elsa.Workflows.Activities.Flowchart.Attributes;
using Rts.Common;
using System.Text.Json;

namespace ElsaServer.Activities
{
    [FlowNode("Expert", "PM")]
    public class TafahomDecision : Activity
    {
        private JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

        protected override async ValueTask ExecuteAsync(ActivityExecutionContext context)
        {
            var a = context.WorkflowInput["RunTaskInput"];
            var wfConfig = JsonSerializer.Deserialize<UserWorkflowConfig>(JsonSerializer.Serialize(a, JsonSerializerOptions), JsonSerializerOptions);
            var wfConfigVariable = context.SetVariable("userWorkflowConfig", wfConfig);
            var outcome = Convert.ToString(wfConfig!.ActivityConfig.PossibleRequiredData!)!;

            switch (outcome)
            {
                case "ApproveBankGuaranteeByExpert":
                default:
                    await context.CompleteActivityWithOutcomesAsync("Expert");
                    break;

                case "ApproveBankGuaranteeByPM":
                    await context.CompleteActivityWithOutcomesAsync("PM");
                    break;
            }

        }
    }
}
