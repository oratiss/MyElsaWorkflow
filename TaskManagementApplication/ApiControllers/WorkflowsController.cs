using Microsoft.AspNetCore.Mvc;
using Rts.Common;
using Rts.Common.BankGuaranteeModels;
using TaskManagementApplication.ApiControllers.ApiModels;
using TaskManagementApplication.Data;
using TaskManagementApplication.Entities;
using TaskManagementApplication.Services;

namespace TaskManagementApplication.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkflowsController(IElsaClient elsaClient, TaskManagementDbContext dbContext) : ControllerBase
    {
        //todo: to be completed later
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public List<WorkflowEntities> Index()
        {
            return new();
        }

        [Microsoft.AspNetCore.Mvc.HttpPost("{id:long}")]
        public async Task<ActionResult> RunWorkflow([FromRoute] long id, [FromBody] RunWorkflowRequest? runWorkflowRequest)
        {
            var workFlow = dbContext.Workflows.Find(id);
            if (workFlow is null)
            {
                return NotFound();
            }

            if (string.IsNullOrWhiteSpace(workFlow.ElsaWorkflowDefinitionId))
            {
                return NotFound("One required data is missing in system. Contact the system admisitrator.");
            }

            //Todo: fill it from database and request
            UserWorkflowConfig workflowConfig = new()
            {
                AssignableUserGroups = new List<UserGroup>
                {
                    new UserGroup(new Guid("4ab19335-1f18-4330-aa14-12e5828a234d"), "EveryOne"),
                    new UserGroup(new Guid("7574f7cc-3670-4889-af76-de6d3a96ec7a"), "Project Managers"),
                    new UserGroup(new Guid("4e342586-0659-4e55-a10b-3ac4b3b7268c"), "Contratc Managers"),
                    new UserGroup(new Guid("3a9ffc60-3246-4068-a10e-1bcfcacaf051"), "Contract Team"),
                    new UserGroup(new Guid("27ded3ec-511b-42b1-8028-3cdaf2cab6ef"), "Financial Managers"),
                    new UserGroup(new Guid("dbad127d-e10a-4235-ab0d-263933bf598b"), "Financial Experts"),
                    new UserGroup(new Guid("6574369f-9f04-4218-865d-838eda3be52b"), "Guarantee Users")
                },

                FirstActivityConfig = new UserActivityConfig(
                    new UserGroup(new Guid("158bb7d3-0be1-4151-9791-274a55a57df8"), "EveryOne"),
                    new User(new Guid("822ac725-75b0-4492-b811-1e673ed4ce65"), "Masoud", "Asgarian"),
                    new List<RequiredFieldValue>
                    {
                        new ("HoldingId", typeof(Guid), new Guid("d1841c3c-db8d-4711-b61b-f8a1f798696b")),
                        new ("CompanyId", typeof(Guid), new Guid("a0385dfa-d071-483b-b0cf-a249cd777e67")),
                        new ("PrjectId", typeof(Guid), new Guid("44c29db5-9b46-4769-9ad5-2f653b1e701f")),
                        new ("ReceivingDate", typeof(DateTime), DateTime.UtcNow.AddDays(50)),
                        new ("Title", typeof(string), "Bank Guarantee For Maroon NGL EPC Project"),
                        new ("Type", typeof(BankGuaranteeType), BankGuaranteeType.Other),
                        new ("BankGuaranteeIssuanceDate", typeof(DateTime), DateTime.UtcNow.AddDays(10)),
                        new ("Amount", typeof(decimal), 100_000_000_000m),
                        new ("Currency", typeof(Currency), Currency.IRR),
                        new ("ProviderCompanyId",  typeof(Guid), new Guid("a814ab28-b86e-4beb-b9b0-1f7c06a5a8cf")),
                        new ("PurchaseRequests", typeof(List<Guid>), new List<Guid>
                                                                              {
                                                                                new ("c705203f-9f33-4602-aa2d-8f70d0a3b238"),
                                                                                new ("c64c8167-70aa-4206-aecf-3a06691cea52"),
                                                                                new ("6f2beeec-5acb-474c-8249-0323250ef76b"),
                                                                              }),
                        new ("PurchaseOrders", typeof(List<Guid>), new List<Guid>
                                                                            {
                                                                                new ("efd0dd21-0755-4d34-b4bc-163729e9e14e"),
                                                                                new ("80e60aa2-fe52-4646-90ec-20f55b3f1407"),
                                                                                new ("542cbe38-79c2-42f6-b073-56cc34a83a65")
                                                                            }),
                        new ("CustomerPurchaseOrders", typeof(List<Guid>), new List<Guid>
                                                                                    {
                                                                                         new ("46eb8f3f-f4cd-4ba9-a6e7-bf079c8b5315"),
                                                                                         new ("a0339bcb-c782-497a-a0b1-fee9d54cabf5"),
                                                                                         new ("3d2f59cf-43bd-43ca-8931-9d2cdf9039d2")
                                                                                    }),
                        new ("ProviderUsers", typeof(List<Guid>), new List<Guid>
                                                                           {
                                                                                new ("25c65ae4-3828-4c11-b87a-03e71b662811"),
                                                                                new ("64372256-e847-42b6-b110-463a99b9b801"),
                                                                                new ("1ef78072-020a-46a6-9b4e-15faadbbfef7")
                                                                           }),
                        new ("PettyCashCustodianIds", typeof(List<Guid>), new List<Guid>
                                                                                   {
                                                                                        new ("e0a2a063-194d-4d5d-9a65-5218e3c29c9f"),
                                                                                        new ("fa04554d-454f-4fdf-a7ef-f8d2fc79f33c"),
                                                                                        new ("a4ae52a0-5138-4b5b-a1d9-efc39738cd36")
                                                                                   }),
                        new ("Description", typeof(string), "This is a 2000 char length for the BankGuarantee Description")
                    },
                    new PossibleRequiredData()
                    {
                        ExpectedState = BankGuaranteeStandardState.Created
                    })

            };

            await elsaClient.RunWorkflowAsync(workFlow.ElsaWorkflowDefinitionId, workflowConfig);

            return Ok();
        }
    }
}
