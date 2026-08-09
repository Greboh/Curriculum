using Curriculum.Core.Entities;
using GraphQL.Types;

namespace Curriculum.Api.GraphQL.Types;

public sealed class ProjectType : ObjectGraphType<Project>
{
    public ProjectType()
    {
        Field(x => x.Id, type: typeof(NonNullGraphType<IdGraphType>));
        Field(x => x.Name, type: typeof(NonNullGraphType<StringGraphType>));
    }
}