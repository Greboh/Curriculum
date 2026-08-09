using Curriculum.Infrastructure.Configurations;

namespace Curriculum.Api.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder ConfigureApi(this WebApplicationBuilder builder)
    {
        if (builder.Environment.IsDevelopment())
        {
            builder.AddAspireDefaults();
        }

        return builder;
    }
} 