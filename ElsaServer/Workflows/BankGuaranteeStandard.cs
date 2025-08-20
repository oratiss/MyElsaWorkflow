//using Elsa.Expressions.Helpers;
//using Elsa.Extensions;
//using Elsa.Workflows;
//using Elsa.Workflows.Activities;
//using Elsa.Workflows.Activities.Flowchart.Activities;
//using Elsa.Workflows.Memory;
//using Elsa.Workflows.Models;
//using ElsaServer.Activities;
//using Rts.Common;
//using Rts.Common.BankGuaranteeModels;

//namespace ElsaServer.Workflows
//{
//    public class BankGuaranteeStandard : WorkflowBase
//    {
//        protected override void Build(IWorkflowBuilder builder)
//        {
//            Variable<UserWorkflowConfig> userWorkflowConfig = GetWorkflowConfig(builder);

//            Variable<BankGuaranteeStandardState> workflowState = builder.WithVariable<BankGuaranteeStandardState>();


//            builder.Root = new Flowchart
//            {
//                Activities =
//                {
//                    new Start(),

//                    new SetVariable
//                    {
//                        Variable = userWorkflowConfig,
//                        Value = new (userWorkflowConfig.Get)
//                    },

//                    //for standard workflows, first step should assign it values from variables defined above. Those variables has nothing to do with input except nextActivityId chosen by user
//                    new Step(
//                        1,
//                        activePerformerGroup: new UserGroup(1, "BankguaranteeCreators"),
//                        requiredFieldValues:  new Input<List<RequiredFieldValue>>(context => userWorkflowConfig.Get(context)?.FirstActivityConfig?.RequiredFieldValues!),
//                        taskName: "Create Bank Guarantee"
//                    )
//                    {
//                        Payload = new (context => new Dictionary<string, object>
//                        {
//                            ["UserWorkflowConfig"] = userWorkflowConfig.Get(context)!,
//                            ["Description"] = "Create Bank Guarantee"
//                        })
//                    },

//                    new SetVariable
//                    {
//                        Variable = workflowState,
//                        Value = new (BankGuaranteeStandardState.Created)
//                    },

//                    new End()

//                }

//            };
//        }

//        protected virtual Variable<UserWorkflowConfig> GetWorkflowConfig(IWorkflowBuilder builder)
//        {
//            var workflowConfig = builder.WithVariable("someWorkflowConfig", new UserWorkflowConfig
//            {
//                AssignableUserGroups = GetPerformerGroups(builder).Value.ConvertTo<List<UserGroup>>()!,
//                FirstActivityConfig = GetUserActivityConfig(builder).Value.ConvertTo<UserActivityConfig>()!
//            });
//            return workflowConfig;
//        }

//        protected virtual Variable<UserGroup[]> GetPerformerGroups(IWorkflowBuilder builder)
//        {
//            var performerGroups = builder.WithVariable("workflowPerformerGroup", new UserGroup[]
//            {
//                new UserGroup(1, "Everyone"),
//                new UserGroup(2, "BankGuaranteeExpertApprovers"),
//                new UserGroup(2, "BankGuaranteeProjectManagerApprovers"),
//                new UserGroup(3, "BankGuaranteeFinalApprovers")
//            });
//            return performerGroups;
//        }

//        protected virtual Variable<UserActivityConfig> GetUserActivityConfig(IWorkflowBuilder builder)
//        {
//            var userActivityConfig = builder.WithVariable("userActivityConfig", new UserActivityConfig
//            (
//                new UserGroup(1, "Everyone"),
//                new User(1, "Masoud", "Asgarian"),
//                GetrEquiredFields(builder).Value.ConvertTo<List<RequiredFieldValue>>()
//            ));
//            return userActivityConfig;
//        }

//        protected virtual Variable<RequiredFieldValue[]> GetrEquiredFields(IWorkflowBuilder builder)
//        {
//            var title = new RequiredFieldValue("Title", typeof(string));
//            title.SetValue("Standard Bank Guarantee Title");

//            var amount = new RequiredFieldValue("Amount", typeof(decimal));
//            amount.SetValue((decimal)10_000_000_000);

//            var amountCurrency = new RequiredFieldValue("AmountCurrency", typeof(string));
//            amountCurrency.SetValue("IRR");

//            var requiredFileds = builder.WithVariable("requiredFileds", new RequiredFieldValue[]
//            {
//                title,
//                amount,
//                amountCurrency
//            });

//            return requiredFileds;
//        }

//    }

//}
