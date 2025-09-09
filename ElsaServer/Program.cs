using Elsa.EntityFrameworkCore.Extensions;
using Elsa.EntityFrameworkCore.Modules.Management;
using Elsa.EntityFrameworkCore.Modules.Runtime;
using Elsa.Extensions;
using Elsa.Workflows.Runtime;
using ElsaServer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseStaticWebAssets();

builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);

var services = builder.Services;
var configuration = builder.Configuration;

services.AddEndpointsApiExplorer();
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Elsa Workflows API",
        Version = "v1",
        Description = "API for Elsa Workflow Management"
    });

    // Critical: Custom schema ID selector to handle problematic types
    c.CustomSchemaIds(type =>
    {
        if (type.FullName == null)
            return type.Name;

        // Handle generic types better
        if (type.IsGenericType)
        {
            var name = type.Name;
            var backtickIndex = name.IndexOf('`');
            if (backtickIndex > 0)
                name = name.Substring(0, backtickIndex);

            var genericArgs = string.Join("", type.GetGenericArguments().Select(t => t.Name));
            return $"{name}Of{genericArgs}";
        }

        return type.FullName.Replace("+", ".").Replace("[]", "Array");
    });

    // Ignore all operations that cause problems
    c.DocInclusionPredicate((name, api) =>
    {
        // Only include specific safe endpoints
        var safePaths = new[]
        {
            "/elsa/api/workflow-definitions",
            "/elsa/api/activity-descriptors",
            "/elsa/api/storage-drivers"
        };

        return safePaths.Any(path => api.RelativePath?.StartsWith(path.TrimStart('/')) == true);
    });

    // Handle authentication
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Enter JWT Bearer token"
    });

    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Enter API Key in format: ApiKey {your-key}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        },
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "ApiKey" }
            },
            Array.Empty<string>()
        }
    });
});

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
    .UseWorkflowRuntime(runtime =>
    {
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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Elsa Workflows API V1");
        c.RoutePrefix = "swagger";
        c.DisplayRequestDuration();
        c.EnableTryItOutByDefault();
    });
}


//added by asgarian
app.UseCustomExceptionHandler();

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
