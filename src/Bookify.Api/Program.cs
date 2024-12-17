//using Asp.Versioning;
//using Asp.Versioning.ApiExplorer;
//using Asp.Versioning.Builder;
//using Bookify.Api.Controllers.Apartments;
//using Bookify.Api.Controllers.Bookings;
//using Bookify.Api.Controllers.Users;
//using Bookify.Api.Extensions;
//using Bookify.Api.OpenApi;
//using Bookify.Application;
//using Bookify.Application.Abstractions.Data;
//using Bookify.Infrastructure;
//using Dapper;
//using HealthChecks.UI.Client;
//using Microsoft.AspNetCore.Diagnostics.HealthChecks;
//using Microsoft.Extensions.Diagnostics.HealthChecks;
//using Serilog;


//var builder = WebApplication.CreateBuilder(args);

//builder.Host.UseSerilog((context, configuration) =>
//{
//    configuration.ReadFrom.Configuration(context.Configuration);
//});

//builder.Services.AddControllers();

//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//builder.Services.AddApplication();
//builder.Services.AddInfrastructure(builder.Configuration);

//builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();


//var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI(options =>
//    {
//        foreach (ApiVersionDescription description in app.DescribeApiVersions())
//        {
//            string url = $"/swagger/{description.GroupName}/swagger.json";
//            string name = description.GroupName.ToUpperInvariant();
//            options.SwaggerEndpoint(url, name);
//        }
//    });

//    app.ApplyMigrations();

//    //app.SeedData();
//}

//app.UseHttpsRedirection();

//app.UseRequestContextLogging();

//app.UseSerilogRequestLogging();

//app.UseCustomExceptionHandler();

//app.UseAuthentication();

//app.UseAuthorization();

//app.MapControllers();

//ApiVersionSet apiVersionSet = app.NewApiVersionSet()
//    .HasApiVersion(new ApiVersion(1))
//    .ReportApiVersions()
//    .Build();

//var routeGroupBuilder = app.MapGroup("api/v{version:apiVersion}").WithApiVersionSet(apiVersionSet);

//routeGroupBuilder.MapBookingEndpoints();
//routeGroupBuilder.MapApartmentEndpoints();
//routeGroupBuilder.MapUserEndpoints();

//app.MapHealthChecks("health", new HealthCheckOptions
//{
//    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
//});

//app.Run();

//public partial class Program;


using System.Reflection;
using Asp.Versioning;
using Asp.Versioning.Builder;
using Bookify.Api.Extensions;
using Bookify.Application;
using Bookify.Infrastructure;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(context.Configuration));

builder.Services
    .AddApplication()
    .AddPresentation()
    .AddInfrastructure(builder.Configuration);

builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());

WebApplication app = builder.Build();

ApiVersionSet apiVersionSet = app.NewApiVersionSet()
    .HasApiVersion(new ApiVersion(1))
    .ReportApiVersions()
    .Build();

RouteGroupBuilder versionedGroup = app
    .MapGroup("api/v{version:apiVersion}")
    .WithApiVersionSet(apiVersionSet);

app.MapEndpoints(versionedGroup);

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerWithUi();

    app.ApplyMigrations();
}

app.UseHttpsRedirection();

app.MapHealthChecks("health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseRequestContextLogging();

app.UseSerilogRequestLogging();

app.UseExceptionHandler();

app.UseAuthentication();

app.UseAuthorization();

// REMARK: If you want to use Controllers, you'll need this.
//app.MapControllers();

await app.RunAsync();

// REMARK: Required for functional and integration tests to work.
public partial class Program;

