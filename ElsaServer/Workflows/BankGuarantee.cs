using Elsa.Workflows;
using Elsa.Workflows.Activities;
using Elsa.Workflows.Runtime.Activities;
using Elsa.Extensions;
using ElsaServer.Activities;
using ElsaServer.Models;
using Elsa.Workflows.Activities.Flowchart.Activities;
using Newtonsoft.Json;
using Elsa.Workflows.Memory;

namespace ElsaServer.Workflows
{
    //public class BankGuarantee : WorkflowBase
    //{
    //    protected override void Build(IWorkflowBuilder builder)
    //    {
    //        var currentStepPerformer = builder.WithVariable<User>();

    //        Variable<PerformerGroup[]> performerGroups = builder.WithVariable("workflowPerformerGroup", new PerformerGroup[]
    //        {
    //            new PerformerGroup(2, "BankGuaranteeExpertApprovers"),
    //            new PerformerGroup(2, "BankGuaranteeProjectManagerApprovers"),
    //            new PerformerGroup(3, "BankGuaranteeFinalApprovers")
    //        });


    //        builder.Root = new Flowchart
    //        {
    //            Activities =
    //            {
    //                new Start(),

    //                new SetVariable
    //                {
    //                    Variable = starterUser,
    //                    Value = new (context => context.GetInput<User>("StarterUser"))
    //                },

    //                new Step(2,
    //                    activePerformerGroup: new PerformerGroup(1, "BankguaranteeCreators"),
    //                    requiredFields: new Dictionary<string, object>
    //                    {
    //                        {"Name", new {FiledId = 1, Type= typeof(string), Value = "MyBankGuarantee"}}
    //                    },
    //                    nextPossibleSteps: new Dictionary<Step, PerformerGroup>
    //                    {
    //                        {new Step("ExpertApprove"),   new PerformerGroup(2, "BankGuaranteeExpertApprovers")},
    //                        {new Step("ExpertApprove"),  new PerformerGroup(3, "BankGuaranteeProjectManagerApprovers")},
    //                        {new Step("ExpertApprove"),  new PerformerGroup(3, "BankGuaranteeFinalApprovers")},

    //                        {new Step("ProjectManagerApprove"),  new PerformerGroup(3, "BankGuaranteeProjectManagerApprovers")},
    //                        {new Step("ProjectManagerApprove"),  new PerformerGroup(3, "BankGuaranteeFinalApprovers")},

    //                    },
    //                    taskName: "Create Bank Guarantee"
    //                )
    //                {
    //                    Payload = new (context => new Dictionary<string, object>
    //                    {
    //                        ["PerformerGroups"] = performerGroups.Get(context)!,
    //                        ["Description"] = "Create Bank Guarantee"
    //                    })
    //                },

    //                new End()

    //            }

    //        };
    //    }
    //}

}
