using Curriculum.Core.Entities;
using GraphQL.Types;

namespace Curriculum.Api.GraphQL.ObjectTypes;

public sealed class CompanyType : ObjectGraphType<Company>
{
    public CompanyType()
    {
        Field(x => x.Id, type: typeof(NonNullGraphType<IdGraphType>));
        Field(x => x.Name, type: typeof(NonNullGraphType<StringGraphType>));
    }
}