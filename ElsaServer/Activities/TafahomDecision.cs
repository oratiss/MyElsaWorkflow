using Elsa.Extensions;
using Elsa.Workflows;
using Elsa.Workflows.Activities.Flowchart.Activities;
using Elsa.Workflows.Activities.Flowchart.Attributes;
using Microsoft.Net.Http.Headers;
using Rts.Common;
using System.Text.Json;

namespace ElsaServer.Activities
{
    [FlowNode("Expert", "PM", "AnotherExpert", "ProjectManager", "LegalAndContractManager",
        "FinancialManager", "BackToExpert", "LegalAndContractAffairsExpert", "RejectToPM", "ReviewByFianncialExpert",
        "RejectToLegalManager", "ReviewByFianncialManager", "ProceededToDelivery", "RejectToFinancialExpert", "ProceededToEnd")]
    public class TafahomDecision : Activity
    {
        private JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

        protected override async ValueTask ExecuteAsync(ActivityExecutionContext context)
        {
            var a = context.WorkflowInput["RunTaskInput"];
            var wfConfig = JsonSerializer.Deserialize<UserWorkflowConfig>(JsonSerializer.Serialize(a, JsonSerializerOptions), JsonSerializerOptions);
            var wfConfigVariable = context.SetVariable("userWorkflowConfig", wfConfig);
            var outcome = Convert.ToString(wfConfig!.ActivityConfig.PossibleRequiredData!)!;

            try
            {
                switch (this.Id)
                {
                    case ("TafahomDecision-BankGuarantee-01-Creation-ExpertsOrPM"):
                        {
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
                            ;
                            break;
                        }
                    case ("TafahomDecision-BankGuarantee-02-AnotherExpertsOrPM"):
                        {
                            switch (outcome)
                            {
                                case "ApproveBankGuaranteeByAnotherExpert":
                                    await context.CompleteActivityWithOutcomesAsync("AnotherExpert");
                                    break;

                                case "ApproveBankGuaranteeByPM":
                                    await context.CompleteActivityWithOutcomesAsync("ProjectManager");
                                    break;
                            }
                            break;
                        }
                    case ("TafahomDecision-BankGuarantee-03-projectManagerDecision"):
                        {
                            switch (outcome)
                            {
                                case "ApproveBankGuaranteeByLegalAndContractAffairsManager":
                                    await context.CompleteActivityWithOutcomesAsync("LegalAndContractManager");
                                    break;

                                case "ReviewBankGuaranteeByFinancialManager":
                                    await context.CompleteActivityWithOutcomesAsync("FinancialManager");
                                    break;

                                case "ApproveBankGuaranteeByExpert":
                                    await context.CompleteActivityWithOutcomesAsync("BackToExpert");
                                    break;

                            }
                            break;
                        }
                    case ("TafahomDecision-BankGuarantee-04-LegalAndContractAffairsManagerDecision"):
                        {
                            switch (outcome)
                            {
                                case "ApproveBankGuaranteeByLegalAndContractAffairsExpert":
                                    await context.CompleteActivityWithOutcomesAsync("LegalAndContractAffairsExpert");
                                    break;

                                case "ReviewBankGuaranteeByFinancialManager":
                                    await context.CompleteActivityWithOutcomesAsync("FinancialManager");
                                    break;

                                case "ApproveBankGuaranteeByPM":
                                    await context.CompleteActivityWithOutcomesAsync("RejectToPM");
                                    break;
                            }
                            break;
                        }
                    case ("TafahomDecision-BankGuarantee-05-FinancialManagerDecision"):
                        {
                            switch (outcome)
                            {
                                case "ReviewBankGuaranteeByFinancialExpert":
                                    await context.CompleteActivityWithOutcomesAsync("ReviewByFianncialExpert");
                                    break;
                                case "ApproveBankGuaranteeByLegalAndContractAffairsManager":
                                    await context.CompleteActivityWithOutcomesAsync("RejectToLegalManager");
                                    break;
                                case "ApproveBankGuaranteeByPM":
                                    await context.CompleteActivityWithOutcomesAsync("RejectToPM");
                                    break;
                            }
                            break;
                        }
                    case ("TafahomDecision-BankGuarantee-06-FinancialExpertDecision"):
                        {
                            switch (outcome)
                            {
                                case "ReviewBankGuaranteeByFinancialManager":
                                    await context.CompleteActivityWithOutcomesAsync("ReviewByFianncialManager");
                                    break;

                                case "DeliverBankGuarantee":
                                    await context.CompleteActivityWithOutcomesAsync("ProceededToDelivery");
                                    break;
                            }
                            break;
                        }
                    case ("TafahomDecision-BankGuarantee-07-LastDecision"):
                        {
                            switch (outcome)
                            {
                                case "ReviewBankGuaranteeByFinancialExpert":
                                    await context.CompleteActivityWithOutcomesAsync("RejectToFinancialExpert");
                                    break;

                                case "End":
                                    await context.CompleteActivityWithOutcomesAsync("ProceededToEnd");
                                    break;
                            }
                            break;
                        }
                    default:
                        {
                            throw new Exception("Activity Name is out of range for decisions.");
                        }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }


        }
    }
}
