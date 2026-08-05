namespace Curriculum.Api.Options;

public class ServiceOptions
{
    public const string Section = "Configuration";

    public string Name { get; set; } = string.Empty;
    
    public static ServiceOptions Get(IConfiguration configuration)
    {
        return configuration
            .GetSection(Section)
            .Get<ServiceOptions>() ?? new();
    }
}