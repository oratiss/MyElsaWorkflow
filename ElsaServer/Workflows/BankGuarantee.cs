using Elsa.Extensions;
using Elsa.Workflows;
using Elsa.Workflows.Activities;
using Elsa.Workflows.Activities.Flowchart.Activities;
using Elsa.Workflows.Memory;
using Elsa.Workflows.Models;
using ElsaServer.Activities;
using ElsaServer.Models;
using Polly;

namespace ElsaServer.Workflows
{
    public enum BankGuaranteeState
    {
        None = 1,

        Created = 5,

        ApprovedByExpert = 10,
        RejectedByExpert = 12,

        ApprovedByPM = 15,
        RejectedByPm = 17
    }

    public class BankGuarantee : BankGuaranteeStandard
    {

        protected override void Build(IWorkflowBuilder builder)
        {

            var userWorkflowConfig = builder.WithVariable<UserWorkflowConfig>();

            Variable<BankGuaranteeState> workflowState = builder.WithVariable<BankGuaranteeState>();

            var performerUserId = builder.WithVariable<long>();

            var currentPerformerGroup = builder.WithVariable<PerformerGroup>();


            builder.Root = new Flowchart
            {
                Activities =
                {
                    new Start(),

                    new SetVariable
                    {
                        Variable = userWorkflowConfig,
                        Value = new (context => context.GetInput<UserWorkflowConfig>("UserWorkflowConfig"))
                    },

                    new SetVariable
                    {
                        Variable = performerUserId,
                        Value = new (context => userWorkflowConfig.Get(context)!.FirstActivityConfig.CurrentPerformerUser.Id)
                    },

                    new SetVariable
                    {
                        Variable = currentPerformerGroup,
                        Value = new (context=> userWorkflowConfig.Get(context)?.FirstActivityConfig.CurrentPerformerGroup)
                    },

                    //for none standard workflows, first step should assign it values from variables defined above. Those variables can be assigned with input.
                    new Step(
                                (long)performerUserId.Value!,
                                activePerformerGroup: (PerformerGroup)currentPerformerGroup.Value!,
                                requiredFields:  new Input<List<RequiredField>>(context => userWorkflowConfig.Get(context)?.FirstActivityConfig?.RequiredFields!),
                                nextPossibleActivityIds: new Input<List<string>>(async context =>
                                {
                                    var defId = context.GetWorkflowExecutionContext().Workflow.DefinitionHandle.DefinitionId;
                                    var nextActivityId = userWorkflowConfig.Get(context)?.FirstActivityConfig.NextActivityId;
                                    return new List<string>()
                                    {
                                        (await context.GetRequiredService<IRtsActivityFinder>()
                                        .FindActivityAsync(definitionId: defId! , activityId: nextActivityId!))!.Id
                                    };
                                }),
                                taskName: "Create Bank Guarantee Document",
                                null,
                                null,
                                description: $"This step is for creating a \"Bank Guarantee document\", "
                            )
                    {
                        Payload = new (context => new Dictionary<string, object>
                        {
                            ["UserWorkflowConfig"] = userWorkflowConfig.Get(context)!,
                            ["Description"] = "Create Bank Guarantee"
                        })
                    },

                    new SetVariable
                    {
                        Variable = workflowState,
                        Value = new (context =>
                        {
                            return BankGuaranteeState.Created;
                        })
                    },

                    new End()

                }

            };
        }
    }

}
