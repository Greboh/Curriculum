using Curriculum.Api.Extensions;
using Curriculum.Api.GraphQL.Types;
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

        Field<BooleanGraphType>("deleteSkill")
            .Argument<NonNullGraphType<StringGraphType>>("name")
            .Resolve(ctx =>
            {
                var name = ctx.GetArgument<string>("name");

                return ctx.RequestServices!
                    .GetRequiredService<ISkillService>()
                    .Delete(name)
                    .GetValueOrAddError(ctx);
            });
    }
}