using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace Curriculum.Infrastructure.Middlewares;

public sealed class GraphQlOperationNameMiddleware(RequestDelegate next)
{
    public static string GraphQlOperationName => "GraphQLOperationName";
    
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/graphql")
            && HttpMethods.IsPost(context.Request.Method))
        {
            context.Request.EnableBuffering();

            using var reader = new StreamReader(
                context.Request.Body,
                Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false,
                bufferSize: 1024,
                leaveOpen: true
            );
            
            var body = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;

            if (!string.IsNullOrWhiteSpace(body))
            {
                using var doc = JsonDocument.Parse(body);
                if (doc.RootElement.TryGetProperty("operationName", out var operationName)
                    && operationName.ValueKind is JsonValueKind.String)
                {
                    context.Items[GraphQlOperationName] = operationName.GetString();
                }
            }
        }
        
        await next(context);
    }
}