using Curriculum.Infrastructure.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.AspNetCore;
using Serilog.Events;

namespace Curriculum.Infrastructure.Configurations;

public static class SerilogConfiguration
{
    public static WebApplicationBuilder ConfigureSerilogLogging(this WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .SetMinimumLevels()
            .CreateLogger();

        builder.Host.UseSerilog();

        return builder;
    }

    public static WebApplication ConfigureSerilogRequestLogging(this WebApplication app)
    {
        app.UseMiddleware<GraphQlOperationNameMiddleware>();

        app.UseSerilogRequestLogging(opt =>
        {
            opt.ConfigureGraphQLRequestLogging();
        });

        return app;
    }

    private static LoggerConfiguration SetMinimumLevels(this LoggerConfiguration configuration)
    {
        return configuration
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning);
    }

    private static RequestLoggingOptions ConfigureGraphQLRequestLogging(this RequestLoggingOptions opt)
    {
        opt.GetLevel = (httpContext, _, _) =>
        {
            if (HttpMethods.IsGet(httpContext.Request.Method)
                && httpContext.Request.Path.StartsWithSegments("/graphql"))
            {
                return LogEventLevel.Debug;
            }
                
            return LogEventLevel.Information;
        };
            
        opt.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
        {
            if (httpContext.Items.TryGetValue(GraphQlOperationNameMiddleware.GraphQlOperationName, out var operation)
                && operation is string operationName)
            {
                diagnosticContext.Set("OperationName", operationName);
            }
            else
            {
                diagnosticContext.Set("OperationName", "Anonymous operation");
            }
        };
        
        opt.MessageTemplate =
            "HTTP {RequestMethod} {RequestPath} {OperationName} responded {StatusCode} in {Elapsed:0.0000} ms";

        return opt;
    }
}