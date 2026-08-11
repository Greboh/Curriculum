using Curriculum.Core.Entities;
using GraphQL.Types;

namespace Curriculum.Api.GraphQL.ObjectTypes;

public sealed class SkillType : ObjectGraphType<Skill>
{
    public SkillType()
    {
        Field(x => x.Id, type: typeof(NonNullGraphType<IdGraphType>));
        Field(x => x.Name, type: typeof(NonNullGraphType<StringGraphType>));
    }
}