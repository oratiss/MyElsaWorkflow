using Antlr4.Runtime.Misc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Rts.Common;
using System.Diagnostics;
using System.Text.Json;
using TaskManagementApplication.ApiControllers.ApiModels;
using TaskManagementApplication.CommonModelsForSerilaizarioan;
using TaskManagementApplication.Data;
using TaskManagementApplication.Entities;
using TaskManagementApplication.Models;
using TaskManagementApplication.Services;
using TaskManagementApplication.Views.Steps;
using YamlDotNet.Core.Tokens;

namespace TaskManagementApplication.Controllers;

public class StepsController(TaskManagementDbContext dbContext, IElsaClient elsaClient, ILogger<StepsController> logger) : Controller
{
    private JsonSerializerOptions Options => new()
    {
        PropertyNameCaseInsensitive = true
    };

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var steps = await dbContext.Steps.Where(x => !x.IsCompleted).ToListAsync(cancellationToken: cancellationToken);
        var model = new IndexViewModel(steps);
        return View(model);
    }

    [HttpGet]
    public IActionResult Review(long stepId, CancellationToken cancellationToken)
    {
        var step = dbContext.Steps.Find(stepId)!;

        DynamicFormViewModel model = PrepareReviewViewModel(step);

        return PartialView("_ReviewModal", model);
    }

    [HttpPost]
    public IActionResult Review(DynamicFormViewModel reviewModel, CancellationToken cancellationToken)
    {
        var step = dbContext.Steps.Find(reviewModel.StepId)!;

        //todo add a code to fetch wf activities from TMA databse and check none-frozen fields to change in code below (converting from dynamicFormViewModel to steps's wfConfiguration field).

        Dictionary<string, object>? requiredFields = new();
        //foreach (DynamicField dynamicfield in reviewModel.Fields)
        //{
        //    if (!dynamicfield.IsDiasabledOnView)
        //    {
        //        var typeName = nameof(dynamicfield.Type).Split(".").Last().ToLower();
        //        switch (dynamicfield.Type)
        //        {
        //            case FieldType.Int:
        //                {
        //                    requiredFields.Add(typeName, (int)dynamicfield.Value!);
        //                    break;
        //                }
        //            case FieldType.Long:
        //                {
        //                    requiredFields.Add(typeName, (long)dynamicfield.Value!);
        //                    break;
        //                }
        //            case FieldType.Short:
        //                {
        //                    requiredFields.Add(typeName, (short)dynamicfield.Value!);
        //                    break;
        //                }
        //            case FieldType.Decimal:
        //                {
        //                    requiredFields.Add(typeName, (decimal)dynamicfield.Value!);
        //                    break;
        //                }
        //            case FieldType.Guid:
        //                {
        //                    requiredFields.Add(typeName, new Guid(dynamicfield.Value!.ToString()!));
        //                    break;
        //                }
        //            case FieldType.Boolean:
        //                {
        //                    requiredFields.Add(typeName, (bool)dynamicfield.Value!);
        //                    break;
        //                }
        //            case FieldType.DateTime:
        //                {
        //                    requiredFields.Add(typeName, (DateTime)dynamicfield.Value!);
        //                    break;
        //                }
        //            case FieldType.String:
        //                {
        //                    requiredFields.Add(typeName, dynamicfield.Value!.ToString()!);
        //                    break;
        //                }
        //            case FieldType.Object:
        //                requiredFields.Add(typeName, dynamicfield.Value!);
        //                break;
        //            case FieldType.Dropdown:
        //                {
        //                    List<TypeCheckPair<object>> pairs = new List<TypeCheckPair<object>>();
        //                    foreach (var item in dynamicfield.Options!)
        //                    {

        //                        var exisitingWfConfig = JsonSerializer.Deserialize<UserWorkflowConfig>(step.UserWorkflowConfigSerialized, Options);
        //                        var reqField = exisitingWfConfig!.ActivityConfig.RequiredFieldValues!.FirstOrDefault(x => x.Key.ToLower() == dynamicfield.Name.ToLower());
        //                        var reqFieldType = (reqField.Value as RequiredFieldValueType)!.Type;

        //                        TypeCheckPair<object> pair = new();
        //                        switch (reqFieldType.ToLower())
        //                        {

        //                            case "decimalarray":
        //                                {
        //                                    pair.Value = Convert.ToDecimal(item);
        //                                    break;
        //                                }
        //                            case "longarray":
        //                                {
        //                                    pair.Value = Convert.ToInt64(item);
        //                                    break;
        //                                }
        //                            case "intarray":
        //                                {
        //                                    pair.Value = Convert.ToInt32(item);
        //                                    break;
        //                                }
        //                            case "shortarray":
        //                                {
        //                                    pair.Value = Convert.ToInt16(item);
        //                                    break;
        //                                }
        //                            case "boolarray":
        //                                {
        //                                    pair.Value = Convert.ToBoolean(item);
        //                                    break;
        //                                }
        //                            case "guidarray":
        //                                {
        //                                    pair.Value = new Guid(item);
        //                                    break;
        //                                }
        //                            case "datetimearray":
        //                                {
        //                                    pair.Value = Convert.ToDateTime(item);
        //                                    break;
        //                                }
        //                            case "stringarray":
        //                                {
        //                                    pair.Value = item;
        //                                    break;
        //                                }
        //                            case "objectarray":
        //                                {
        //                                    pair.Value = item;
        //                                    break;
        //                                }
        //                        }
        //                        if (item == dynamicfield.Value!.ToString()) pair.IsChecked = true;
        //                        pairs.Add(pair);
        //                    }

        //                    requiredFields.Add(dynamicfield.Name, pairs.ToArray());
        //                    break;
        //                }
        //            case FieldType.CheckBoxListAll:
        //                {
        //                    List<TypeCheckPair<object>> pairs = new List<TypeCheckPair<object>>();
        //                    foreach (var item in (string[])dynamicfield.Value!)
        //                    {

        //                        var exisitingWfConfig = JsonSerializer.Deserialize<UserWorkflowConfig>(step.UserWorkflowConfigSerialized, Options);
        //                        var reqField = exisitingWfConfig!.ActivityConfig.RequiredFieldValues!.FirstOrDefault(x => x.Key.ToLower() == dynamicfield.Name.ToLower());
        //                        var reqFieldType = (reqField.Value as RequiredFieldValueType)!.Type;

        //                        TypeCheckPair<object> pair = new();
        //                        switch (reqFieldType.ToLower())
        //                        {

        //                            case "decimalarray":
        //                                {
        //                                    pair.Value = Convert.ToDecimal(item);
        //                                    break;
        //                                }
        //                            case "longarray":
        //                                {
        //                                    pair.Value = Convert.ToInt64(item);
        //                                    break;
        //                                }
        //                            case "intarray":
        //                                {
        //                                    pair.Value = Convert.ToInt32(item);
        //                                    break;
        //                                }
        //                            case "shortarray":
        //                                {
        //                                    pair.Value = Convert.ToInt16(item);
        //                                    break;
        //                                }
        //                            case "boolarray":
        //                                {
        //                                    pair.Value = Convert.ToBoolean(item);
        //                                    break;
        //                                }
        //                            case "guidarray":
        //                                {
        //                                    pair.Value = new Guid(item);
        //                                    break;
        //                                }
        //                            case "datetimearray":
        //                                {
        //                                    pair.Value = Convert.ToDateTime(item);
        //                                    break;
        //                                }
        //                            case "stringarray":
        //                                {
        //                                    pair.Value = item;
        //                                    break;
        //                                }
        //                            case "objectarray":
        //                                {
        //                                    pair.Value = item;
        //                                    break;
        //                                }
        //                        }
        //                        pair.Value = true;
        //                        pairs.Add(pair);
        //                    }

        //                    requiredFields.Add(dynamicfield.Name, pairs.ToArray());
        //                    break;


        //                }
        //            case FieldType.CheckBoxListMany:
        //                {
        //                    var exisitingWfConfig = JsonSerializer.Deserialize<UserWorkflowConfig>(step.UserWorkflowConfigSerialized, Options);
        //                    var reqField = exisitingWfConfig!.ActivityConfig.RequiredFieldValues!.FirstOrDefault(x => x.Key.ToLower() == dynamicfield.Name.ToLower());
        //                    var reqFieldType = (reqField.Value as RequiredFieldValueType)!.Type;

        //                    switch (reqFieldType.ToLower())
        //                    {
        //                        case "decimalarray-checkboxlistselectmany":
        //                            {
        //                                var decimalPairs = (List<TypeCheckPair<decimal>>)dynamicfield.Value!;
        //                                requiredFields.Add(dynamicfield.Name, decimalPairs.ToArray());
        //                                break;
        //                            }
        //                        case "longarray-checkboxlistselectmany":
        //                            {
        //                                var longPairs = (List<TypeCheckPair<long>>)dynamicfield.Value!;
        //                                requiredFields.Add(dynamicfield.Name, longPairs.ToArray());
        //                                break;
        //                            }
        //                        case "intarray-checkboxlistselectmany":
        //                            {
        //                                var intPairs = (List<TypeCheckPair<int>>)dynamicfield.Value!;
        //                                requiredFields.Add(dynamicfield.Name, intPairs.ToArray());
        //                                break;
        //                            }
        //                        case "shortarray-checkboxlistselectmany":
        //                            {
        //                                var shortPairs = (List<TypeCheckPair<short>>)dynamicfield.Value!;
        //                                requiredFields.Add(dynamicfield.Name, shortPairs.ToArray());
        //                                break;
        //                            }
        //                        case "boolarray-checkboxlistselectmany":
        //                            {
        //                                var boolPairs = (List<TypeCheckPair<bool>>)dynamicfield.Value!;
        //                                requiredFields.Add(dynamicfield.Name, boolPairs.ToArray());
        //                                break;
        //                            }
        //                        case "guidarray-checkboxlistselectmany":
        //                            {
        //                                var guidPairs = (List<TypeCheckPair<Guid>>)dynamicfield.Value!;
        //                                requiredFields.Add(dynamicfield.Name, guidPairs.ToArray());
        //                                break;
        //                            }
        //                        case "datetimearray-checkboxlistselectmany":
        //                            {
        //                                var dateTimePairs = (List<TypeCheckPair<DateTime>>)dynamicfield.Value!;
        //                                requiredFields.Add(dynamicfield.Name, dateTimePairs.ToArray());
        //                                break;
        //                            }
        //                        case "stringarray-checkboxlistselectmany":
        //                            {
        //                                var stringPairs = (List<TypeCheckPair<string>>)dynamicfield.Value!;
        //                                requiredFields.Add(dynamicfield.Name, stringPairs.ToArray());
        //                                break;
        //                            }
        //                        case "objectarray-checkboxlistselectmany":
        //                            {
        //                                var objectPairs = (List<TypeCheckPair<object>>)dynamicfield.Value!;
        //                                requiredFields.Add(dynamicfield.Name, objectPairs.ToArray());
        //                                break;
        //                            }
        //                    }

        //                    break;
        //                }
        //            default:
        //                throw new NotSupportedException();
        //        }
        //    }
        //}


        DynamicFormViewModel model = PrepareReviewViewModel(step);


        return PartialView("_ReviewModal", model);
    }



    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Complete(CompleteStepRequest request, CancellationToken cancellationToken)
    {
        var step = dbContext.Steps.FirstOrDefault(x => x.Id == request.StepId);

        if (step is null) return NotFound();


        //todo: fetch result
        //var result = request.Result ?? task.Result; // 

        await elsaClient.ReportTaskCompletedAsync(step.ExternalId, new(), cancellationToken);

        step.IsCompleted = true;
        step.CompletedAt = DateTimeOffset.Now;

        dbContext.Steps.Update(step);
        await dbContext.SaveChangesAsync(cancellationToken);

        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }


    private DynamicFormViewModel PrepareReviewViewModel(Step step)
    {
        var userWorkflowConfig = JsonSerializer.Deserialize<UserWorkflowConfig>(step!.UserWorkflowConfigSerialized)!;
        var model = new DynamicFormViewModel
        {
            StepId = step.Id,
            Fields = new List<DynamicField>(),

        };

        //below is informative
        PrepareAssignableGroups(userWorkflowConfig, model);

        PrepareCurrentPerfomerGroup(userWorkflowConfig, model);

        //PreparePerformerUser(userWorkflowConfig, model);

        foreach (var requiredField in userWorkflowConfig!.ActivityConfig.RequiredFieldValues!)
        {
            var (type, value) = FetchRequiredFieldData(requiredField.Value);
            DynamicField dynamicField = new()
            {
                Name = requiredField.Key,
                Label = requiredField.Key,
                Type = type,
                //todo: to be discussed with team mates
                IsDiasabledOnView = false,
            };
            if ((int)dynamicField.Type == 11) //selectMany
            {
                if (value is TypeCheckPair<object>[] typeCheckPairs)
                {
                    dynamicField.Value = JsonSerializer.Serialize(typeCheckPairs, Options);
                }
            }
            else if ((int)dynamicField.Type == 10) //SelectAll
            {
                if (value is string[] stringValues)
                {
                    dynamicField.Value = string.Join(",", stringValues);
                }
            }
            else if ((int)dynamicField.Type == 9)
            {
                dynamicField.Options = new List<string>();
                if (value is TypeCheckPair<object>[] pairs)
                {
                    dynamicField.Options.AddRange(pairs.Select(x => x.Value.ToString()).ToList()!);
                    dynamicField.Value = pairs.SingleOrDefault(x => x.IsChecked)?.Value.ToString();
                }
            }
            else
            {
                dynamicField.Value = value.ToString();
            }


            model.Fields.Add(dynamicField);

        }

        return model;
    }

    private (FieldType, object) FetchRequiredFieldData(object requiredField)
    {
        if (requiredField is not JsonElement jsonElement) throw new Exception("RequiredField Is not parsable to JsonElement.");

        var requiredFieldValue = jsonElement.Deserialize<RequiredFieldValueType>(Options);

        FieldType type;
        object value;
        switch (requiredFieldValue!.Type)
        {
            case "decimal":
                {
                    type = FieldType.Decimal;
                    value = ((JsonElement)requiredFieldValue!.Value!).Deserialize<decimal>();
                    break;
                }
            case "long":
                {
                    type = FieldType.Long;
                    value = ((JsonElement)requiredFieldValue!.Value!).Deserialize<long>();
                    break;
                }
            case "int":
                {
                    type = FieldType.Int;
                    value = ((JsonElement)requiredFieldValue!.Value!).Deserialize<int>();
                    break;
                }
            case "short":
                {
                    type = FieldType.Short;
                    value = ((JsonElement)requiredFieldValue!.Value!).Deserialize<short>();
                    break;
                }
            case "bool":
                {
                    type = FieldType.Boolean;
                    value = ((JsonElement)requiredFieldValue!.Value!).Deserialize<bool>();
                    break;
                }
            case "guid":
                {
                    type = FieldType.Guid;
                    value = ((JsonElement)requiredFieldValue!.Value!).Deserialize<Guid>();
                    break;
                }
            case "datetime":
                {
                    type = FieldType.DateTime;
                    value = ((JsonElement)requiredFieldValue!.Value!).Deserialize<DateTime>();
                    break;
                }
            case "string":
                {
                    type = FieldType.String;
                    value = ((JsonElement)requiredFieldValue!.Value!).Deserialize<string>()!;
                    break;
                }
            case "object":
                {
                    type = FieldType.Object;
                    value = ((JsonElement)requiredFieldValue!.Value!).Deserialize<object>()!;
                    break;
                }

            case "decimalarray":
            case "longarray":
            case "intarray":
            case "shortarray":
            case "boolarray":
            case "guidarray":
            case "datetimearray":
            case "stringarray":
            case "objectarray":
                {
                    type = FieldType.Dropdown;
                    value = ((JsonElement)requiredFieldValue!.Value!).Deserialize<TypeCheckPair<object>[]>(Options)!;
                    break;
                }

            case "decimalarray-checkboxlistselectall":
            case "longarray-checkboxlistselectall":
            case "intarray-checkboxlistselectall":
            case "shortarray-checkboxlistselectall":
            case "boolarray-checkboxlistselectall":
            case "guidarray-checkboxlistselectall":
            case "datetimearray-checkboxlistselectall":
            case "stringarray-checkboxlistselectall":
            case "objectarray-checkboxlistselectall":
                {
                    type = FieldType.CheckBoxListAll;
                    value = ((JsonElement)requiredFieldValue!.Value!).Deserialize<string[]>()!;
                    break;
                }

            case "decimalarray-checkboxlistselectmany":
            case "longarray-checkboxlistselectmany":
            case "intarray-checkboxlistselectmany":
            case "shortarray-checkboxlistselectmany":
            case "boolarray-checkboxlistselectmany":
            case "guidarray-checkboxlistselectmany":
            case "datetimearray-checkboxlistselectmany":
            case "stringarray-checkboxlistselectmany":
            case "objectarray-checkboxlistselectmany":
                {
                    type = FieldType.CheckBoxListMany;
                    value = ((JsonElement)requiredFieldValue!.Value!).Deserialize<TypeCheckPair<object>[]>(Options)!;
                    break;
                }
            default:
                throw new ArgumentOutOfRangeException("Type is out of range!.");
        }

        return (type, value);
    }


    private static void PrepareAssignableGroups(UserWorkflowConfig userWorkflowConfig, DynamicFormViewModel model)
    {
        DynamicField assignableUserGroup = new()
        {
            Name = "AssignableUserGroups",
            Label = "AssignableUserGroups",
            Type = FieldType.CheckBoxListAll,
            IsDiasabledOnView = true,
            Value = ""
        };
        var values = new List<string>();
        foreach (var userGroup in userWorkflowConfig!.AssignableUserGroups)
        {
            values.Add(userGroup.Name);
        }
        assignableUserGroup.Value = string.Join(",", values.ToArray());
        model.Fields.Add(assignableUserGroup);
    }

    private void PrepareCurrentPerfomerGroup(UserWorkflowConfig userWorkflowConfig, DynamicFormViewModel model)
    {
        DynamicField performerGroup = new()
        {
            Name = "CurrentPerformerUserGroup",
            Label = "CurrentPerformerUserGroup",
            Type = FieldType.String,
            IsDiasabledOnView = true,
            Value = ""
        };
        if (userWorkflowConfig.ActivityConfig.CurrentPerformerGroup is not null)
        {
            performerGroup.Value = userWorkflowConfig.ActivityConfig.CurrentPerformerGroup!.Name;
        }
        else
        {
            performerGroup = new()
            {
                Name = "CurrentPerformerUserGroup",
                Label = "CurrentPerformerUserGroup",
                Type = FieldType.Dropdown,
                IsDiasabledOnView = false,
                Value = ""
            };
            var values = new List<string>();
            foreach (var userGroup in userWorkflowConfig!.AssignableUserGroups)
            {
                values.Add(userGroup.Name);
            }
            performerGroup.Value = values.ToArray().ToString();
        }

        model.Fields.Add(performerGroup);
    }

}

