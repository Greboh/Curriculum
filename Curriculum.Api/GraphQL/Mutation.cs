using Curriculum.Api.Extensions;
using Curriculum.Api.GraphQL.InputTypes;
using Curriculum.Api.GraphQL.ObjectTypes;
using Curriculum.Services;
using GraphQL;
using GraphQL.Types;

namespace Curriculum.Api.GraphQL;

public sealed class Mutation : ObjectGraphType 
{
    public Mutation()
    {
        ResolveSkill();
    }

    private void ResolveSkill()
    {
        Field<SkillType>("createSkill")
            .Argument<NonNullGraphType<StringGraphType>>("name")
            .Resolve(ctx =>
            {
                var name = ctx.GetArgument<string>("name");

                return ctx.RequestServices!
                    .GetRequiredService<ISkillService>()
                    .Create(name)
                    .GetValueOrAddError(ctx);
            });

        Field<SkillType>("deleteSkill")
            .Argument<NonNullGraphType<ByIdOrNameInputType>>("by")
            .Resolve(ctx =>
            {
                var by = ctx.GetArgument<ByIdOrName>("by");

                return ctx.RequestServices!
                    .GetRequiredService<ISkillService>()
                    .Delete(by.Id, by.Name)
                    .GetValueOrAddError(ctx);
            });
    }
}