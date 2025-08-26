using Elsa.Api.Client.Extensions;
using Elsa.Extensions;
using Elsa.Workflows;
using Elsa.Workflows.Activities;
using Elsa.Workflows.Activities.Flowchart.Activities;
using ElsaServer.Activities;
using ElsaServer.Models;
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
            var userWorkflowConfigDef = builder.WithInput<UserWorkflowConfig>("UserWorkflowConfig");

            //Variable<BankGuaranteeState> workflowState = builder.WithVariable<BankGuaranteeState>();

            var previousRunTasKResultAsInput = builder.WithVariable<Dictionary<string, object>>();

            NextActivityTransistionType nextActivityTransistionType = NextActivityTransistionType.None;

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

            var stepActivity = new Step(
                  taskName: "Create Bank Guarantee Document",
                  null,
                  null,
                  description: $"This step is for creating a \"Bank Guarantee document\""
              )
            {
                Id = "createBankGuarantee",
                Payload = new(context =>
                {
                    nextActivityTransistionType = NextActivityTransistionType.SelectByLogic;
                    var resultDict = new Dictionary<string, object>();

                    var userWorkFlowConfig = userWorkflowConfig.Get(context)!;
                    resultDict.Add("UserWorkflowConfig", userWorkFlowConfig);
                    resultDict.Add("Description", "Create Bank Guarantee");
                    resultDict.Add("NextActivityTransistionType", nextActivityTransistionType);

                    return resultDict;
                }),
            };

            var setResultActivity = new SetVariable
            {
                Id = "setResult",
                Variable = previousRunTasKResultAsInput,
                Value = new(context =>
                {
                    var input = context.GetWorkflowExecutionContext().Input;
                    var runTaskInput = input["RunTaskInput"];
                    var valueToBeSet = runTaskInput.ConvertTo<ResumedTaskResult>();
                    return valueToBeSet;
                })
            };

            var endActivity = new End
            {
                Id = "end"
            };

            builder.Root = new Flowchart
            {
                Activities =
                {
                    startActivity,
                    setConfigActivity,
                    stepActivity,
                    setResultActivity,
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
                    // Connect SetConfig to Step
                    new Connection
                    {
                        Source = new Endpoint
                        {
                            Activity = setConfigActivity,
                            Port = "Done"
                        },
                        Target = new Endpoint
                        {
                            Activity = stepActivity,
                            Port = "In"
                        }
                    },
                    // Connect Step to SetResult
                    new Connection
                    {
                        Source = new Endpoint
                        {
                            Activity = stepActivity,
                            Port = "Done"
                        },
                        Target = new Endpoint
                        {
                            Activity = setResultActivity,
                            Port = "In"
                        }
                    },
                    // Connect SetResult to End
                    new Connection
                    {
                        Source = new Endpoint
                        {
                            Activity = setResultActivity,
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
