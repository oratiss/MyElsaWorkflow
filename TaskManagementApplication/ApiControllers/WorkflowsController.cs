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
using YamlDotNet.Core.Tokens;

namespace TaskManagementApplication.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkflowsController(IElsaClient elsaClient, TaskManagementDbContext dbContext) : ControllerBase
    {
        private JsonSerializerOptions JsonSerializerOptions => new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };


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
            var currentPerformerGroup = request.ActivityConfig.CurrentPerformerGroup.Adapt<UserGroup?>();
            User? currentPerfromerUser = AdaptUser(request);

            UserWorkflowConfig workflowConfig = new()
            {
                AssignableUserGroups = assignableUserGroups,
                ActivityConfig = new
                (
                    currentPerformerGroup, currentPerfromerUser, processedData, request.ActivityConfig.PossibleRequiredData
                )
            };

            await elsaClient.RunWorkflowAsync(workflow.ElsaWorkflowDefinitionId, workflowConfig);

            return Ok();
        }

        private User? AdaptUser(RunWorkflowRequest request)
        {
            if (request.ActivityConfig.CurrentPerformerUser == null)
            {
                return null;
            }
            var userId = request.ActivityConfig.CurrentPerformerUser.Id!;
            var firstName = request.ActivityConfig.CurrentPerformerUser.FirstName;
            var lastName = request.ActivityConfig.CurrentPerformerUser.LastName;
            var currentPerfromerUser = new User(userId, firstName, lastName);
            return currentPerfromerUser;
        }

        private (bool flowControl, ActionResult? value) AdaptRequestFields(RunWorkflowRequest runWorkflowRequest, out Dictionary<string, object> processedData)
        {
            try
            {
                processedData = new Dictionary<string, object>();
                foreach (var field in runWorkflowRequest!.ActivityConfig.RequiredFieldValues!)
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
                        case "decimalarray-checkboxlistselectall":
                            parsedValue = new RequiredFieldValueType()
                            {
                                Type = type,
                                Value = field.Value.Adapt<decimal[]>()
                            };
                            break;

                        case "longarray":
                        case "longarray-checkboxlistselectall":
                            parsedValue = new RequiredFieldValueType()
                            {
                                Type = type,
                                Value = field.Value.Adapt<long[]>()
                            };
                            break;

                        case "intarray":
                        case "intarray-checkboxlistselectall":
                            parsedValue = new RequiredFieldValueType()
                            {
                                Type = type,
                                Value = field.Value.Adapt<int[]>()
                            };
                            break;

                        case "shortarray":
                        case "shortarray-checkboxlistselectall":
                            parsedValue = new RequiredFieldValueType()
                            {
                                Type = type,
                                Value = field.Value.Adapt<short[]>()
                            };
                            break;

                        case "boolarray":
                        case "boolarray-checkboxlistselectall":
                            parsedValue = new RequiredFieldValueType()
                            {
                                Type = type,
                                Value = field.Value.Adapt<bool[]>()
                            };
                            break;

                        case "guidarray":
                        case "guidarray-checkboxlistselectall":
                            parsedValue = new RequiredFieldValueType()
                            {
                                Type = type,
                                Value = JsonSerializer.Deserialize<string[]>(field!.Value!)
                                   !.Select(x => new Guid(x))
                                   .ToArray()
                            };
                            break;

                        case "dateTimearray":
                        case "dateTimearray-checkboxlistselectall":
                            parsedValue = new RequiredFieldValueType()
                            {
                                Type = type,
                                Value = field.Value.Adapt<DateTime[]>()
                            };
                            break;

                        case "stringarray":
                        case "stringarray-checkboxlistselectall":
                            parsedValue = new RequiredFieldValueType()
                            {
                                Type = type,
                                Value = field.Value.Adapt<string[]>()
                            };
                            break;

                        case "objectarray":
                        case "objectarray-checkboxlistselectall":
                            {

                                parsedValue = new RequiredFieldValueType()
                                {
                                    Type = type,
                                    Value = field.Value.Adapt<object[]>()
                                };
                                break;
                            }
                        case "decimalarray-checkboxlistselectmany":
                            {
                                var pairItems = JsonSerializer.Deserialize<TypeCheckPair<decimal>[]>(field.Value, JsonSerializerOptions);
                                parsedValue = new RequiredFieldValueType()
                                {
                                    Type = type,
                                    Value = pairItems
                                };
                                break;
                            }
                        case "longarray-checkboxlistselectmany":
                            {
                                var pairItems = JsonSerializer.Deserialize<TypeCheckPair<long>[]>(field.Value, JsonSerializerOptions);
                                parsedValue = new RequiredFieldValueType()
                                {
                                    Type = type,
                                    Value = pairItems
                                };
                                break;
                            }
                        case "intarray-checkboxlistselectmany":
                            {
                                var pairItems = JsonSerializer.Deserialize<TypeCheckPair<int>[]>(field.Value, JsonSerializerOptions);
                                parsedValue = new RequiredFieldValueType()
                                {
                                    Type = type,
                                    Value = pairItems
                                };
                                break;
                            }
                        case "shortarray-checkboxlistselectmany":
                            {
                                var pairItems = JsonSerializer.Deserialize<TypeCheckPair<short>[]>(field.Value, JsonSerializerOptions);
                                parsedValue = new RequiredFieldValueType()
                                {
                                    Type = type,
                                    Value = pairItems
                                };
                                break;
                            }
                        case "boolarray-checkboxlistselectmany":
                            {
                                var pairItems = JsonSerializer.Deserialize<TypeCheckPair<bool>[]>(field.Value, JsonSerializerOptions);
                                parsedValue = new RequiredFieldValueType()
                                {
                                    Type = type,
                                    Value = pairItems
                                };
                                break;
                            }
                        case "guidarray-checkboxlistselectmany":
                            {
                                var pairItems = JsonSerializer.Deserialize<TypeCheckPair<Guid>[]>(field.Value, JsonSerializerOptions);
                                parsedValue = new RequiredFieldValueType()
                                {
                                    Type = type,
                                    Value = pairItems
                                };
                                break;
                            }
                        case "dateTimearray-checkboxlistselectmany":
                            {
                                var pairItems = JsonSerializer.Deserialize<TypeCheckPair<DateTime>[]>(field.Value, JsonSerializerOptions);
                                parsedValue = new RequiredFieldValueType()
                                {
                                    Type = type,
                                    Value = pairItems
                                };
                                break;
                            }
                        case "stringarray-checkboxlistselectmany":
                            {
                                var pairItems = JsonSerializer.Deserialize<TypeCheckPair<string>[]>(field.Value, JsonSerializerOptions);
                                parsedValue = new RequiredFieldValueType()
                                {
                                    Type = type,
                                    Value = pairItems
                                };
                                break;
                            }
                        case "objectarray-checkboxlistselectmany":
                            {
                                var pairItems = JsonSerializer.Deserialize<TypeCheckPair<object>[]>(field.Value, JsonSerializerOptions);
                                parsedValue = new RequiredFieldValueType()
                                {
                                    Type = type,
                                    Value = pairItems
                                };
                                break;
                            }
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
