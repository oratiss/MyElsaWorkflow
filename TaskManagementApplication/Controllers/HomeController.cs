using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TaskManagementApplication.Data;
using TaskManagementApplication.Models;
using TaskManagementApplication.Services;
using TaskManagementApplication.Views.Home;

namespace TaskManagementApplication.Controllers;

public class HomeController(TaskManagementDbContext dbContext, IElsaClient elsaClient, ILogger<HomeController> logger) : Controller
{

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var tasks = await dbContext.OnBoardingTasks.Where(x => !x.IsCompleted).ToListAsync(cancellationToken: cancellationToken);
        var model = new IndexViewModel(tasks);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CompleteTask(int taskId, string nextActivityId, CancellationToken cancellationToken)
    {
        var task = dbContext.OnBoardingTasks.FirstOrDefault(x => x.Id == taskId);

        if (task is null) return NotFound();

        await elsaClient.ReportTaskCompletedAsync(task.ExternalId, nextActivityId, cancellationToken: cancellationToken);

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


}
