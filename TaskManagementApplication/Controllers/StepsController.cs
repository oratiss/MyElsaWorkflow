using Elsa.Expressions.Helpers;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rts.Common;
using System.Diagnostics;
using System.Text.Json;
using TaskManagementApplication.ApiControllers.ApiModels;
using TaskManagementApplication.CommonModelsForSerilaizarioan;
using TaskManagementApplication.Data;
using TaskManagementApplication.Models;
using TaskManagementApplication.Services;
using TaskManagementApplication.Views.Steps;

namespace TaskManagementApplication.Controllers;

public class StepsController(TaskManagementDbContext dbContext, IElsaClient elsaClient, ILogger<StepsController> logger) : Controller
{

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var steps = await dbContext.Steps.Where(x => !x.IsCompleted).ToListAsync(cancellationToken: cancellationToken);
        var model = new IndexViewModel(steps);
        return View(model);
    }

    [HttpGet]
    public IActionResult FillForm(long stepId, CancellationToken cancellationToken)
    {
        var step = dbContext.Steps.Find(stepId);

        var userWorkflowConfig = JsonSerializer.Deserialize<UserWorkflowConfig>(step!.UserWorkflowConfigSerialized);


        var model = new DynamicFormViewModel
        {
            StepId = stepId,
            Fields = new List<DynamicField>()

        };


        foreach (var requiredField in userWorkflowConfig!.FirstActivityConfig.RequiredFieldValues!)
        {
            var (type, value) = FetchRequiredFieldData(requiredField.Value);
            DynamicField dynamicField = new()
            {
                Name = requiredField.Key,
                Label = requiredField.Key,
                Type = type,
                IsDiasabledOnView = true,
            };
            if ((int)dynamicField.Type > 8)
            {
                dynamicField.Options = new List<string>();
                if (value is string[] stringValues)
                {
                    dynamicField.Options.AddRange(stringValues.ToList());
                }
            }
            else
            {
                dynamicField.Value = value;
            }
            model.Fields.Add(dynamicField);
        }

        return PartialView("_FillFormModal", model);
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

    private (FieldType, object) FetchRequiredFieldData(object requiredField)
    {
        if (requiredField is not JsonElement jsonElement) throw new Exception("RequiredField Is not parsable to JsonElement.");
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        var requiredFieldValue = jsonElement.Deserialize<RequiredFieldValueType>(options);

        FieldType type = FieldType.String;
        object value = requiredFieldValue!.Value!;
        switch (requiredFieldValue.Type)
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
                    value = ((JsonElement)requiredFieldValue!.Value!).Deserialize<string>();
                    break;
                }

            //todo: tobe removed
            case "object":
                {
                    type = FieldType.Object;
                    value = ((JsonElement)requiredFieldValue!.Value!).Deserialize<object>();
                    break;
                }

            case "decimalarray":
            case "longarray":
            case "intarray":
            case "shortarray":
            case "boolarray":
            case "guidarray":
            case "dateTimearray":
            case "stringarray":
            //todo: tobe removed
            case "objectarray":
                {
                    type = FieldType.Dropdown;
                    value = ((JsonElement)requiredFieldValue!.Value!).Deserialize<string[]>();
                    break;
                }
        }

        return (type, value);
    }


}

