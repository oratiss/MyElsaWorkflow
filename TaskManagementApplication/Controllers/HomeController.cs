using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TaskManagementApplication.ApiModels;
using TaskManagementApplication.Data;
using TaskManagementApplication.Models;
using TaskManagementApplication.Services;
using TaskManagementApplication.Views.Home;

namespace TaskManagementApplication.Controllers;

public class HomeController(TaskManagementDbContext dbContext, IElsaClient elsaClient, ILogger<HomeController> logger) : Controller
{
    #region Commented
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var tasks = await dbContext.OnBoardingTasks.Where(x => !x.IsCompleted).ToListAsync(cancellationToken: cancellationToken);
        var model = new IndexViewModel(tasks);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CompleteTask(CompleteTaskRequest request, CancellationToken cancellationToken)
    {
        var task = dbContext.OnBoardingTasks.FirstOrDefault(x => x.Id == request.TaskId);

        if (task is null) return NotFound();

        //var result = request.Result ?? task.Result; // 
        var result = new
        {
            Name = "Masoud Asgarian"
        }; 

        //await elsaClient.ReportTaskCompletedAsync(task.ExternalId, result, request.NextActivityId, cancellationToken);
        await elsaClient.ReportTaskCompletedAsync(task.ExternalId, result, cancellationToken);

        task.IsCompleted = true;
        task.CompletedAt = DateTimeOffset.Now;

        dbContext.OnBoardingTasks.Update(task);
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
    #endregion

    //[HttpGet]
    //public async Task<IActionResult> Index(CancellationToken cancellationToken)
    //{
    //    var steps = await dbContext.Steps.Where(x => !x.IsCompleted).ToListAsync(cancellationToken: cancellationToken);
    //    var model = new IndexViewModel(steps);
    //    return View(model);
    //}

    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> CompleteStep(CompleteStepRequest request, CancellationToken cancellationToken)
    //{
    //    var step = dbContext.Steps.FirstOrDefault(x => x.Id == request.StepId);

    //    if (step is null) return NotFound();

    //    var result = request.Result ?? step.Result; // 

    //    await elsaClient.ReportStepCompletedAsync(step.ExternalId, result, request.NextActivityId, cancellationToken);

    //    step.IsCompleted = true;
    //    step.CompletedAt = DateTimeOffset.Now;

    //    dbContext.Steps.Update(step);
    //    await dbContext.SaveChangesAsync(cancellationToken);

    //    return RedirectToAction("Index");
    //}

    //[HttpGet]
    //public IActionResult Privacy()
    //{
    //    return View();
    //}

    //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    //public IActionResult Error()
    //{
    //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    //}


}

