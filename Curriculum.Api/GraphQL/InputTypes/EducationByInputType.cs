using GraphQL.Types;

namespace Curriculum.Api.GraphQL.InputTypes;

public sealed record EducationBy(Guid? Id, string? Institution);


public sealed class EducationByInputType : InputObjectGraphType<EducationBy>
{
    public EducationByInputType()
    {
        Name = "EducationBy";
        Description = "Identifies a single education entry using exactly one of the available fields.";
        IsOneOf = true;
        Field(x => x.Id, type: typeof(IdGraphType))
            .Description("The id of the education entry.");
        Field(x => x.Institution, type: typeof(StringGraphType))
            .Description("The institution of the education entry.");
    }
}