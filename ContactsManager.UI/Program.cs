using ServiceContracts;
using Services;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Rotativa.AspNetCore;
using Domain.RepositoryContracts;
using Repositories;
using Serilog;
using CRUDExample.StartupExtensions;
using CRUDExample.Middleware;
using Microsoft.Extensions.FileSystemGlobbing.Internal.Patterns;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureServices(builder.Configuration);

if(builder.Environment.IsEnvironment("Test") == false)
    RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");

builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services);
});

var app = builder.Build();

if(app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/error");
    app.UseExceptionHandlingMiddleware();
}

app.UseHsts();
app.UseHttpsRedirection();
app.UseHttpLogging();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "area",
        pattern: "{area:exists}{controller=Home}/{action=Index}");
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

public partial class Program { }