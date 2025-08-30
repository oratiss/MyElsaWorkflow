using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rts.Common;
using System.Diagnostics;
using System.Text.Json;
using TaskManagementApplication.ApiControllers.ApiModels;
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
    public async Task<IActionResult> FillForm(int stepId, CancellationToken cancellationToken)
    {
        var step = dbContext.Steps.Find(stepId);

        var userWorkflowConfig = JsonSerializer.Deserialize<UserWorkflowConfig>(step!.UserWorkflowConfigSerialized);

        List<DynamicField> dynamicFields = new();
        foreach (var requiredField in userWorkflowConfig!.FirstActivityConfig.RequiredFieldValues!)
        {
            DynamicField dynamicField = new()
            {
                Name = requiredField.Key,
                Label = requiredField.Key,
                //Type = requiredField.
            };
        }

        var model = new DynamicFormViewModel
        {
            StepId = stepId,
            Fields = new List<DynamicField>
            {
                new DynamicField { Name = "TextValue", Label = "Text Field", Type = FieldType.String },
                new DynamicField { Name = "IntValue", Label = "Integer", Type = FieldType.Int },
                new DynamicField { Name = "DecimalValue", Label = "Price", Type = FieldType.Decimal },
                new DynamicField { Name = "GuidValue", Label = "Reference ID", Type = FieldType.Guid },
                new DynamicField { Name = "BoolValue", Label = "Is Active?", Type = FieldType.Boolean },
                new DynamicField { Name = "SelectedOption", Label = "Category", Type = FieldType.Dropdown, Options = new List<string>{ "Option1", "Option2", "Option3" } }
            }
        };

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




}

