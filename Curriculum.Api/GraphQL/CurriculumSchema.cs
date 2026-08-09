using GraphQL.Types;

namespace Curriculum.Api.GraphQL;

public class CurriculumSchema : Schema
{
    public CurriculumSchema(IServiceProvider provider) : base(provider)
    {
        Query = provider.GetRequiredService<Query>();
    }
}