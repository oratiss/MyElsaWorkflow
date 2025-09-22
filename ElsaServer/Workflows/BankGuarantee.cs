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
            var createBankGuarantee = new RunTask("Create Bank Guarantee Document")
            {
                Id = "CreateBankGuarantee",
                Name = "Create Bank Guarantee",
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

            var approveByAnotherExpertOrPMDecision = new TafahomDecision
            {
                Id = "TafahomDecision-BankGuarantee-02-AnotherExpertsOrPM",
                Name = "TafahomDecision-BankGuarantee-02-AnotherExpertsOrPM"
            };

            var approveBankGuaranteeByAnotherExpert = new RunTask("Approve Bank Guarantee By Another Expert")
            {
                Id = "ApproveBankGuaranteeByAnotherExpert",
                Name = "Approve Bank Guarantee By Another Expert",
                Payload = new(context =>
                {
                    var resultDict = new Dictionary<string, object>();

                    var wfConfig = userWorkflowConfig.Get(context)!;
                    resultDict.Add("UserWorkflowConfig", wfConfig);
                    resultDict.Add("Description", "Approve Bank Guarantee By Another Expert");
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

            var projectManagerDecision = new TafahomDecision
            {
                Id = "TafahomDecision-BankGuarantee-03-projectManagerDecision",
                Name = "TafahomDecision-BankGuarantee-03-projectManagerDecision"
            };

            var approveBankGuaranteeByLegalAndContractAffairsManager = new RunTask("Approve Bank Guarantee By Legal And Contract Affairs Manager")
            {
                Id = "ApproveBankGuaranteeByLegalAndContractAffairsManager",
                Name = "Approve Bank Guarantee By Legal And Contract Affairs Manager",
                Payload = new(context =>
                {
                    var resultDict = new Dictionary<string, object>();

                    var wfConfig = userWorkflowConfig.Get(context)!;
                    resultDict.Add("UserWorkflowConfig", wfConfig);
                    resultDict.Add("Description", "Approve Bank Guarantee By Legal And Contract Affairs Manager");
                    return resultDict;
                }),
            };

            var legalAndContractAffairsManagerDecision = new TafahomDecision
            {
                Id = "TafahomDecision-BankGuarantee-04-LegalAndContractAffairsManagerDecision",
                Name = "TafahomDecision-BankGuarantee-04-LegalAndContractAffairsManagerDecision"
            };

            var approveBankGuaranteeByLegalAndContractAffairsExpert = new RunTask("Approve Bank Guarantee By LegalAndContractAffairs Expert")
            {
                Id = "ApproveBankGuaranteeByLegalAndContractAffairsExpert",
                Name = "Approve Bank Guarantee By LegalAndContractAffairs Expert",
                Payload = new(context =>
                {
                    var resultDict = new Dictionary<string, object>();

                    var wfConfig = userWorkflowConfig.Get(context)!;
                    resultDict.Add("UserWorkflowConfig", wfConfig);
                    resultDict.Add("Description", "Approve Bank Guarantee By LegalAndContractAffairs Expert");
                    return resultDict;
                }),
            };

            var reviewBankGuaranteeByFinancialManager = new RunTask("Review Bank Guarantee By Financial Manager")
            {
                Id = "ReviewBankGuaranteeByFinancialManager",
                Name = "Review Bank Guarantee By Financial Manager",
                Payload = new(context =>
                {
                    var resultDict = new Dictionary<string, object>();

                    var wfConfig = userWorkflowConfig.Get(context)!;
                    resultDict.Add("UserWorkflowConfig", wfConfig);
                    resultDict.Add("Description", "Review Bank Guarantee By Financial Manager");
                    return resultDict;
                }),
            };

            var fiancialManagerDecision = new TafahomDecision
            {
                Id = "TafahomDecision-BankGuarantee-05-FinancialManagerDecision",
                Name = "TafahomDecision-BankGuarantee-05-FinancialManagerDecision"
            };

            var reviewBankGuaranteeByFinancialExpert = new RunTask("Review Bank Guarantee By Financial Expert")
            {
                Id = "ReviewBankGuaranteeByFinancialExpert",
                Name = "Review Bank Guarantee By Financial Expert",
                Payload = new(context =>
                {
                    var resultDict = new Dictionary<string, object>();

                    var wfConfig = userWorkflowConfig.Get(context)!;
                    resultDict.Add("UserWorkflowConfig", wfConfig);
                    resultDict.Add("Description", "Review Bank Guarantee By Financial Expert");
                    return resultDict;
                }),
            };

            var financialExpertDecision = new TafahomDecision
            {
                Id = "TafahomDecision-BankGuarantee-06-FinancialExpertDecision",
                Name = "TafahomDecision-BankGuarantee-06-FinancialExpertDecision"
            };

            var deliverBankGuarantee = new RunTask("Deliver Bank Guarantee")
            {
                Id = "DeliverBankGuarantee",
                Name = "Deliver Bank Guarantee",
                Payload = new(context =>
                {
                    var resultDict = new Dictionary<string, object>();

                    var wfConfig = userWorkflowConfig.Get(context)!;
                    resultDict.Add("UserWorkflowConfig", wfConfig);
                    resultDict.Add("Description", "Deliver Bank Guarantee");
                    return resultDict;
                }),
            };

            var lastDecision = new TafahomDecision
            {
                Id = "TafahomDecision-BankGuarantee-07-LastDecision",
                Name = "TafahomDecision-BankGuarantee-07-LastDecision"
            };

            var endActivity = new End
            {
                Id = "End",
                Name = "End"
            };

            builder.Root = new Flowchart
            {
                Activities =
                {
                    startActivity,
                    setConfigActivity,
                    createBankGuarantee,
                    approveByExpertOrPMDecision, //decsion
                    approveBankGuaranteeByExpert,
                    approveBankGuaranteeByPM,
                    approveByAnotherExpertOrPMDecision, //decsion
                    approveBankGuaranteeByAnotherExpert,
                    projectManagerDecision, //decsion
                    approveBankGuaranteeByLegalAndContractAffairsManager,
                    reviewBankGuaranteeByFinancialManager,
                    legalAndContractAffairsManagerDecision, //decsion
                    approveBankGuaranteeByLegalAndContractAffairsExpert,
                    fiancialManagerDecision, //decsion
                    reviewBankGuaranteeByFinancialExpert,
                    financialExpertDecision, //decsion
                    deliverBankGuarantee,
                    lastDecision, //decsion
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
                        Source= new Endpoint
                        {
                            Activity = approveBankGuaranteeByExpert,
                            Port = "Done"
                        },
                        Target = new Endpoint
                        {
                            Activity = approveByAnotherExpertOrPMDecision,
                            Port = "In"
                        }
                    },
                    new Connection
                    {
                        Source= new Endpoint
                        {
                            Activity = approveByAnotherExpertOrPMDecision,
                            Port = "AnotherExpert"
                        },
                        Target = new Endpoint
                        {
                            Activity = approveBankGuaranteeByAnotherExpert,
                            Port = "In"
                        }
                    },
                    new Connection
                    {
                        Source= new Endpoint
                        {
                            Activity = approveByAnotherExpertOrPMDecision                                                                                                                                    ,
                            Port = "ProjectManager"
                        },
                        Target = new Endpoint
                        {
                            Activity = approveBankGuaranteeByPM,
                            Port = "In"
                        }
                    },
                    new Connection
                    {
                        Source= new Endpoint
                        {
                            Activity = approveBankGuaranteeByAnotherExpert,
                            Port = "Done"
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
                            Activity = approveBankGuaranteeByPM,
                            Port = "Done"
                        },
                        Target = new Endpoint
                        {
                            Activity = projectManagerDecision,
                            Port = "In"
                        }
                    },
                    new Connection
                    {
                        Source = new Endpoint
                        {
                            Activity = projectManagerDecision,
                            Port = "BackToExpert"
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
                            Activity = projectManagerDecision,
                            Port = "LegalAndContractManager"
                        },
                        Target = new Endpoint
                        {
                            Activity = approveBankGuaranteeByLegalAndContractAffairsManager,
                            Port = "In"
                        }
                    },
                    new Connection
                    {
                        Source = new Endpoint
                        {
                            Activity = projectManagerDecision,
                            Port = "FinancialManager"
                        },
                        Target = new Endpoint
                        {
                            Activity = reviewBankGuaranteeByFinancialManager,
                            Port = "In"
                        }
                    },
                    new Connection
                    {
                        Source = new Endpoint
                        {
                            Activity = approveBankGuaranteeByLegalAndContractAffairsManager,
                            Port = "Done"
                        },
                        Target = new Endpoint
                        {
                            Activity = legalAndContractAffairsManagerDecision,
                            Port = "In"
                        },
                    },
                    new Connection
                    {
                        Source = new Endpoint
                        {
                            Activity = legalAndContractAffairsManagerDecision,
                            Port = "RejectToPM"
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
                            Activity = legalAndContractAffairsManagerDecision,
                            Port = "LegalAndContractAffairsExpert"
                        },
                        Target = new Endpoint
                        {
                            Activity = approveBankGuaranteeByLegalAndContractAffairsExpert,
                            Port = "In"
                        }
                    },
                    new Connection
                    {
                        Source = new Endpoint
                        {
                            Activity = legalAndContractAffairsManagerDecision,
                            Port = "FinancialManager"
                        },
                        Target = new Endpoint
                        {
                            Activity = reviewBankGuaranteeByFinancialManager,
                            Port = "In"
                        }
                    },
                    new Connection
                    {
                        Source = new Endpoint
                        {
                            Activity = approveBankGuaranteeByLegalAndContractAffairsExpert,
                            Port = "Done"
                        },
                        Target = new Endpoint
                        {
                            Activity = approveBankGuaranteeByLegalAndContractAffairsManager,
                            Port = "In"
                        }
                    },
                    new Connection
                    {
                        Source = new Endpoint
                        {
                            Activity = reviewBankGuaranteeByFinancialManager,
                            Port = "Done"
                        },
                        Target = new Endpoint
                        {
                            Activity = fiancialManagerDecision,
                            Port = "In"
                        }
                    },
                    new Connection
                    {
                        Source = new Endpoint
                        {
                            Activity = fiancialManagerDecision,
                            Port = "ReviewByFianncialExpert"
                        },
                        Target = new Endpoint
                        {
                            Activity = reviewBankGuaranteeByFinancialExpert,
                            Port = "In"
                        }
                    },
                    new Connection
                    {
                        Source = new Endpoint
                        {
                            Activity = fiancialManagerDecision,
                            Port = "RejectToLegalManager"
                        },
                        Target = new Endpoint
                        {
                            Activity = approveBankGuaranteeByLegalAndContractAffairsManager,
                            Port = "In"
                        }
                    },
                    new Connection
                    {
                        Source = new Endpoint
                        {
                            Activity = fiancialManagerDecision,
                            Port = "RejectToPM"
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
                            Activity = reviewBankGuaranteeByFinancialExpert,
                            Port = "Done"
                        },
                        Target = new Endpoint
                        {
                            Activity = financialExpertDecision,
                            Port = "In"
                        }
                    },
                    new Connection
                    {
                        Source = new Endpoint
                        {
                            Activity = financialExpertDecision,
                            Port = "ProceededToDelivery"
                        },
                        Target = new Endpoint
                        {
                            Activity = deliverBankGuarantee,
                            Port = "In"
                        }
                    },
                    new Connection
                    {
                        Source = new Endpoint
                        {
                            Activity = financialExpertDecision,
                            Port = "ReviewByFianncialManager"
                        },
                        Target = new Endpoint
                        {
                            Activity = reviewBankGuaranteeByFinancialManager,
                            Port = "In"
                        }
                    },
                    new Connection
                    {
                        Source = new Endpoint
                        {
                            Activity = deliverBankGuarantee,
                            Port = "Done"
                        },
                        Target = new Endpoint
                        {
                            Activity = lastDecision,
                            Port = "In"
                        }
                    },
                    new Connection
                    {
                        Source = new Endpoint
                        {
                            Activity = lastDecision,
                            Port = "RejectToFinancialExpert"
                        },
                        Target = new Endpoint
                        {
                            Activity = reviewBankGuaranteeByFinancialExpert,
                            Port = "In"
                        }
                    },
                    new Connection
                    {
                        Source = new Endpoint
                        {
                            Activity = lastDecision,
                            Port = "ProceededToEnd"
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
