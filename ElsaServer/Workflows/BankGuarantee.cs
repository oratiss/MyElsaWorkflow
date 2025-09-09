using Elsa.Extensions;
using Elsa.Workflows;
using Elsa.Workflows.Activities;
using Elsa.Workflows.Activities.Flowchart.Activities;
using Elsa.Workflows.Runtime.Activities;
using ElsaServer.Activities;
using Rts.Common;
using Connection = Elsa.Workflows.Activities.Flowchart.Models.Connection;
using Endpoint = Elsa.Workflows.Activities.Flowchart.Models.Endpoint;


namespace ElsaServer.Workflows
{
    public class BankGuarantee : WorkflowBase /*: BankGuaranteeStandard*/
    {
        protected override void Build(IWorkflowBuilder builder)
        {

            var userWorkflowConfig = builder.WithVariable<UserWorkflowConfig>();

            var previousRunTasKResultAsInput = builder.WithVariable<Dictionary<string, object>>();

            // Create activities with explicit IDs
            var startActivity = new Start
            {
                Id = "start"
            };

            var setConfigActivity = new SetVariable
            {
                Id = "setConfig",
                Variable = userWorkflowConfig,
                Value = new(context =>
                {
                    var inputConfig = context.GetInput<UserWorkflowConfig>("UserWorkflowConfig");
                    return inputConfig;
                })
            };


            //Create Step with runtime evaluation using delegates

            //var stepActivity = new Step(
            //      taskName: "Create Bank Guarantee Document",
            //      null,
            //      null,
            //      description: $"This step is for creating a \"Bank Guarantee document\""
            //  )
            var createBankGuarantee= new RunTask("Create Bank Guarantee Document")
            {
                Id = "createBankGuarantee",
                Payload = new(context =>
                {
                    var resultDict = new Dictionary<string, object>();

                    var wfConfig = userWorkflowConfig.Get(context)!;
                    resultDict.Add("UserWorkflowConfig", wfConfig);
                    resultDict.Add("Description", "Create Bank Guarantee");
                    return resultDict;
                }),
            };

            var approveByExpertOrPMDecision = new TafahomDecision
            {
                Id = "TafahomDecision-BankGuarantee-01-Creation-ExpertsOrPM",
                Name = "TafahomDecision-BankGuarantee-01-Creation-ExpertsOrPM"

            };

            var approveBankGuaranteeByExpert = new RunTask("Approve Bank Guarantee By Expert")
            {
                Id = "ApproveBankGuaranteeByExpert",
                Name = "Approve Bank Guarantee By Expert",
                Payload = new(context =>
                {
                    var resultDict = new Dictionary<string, object>();

                    var wfConfig = userWorkflowConfig.Get(context)!;
                    resultDict.Add("UserWorkflowConfig", wfConfig);
                    resultDict.Add("Description", "Approve Bank Guarantee By Expert");
                    return resultDict;
                }),
            };

            var approveBankGuaranteeByPM = new RunTask("Approve Bank Guarantee By PM")
            {
                Id = "ApproveBankGuaranteeByPM",
                Name = "Approve Bank Guarantee By PM",
                Payload = new(context =>
                {
                    var resultDict = new Dictionary<string, object>();

                    var wfConfig = userWorkflowConfig.Get(context)!;
                    resultDict.Add("UserWorkflowConfig", wfConfig);
                    resultDict.Add("Description", "Approve Bank Guarantee By PM");
                    return resultDict;
                }),
            };


            var endActivity = new End
            {
                Id = "end",
                Name = "end"
            };

            builder.Root = new Flowchart
            {
                Activities =
                {
                    startActivity,
                    setConfigActivity,
                    createBankGuarantee,
                    approveByExpertOrPMDecision,
                    approveBankGuaranteeByExpert,
                    approveBankGuaranteeByPM,
                    endActivity
                },

                Connections =
                {
                    // Connect Start to SetConfig
                    new Connection
                    {
                        Source = new Endpoint
                        {
                            Activity = startActivity,
                            Port = "Done"
                        },
                        Target = new Endpoint
                        {
                            Activity = setConfigActivity,
                            Port = "In"
                        }
                    },
                    // Connect SetConfig to createBankGuarantee
                    new Connection
                    {
                        Source = new Endpoint
                        {
                            Activity = setConfigActivity,
                            Port = "Done"
                        },
                        Target = new Endpoint
                        {
                            Activity = createBankGuarantee,
                            Port = "In"
                        }
                    },
                    // Connect Step to SetResult
                    new Connection
                    {
                        Source = new Endpoint
                        {
                            Activity = createBankGuarantee,
                            Port = "Done"
                        },
                        Target = new Endpoint
                        {
                            Activity = approveByExpertOrPMDecision,
                            Port = "In"
                        }
                    },
                    new Connection
                    {
                        Source = new Endpoint
                        {
                            Activity = approveByExpertOrPMDecision,
                            Port = "Expert"
                        },
                        Target = new Endpoint
                        {
                            Activity = approveBankGuaranteeByExpert,
                            Port = "In"
                        }
                    },
                    new Connection
                    {
                        Source = new Endpoint
                        {
                            Activity = approveByExpertOrPMDecision,
                            Port = "PM"
                        },
                        Target = new Endpoint
                        {
                            Activity = approveBankGuaranteeByPM,
                            Port = "In"
                        }
                    },
                    new Connection
                    {
                        Source = new Endpoint
                        {
                            Activity = approveBankGuaranteeByPM,
                            Port = "Done"
                        },
                        Target = new Endpoint
                        {
                            Activity = endActivity,
                            Port = "In"
                        }
                    },
                    new Connection
                    {
                        Source = new Endpoint
                        {
                            Activity = approveBankGuaranteeByExpert,
                            Port = "Done"
                        },
                        Target = new Endpoint
                        {
                            Activity = endActivity,
                            Port = "In"
                        }
                    }
                }
            };


        }

    }

}
