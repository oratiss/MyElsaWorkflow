using Elsa.EntityFrameworkCore.Extensions;
using Elsa.EntityFrameworkCore.Modules.Management;
using Elsa.EntityFrameworkCore.Modules.Runtime;
using Elsa.Extensions;
using Elsa.Workflows.Runtime;
using ElsaServer;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseStaticWebAssets();

builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);

var services = builder.Services;
var configuration = builder.Configuration;

services
.AddElsa(elsa => elsa
    .UseIdentity(identity =>
    {
        identity.TokenOptions = options => options.SigningKey = "large-signing-key-for-signing-JWT-tokens";
        identity.UseAdminUserProvider();
    })
    .UseDefaultAuthentication(a =>
    {
        //added by pejamn:
        a.UseApiKeyAuthorization<TafahomApiKeyProvider>();
    })
    .UseWorkflowManagement(management =>
    {
        //management.UseEntityFrameworkCore(ef => ef.UseSqlite());
        var connectionString = builder.Configuration.GetConnectionString("ElsaSQlServer")!;
        management.UseEntityFrameworkCore(ef => ef.UseSqlServer(connectionString));

        //added by asgarian:
        management.SetDefaultLogPersistenceMode(Elsa.Workflows.LogPersistence.LogPersistenceMode.Include);
    })
    .UseWorkflowRuntime(runtime => {
        //runtime.UseEntityFrameworkCore(ef => ef.UseSqlite());
        var connectionString = builder.Configuration.GetConnectionString("ElsaSQlServer")!;
        runtime.UseEntityFrameworkCore(ef => ef.UseSqlServer(connectionString));
    })
    .UseScheduling()
    .UseJavaScript()
    .UseLiquid()
    .UseCSharp()
    .UseHttp(http => http.ConfigureHttpOptions = options => configuration.GetSection("Http").Bind(options))
    .UseWorkflowsApi()
    .AddActivitiesFrom<Program>()
    .AddWorkflowsFrom<Program>()
    .UseWebhooks(webhooks => webhooks.ConfigureSinks += options => builder.Configuration.GetSection("webhooks").Bind(options))
    );

services.AddCors(cors => cors.AddDefaultPolicy(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().WithExposedHeaders("*")));
services.AddRazorPages(options => options.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute()));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseRouting();
app.UseCors();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseWorkflowsApi();
app.UseWorkflows();
app.MapFallbackToPage("/_Host");

app.MapPost("/api/events/publish", async (HttpContext context, IEventPublisher eventPublisher) =>
{
    try
    {
        // Read the JSON body
        using var reader = new StreamReader(context.Request.Body);
        var body = await reader.ReadToEndAsync();
        var eventRequest = JsonSerializer.Deserialize<JsonElement>(body);

        var eventName = eventRequest.GetProperty("eventName").GetString();
        var correlationId = eventRequest.GetProperty("correlationId").GetString();
        var input = eventRequest.GetProperty("input");

        // Publish the event
        await eventPublisher.PublishAsync(eventName!, correlationId, null, null, JsonSerializer.Serialize(input));

        return Results.Ok(new
        {
            success = true,
            message = "Event published successfully",
            eventName,
            correlationId
        });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new
        {
            success = false,
            error = ex.Message
        });
    }
});

app.Run();