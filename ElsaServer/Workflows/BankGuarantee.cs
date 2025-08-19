using Elsa.Extensions;
using Elsa.Workflows;
using Elsa.Workflows.Activities;
using Elsa.Workflows.Activities.Flowchart.Activities;
using Elsa.Workflows.Memory;
using Elsa.Workflows.Models;
using ElsaServer.Activities;
using Rts.Common;

namespace ElsaServer.Workflows
{
    public class BankGuarantee : BankGuaranteeStandard
    {

        protected override void Build(IWorkflowBuilder builder)
        {

            var userWorkflowConfig = builder.WithVariable<UserWorkflowConfig>();

            Variable<BankGuaranteeState> workflowState = builder.WithVariable<BankGuaranteeState>();

            var performerUserId = builder.WithVariable<long>();

            var currentPerformerGroup = builder.WithVariable<PerformerGroup>();

            var previousRunTasKResultAsInput = builder.WithVariable<Dictionary<string, object>>();


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

                    //Todo: find a way to set  this variable 
                    new SetVariable
                    {
                        Variable = workflowState,
                        Value = new (context =>
                        {
                            return BankGuaranteeState.Created;
                        })
                    },
                    //for none standard workflows, first step should assign it values from variables defined above. Those variables can be assigned with input.
                    new Step(
                                (long)performerUserId.Value!,
                                activePerformerGroup: (PerformerGroup)currentPerformerGroup.Value!,
                                requiredFieldValues:  new Input<List<RequiredFieldValue>>(context => userWorkflowConfig.Get(context)?.FirstActivityConfig?.RequiredFieldValues!),
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
                        Variable = previousRunTasKResultAsInput,
                        Value = new (context => context.GetInput<Dictionary<string, object>>("Result"))
                    },


                    new End()

                }

            };
        }
    }

}
