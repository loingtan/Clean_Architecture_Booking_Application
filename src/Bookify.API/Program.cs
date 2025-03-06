using Bookify.API.Extensions;
using Bookify.API.Hypermedia;
using Bookify.API.Middleware;
using Bookify.API.OpenApi;
using Bookify.Application;
using Bookify.Infrastructure;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandling>();
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddControllers(cfg =>
{
    cfg.RespectBrowserAcceptHeader = true;
    cfg.ReturnHttpNotAcceptable = true;
}).AddXmlDataContractSerializerFormatters();

builder.Services.AddEndpointsApiExplorer();
// Add after existing Swagger configuration

// Configure Swagger for HATEOAS
builder.Services.AddSwaggerGen(options =>
{
    // Existing SwaggerGen configuration...

    options.SchemaFilter<HateoasSchemaFilter>();

    // Add HATEOAS media type support
    options.MapType<EntityResponseWrapper<object>>(() => new OpenApiSchema
    {
        Type = "object",
        Properties = new Dictionary<string, OpenApiSchema>
        {
            ["value"] = new OpenApiSchema { Type = "object" },
            ["_links"] = new OpenApiSchema
            {
                Type = "array",
                Items = new OpenApiSchema
                {
                    Type = "object",
                    Properties = new Dictionary<string, OpenApiSchema>
                    {
                        ["href"] = new OpenApiSchema { Type = "string" },
                        ["rel"] = new OpenApiSchema { Type = "string" },
                        ["method"] = new OpenApiSchema { Type = "string" }
                    }
                }
            }
        }
    });
});

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();
// Add after other service registrations
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
builder.Services.AddScoped<ILinkGenerator, Bookify.API.Hypermedia.LinkGenerator>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var descriptions = app.DescribeApiVersions();

        foreach (var groupName in descriptions.Select(description => description.GroupName))
        {
            var url = $"/swagger/{groupName}/swagger.json";
            var name = groupName.ToUpperInvariant();
            options.SwaggerEndpoint(url, name);
        }
    });
    app.ApplyMigrations();
    await app.SeedDataAsync();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});
app.UseRequestContextLogging();

app.UseSerilogRequestLogging();

// app.UseCustomExceptionHandler();
app.UseExceptionHandler();
app.UseStatusCodePages();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

//app.MapBookingEndpoints();

app.MapHealthChecks("health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();

//In order to reference in the IntegrationTest project
public abstract partial class Program;