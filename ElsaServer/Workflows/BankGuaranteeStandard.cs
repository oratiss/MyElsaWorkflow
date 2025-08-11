using Elsa.Expressions.Helpers;
using Elsa.Extensions;
using Elsa.Workflows;
using Elsa.Workflows.Activities;
using Elsa.Workflows.Activities.Flowchart.Activities;
using Elsa.Workflows.Memory;
using Elsa.Workflows.Models;
using ElsaServer.Activities;
using ElsaServer.Models;

namespace ElsaServer.Workflows
{
    public enum BankGuaranteeStandardState
    {
        None = 1,

        Created = 5,
        
        ApprovedByExpert = 10,
        RejectedByExpert = 12,
        
        ApprovedByPM = 15,
        RejectedByPm = 17
    }

    public class BankGuaranteeStandard : WorkflowBase
    {
        protected override void Build(IWorkflowBuilder builder)
        {
            Variable<UserWorkflowConfig> userWorkflowConfig = GetWorkflowConfig(builder);

            Variable<BankGuaranteeStandardState> workflowState = builder.WithVariable<BankGuaranteeStandardState>();


            builder.Root = new Flowchart
            {
                Activities =
                {
                    new Start(),

                    new SetVariable
                    {
                        Variable = userWorkflowConfig,
                        Value = new (userWorkflowConfig.Get)
                    },

                    //for standard workflows, first step should assign it values from variables defined above. Those variables has nothing to do with input except nextActivityId chosen by user
                    new Step(
                        1,
                        activePerformerGroup: new PerformerGroup(1, "BankguaranteeCreators"),
                        requiredFields:  new Input<List<RequiredField>>(context => userWorkflowConfig.Get(context)?.FirstActivityConfig?.RequiredFields!),
                        nextPossibleActivityIds: new Input<List<string>>(context => new List<string> { userWorkflowConfig.Get(context)?.FirstActivityConfig.NextActivityId!}),
                        taskName: "Create Bank Guarantee"
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
                        Value = new (BankGuaranteeStandardState.Created)
                    },

                    new End()

                }

            };
        }

        protected virtual Variable<UserWorkflowConfig> GetWorkflowConfig(IWorkflowBuilder builder)
        {
            var workflowConfig = builder.WithVariable("someWorkflowConfig", new UserWorkflowConfig
            {
                PerformerGroups = GetPerformerGroups(builder).Value.ConvertTo<List<PerformerGroup>>(),
                FirstActivityConfig = GetUserActivityConfig(builder).Value.ConvertTo<UserActivityConfig>()!
            });
            return workflowConfig;
        }

        protected virtual Variable<PerformerGroup[]> GetPerformerGroups(IWorkflowBuilder builder)
        {
            var performerGroups = builder.WithVariable("workflowPerformerGroup", new PerformerGroup[]
            {
                new PerformerGroup(1, "Everyone"),
                new PerformerGroup(2, "BankGuaranteeExpertApprovers"),
                new PerformerGroup(2, "BankGuaranteeProjectManagerApprovers"),
                new PerformerGroup(3, "BankGuaranteeFinalApprovers")
            });
            return performerGroups;
        }

        protected virtual Variable<UserActivityConfig> GetUserActivityConfig(IWorkflowBuilder builder)
        {
            var userActivityConfig = builder.WithVariable("userActivityConfig", new UserActivityConfig
            {
                CurrentPerformerGroup = new PerformerGroup(1, "Everyone"),
                CurrentPerformerUser = new User(1, "Masoud", "Asgarian"),
                Decision = null,
                NextActivityId = "2",
                RequiredFields = GetrEquiredFields(builder).Value.ConvertTo<List<RequiredField>>()
            });
            return userActivityConfig;
        }

        protected virtual Variable<RequiredField[]> GetrEquiredFields(IWorkflowBuilder builder)
        {
            var title = new RequiredField("Title", typeof(string));
            title.SetValue("Standard Bank Guarantee Title");

            var amount = new RequiredField("Amount", typeof(decimal));
            amount.SetValue((decimal)10_000_000_000);

            var amountCurrency = new RequiredField("AmountCurrency", typeof(string));
            amountCurrency.SetValue("IRR");

            var requiredFileds = builder.WithVariable("requiredFileds", new RequiredField[]
            {
                title,
                amount,
                amountCurrency
            });

            return requiredFileds;
        }

    }

}
