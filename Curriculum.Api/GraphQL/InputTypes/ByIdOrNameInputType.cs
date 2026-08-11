using GraphQL.Types;

namespace Curriculum.Api.GraphQL.InputTypes;

public sealed record ByIdOrName(Guid? Id, string? Name);


public sealed class ByIdOrNameInputType : InputObjectGraphType<ByIdOrName>
{
    public ByIdOrNameInputType()
    {
        Name = "ByIdOrName";
        Description = "Identifies a single resource using exactly one of the available fields.";
        IsOneOf = true;
        Field(x => x.Id, type: typeof(IdGraphType))
            .Description("The id of the resource.");
        Field(x => x.Name, type: typeof(StringGraphType))
            .Description("The name of the resource.");
    }
}