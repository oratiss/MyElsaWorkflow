using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rts.Common;
using System.Text.Json;
using TaskManagementApplication.ApiControllers.ApiModels;
using TaskManagementApplication.CommonModelsForSerilaizarioan;
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
        [HttpGet]
        public List<WorkflowEntities> Index()
        {
            return new();
        }

        [HttpPost("{id:long}")]
        [AllowAnonymous]
        public async Task<ActionResult> RunWorkflow([FromRoute] long id, [FromBody] RunWorkflowRequest request)
        {
            //Todo: validate request

            //var workFlow = dbContext.Workflows.Find(id);
            //if (workFlow is null)
            //{
            //    return NotFound();
            //}

            //if (string.IsNullOrWhiteSpace(workFlow.ElsaWorkflowDefinitionId))
            //{
            //    return NotFound("One required data is missing in system. Contact the system admisitrator.");
            //}

            //Todo: tobe removed:
            Workflow workflow = new();
            workflow.ElsaWorkflowDefinitionId = "BankGuarantee";

            Dictionary<string, object> processedData;
            (bool flowControl, ActionResult? value) = AdaptRequestFields(request!, out processedData);
            if (!flowControl)
            {
                return value!;
            }

            var assignableUserGroups = request!.AssignableUserGroups.Adapt<UserGroup[]>();
            var currentPerformerGroup = request.FirstActivityConfig.CurrentPerformerGroup.Adapt<UserGroup>();
            User currentPerfromerUser = AdaptUser(request);

            UserWorkflowConfig workflowConfig = new()
            {
                AssignableUserGroups = assignableUserGroups,
                FirstActivityConfig = new
                (
                    currentPerformerGroup, currentPerfromerUser, processedData, request.FirstActivityConfig.PossibleRequiredData
                )
            };

            await elsaClient.RunWorkflowAsync(workflow.ElsaWorkflowDefinitionId, workflowConfig);

            return Ok();
        }

        private User AdaptUser(RunWorkflowRequest request)
        {
            var userId = request.FirstActivityConfig.CurrentPerformerUser.Id;
            var firstName = request.FirstActivityConfig.CurrentPerformerUser.FirstName;
            var lastName = request.FirstActivityConfig.CurrentPerformerUser.LastName;
            var currentPerfromerUser = new User(userId, firstName, lastName);
            return currentPerfromerUser;
        }

        private (bool flowControl, ActionResult? value) AdaptRequestFields(RunWorkflowRequest runWorkflowRequest, out Dictionary<string, object> processedData)
        {
            try
            {
                processedData = new Dictionary<string, object>();
                foreach (var field in runWorkflowRequest!.FirstActivityConfig.RequiredFieldValues!)
                {
                    object parsedValue;
                    var type = field.Type.ToLower();
                    switch (type)
                    {
                        case "decimal":
                            parsedValue = new RequiredFieldValueType()
                            {
                                Type = type,
                                Value = decimal.Parse(field.Value) 
                            };
                            break;

                        case "long":
                            parsedValue = new RequiredFieldValueType()
                            {
                                Type = type,
                                Value = long.Parse(field.Value)
                            };
                            break;

                        case "int":
                            parsedValue = new RequiredFieldValueType()
                            {
                                Type = type,
                                Value = int.Parse(field.Value)
                            };
                            break;

                        case "short":
                            parsedValue = new RequiredFieldValueType()
                            {
                                Type = type,
                                Value = short.Parse(field.Value)
                            };
                            break;

                        case "bool":
                            parsedValue = new RequiredFieldValueType()
                            {
                                Type = type,
                                Value = bool.Parse(field.Value)
                            };
                            break;

                        case "guid":
                            parsedValue = new RequiredFieldValueType()
                            {
                                Type = type,
                                Value = new Guid(field.Value.ToString())
                            };
                            break;

                        case "datetime":
                            parsedValue = new RequiredFieldValueType()
                            {
                                Type = type,
                                Value = DateTime.Parse(field.Value)
                            };
                            break;

                        case "string":
                            parsedValue = new RequiredFieldValueType()
                            {
                                Type = type,
                                Value = field.Value
                            };
                            break;

                        case "object":
                            parsedValue = new RequiredFieldValueType()
                            {
                                Type = type,
                                Value = field.Value
                            };
                            break;

                        case "decimalarray":
                            parsedValue = new RequiredFieldValueType()
                            {
                                Type = type,
                                Value = field.Value.Adapt<decimal[]>()
                            };
                            break;

                        case "longarray":
                            parsedValue = new RequiredFieldValueType()
                            {
                                Type = type,
                                Value = field.Value.Adapt<long[]>()
                            };
                            break;

                        case "intarray":
                            parsedValue = new RequiredFieldValueType()
                            {
                                Type = type,
                                Value = field.Value.Adapt<int[]>()
                            };
                            break;

                        case "shortarray":
                            parsedValue = new RequiredFieldValueType()
                            {
                                Type = type,
                                Value = field.Value.Adapt<short[]>()
                            };
                            break;

                        case "boolarray":
                            parsedValue = new RequiredFieldValueType()
                            {
                                Type = type,
                                Value = field.Value.Adapt<bool[]>()
                            };
                            break;

                        case "guidarray":
                            parsedValue = new RequiredFieldValueType()
                            {
                                Type = type,
                                Value = JsonSerializer.Deserialize<string[]>(field!.Value!)
                                   !.Select(x => new Guid(x))
                                   .ToArray()
                            };
                            break;

                        case "dateTimearray":
                            parsedValue = new RequiredFieldValueType()
                            {
                                Type = type,
                                Value = field.Value.Adapt<DateTime[]>()
                            };
                            break;

                        case "stringarray":
                            parsedValue = new RequiredFieldValueType()
                            {
                                Type = type,
                                Value = field.Value.Adapt<string[]>()
                            };
                            break;

                        case "objectarray":
                            parsedValue = new RequiredFieldValueType()
                            {
                                Type = type,
                                Value = field.Value.Adapt<object[]>()
                            };
                            break;

                        default:
                            // Handle unknown or unsupported types
                            return (flowControl: false, value: BadRequest($"Unsupported type: {field.Type}"));
                    }
                    processedData.Add(field.Name, parsedValue);
                }

                return (flowControl: true, value: null);
            }
            catch (Exception e)
            {

                throw;
            }
        }
    }


   
}
