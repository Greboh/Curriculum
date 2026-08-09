using Curriculum.Core.Results;
using GraphQL;

namespace Curriculum.Api.Extensions;

public static class ResultExtensions
{
    public static TValue? GetValueOrAddError<TValue>(
        this Result<TValue> result,
        IResolveFieldContext ctx
    )
    {
        if (result.IsSuccess)
        {
            return result.Value;
        }

        var error = new ExecutionError(result.Error!.Message)
        {
            Code = result.Error.HttpStatusCode.ToString(),
        };
        
        error.AddExtension("title", result.Error.HttpTitle);
        
        if (result.Error.HttpExtensions is { Count: > 0 })
        {
            error.AddExtension("details", result.Error.HttpExtensions);
        }
        
        ctx.Errors.Add(error);

        return default;
    }
}