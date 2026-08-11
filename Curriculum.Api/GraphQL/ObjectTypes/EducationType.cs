using Curriculum.Core.Entities;
using GraphQL.Types;

namespace Curriculum.Api.GraphQL.ObjectTypes;

public sealed class EducationType : ObjectGraphType<Education>
{
    public EducationType()
    {
        Field(x => x.Id, type: typeof(NonNullGraphType<IdGraphType>));
        Field(x => x.Institution, type: typeof(NonNullGraphType<StringGraphType>));
        Field(x => x.Degree, type: typeof(NonNullGraphType<StringGraphType>));
        Field(x => x.StartDate, type: typeof(NonNullGraphType<DateOnlyGraphType>));
        Field(x => x.EndDate, type: typeof(DateOnlyGraphType));
    }
}