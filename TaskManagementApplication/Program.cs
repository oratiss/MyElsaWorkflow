using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using TaskManagementApplication.Data;
using TaskManagementApplication.Services;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

//builder.Services.AddDbContext<TaskManagementDbContext>(options =>
//    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<TaskManagementDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ElsaSQlServer"), 
    sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()));

builder.Services.AddHttpClient<ElsaClient>("elsaHttpClient", httpClient =>
{
    var url = configuration["Elsa:ServerUrl"]!.TrimEnd('/') + '/';
    var apiKey = configuration["Elsa:ApiKey"]!;
    httpClient.BaseAddress = new Uri(url);
    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("ApiKey", apiKey);
});

builder.Services.AddScoped<IElsaClient, ElsaClient>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
