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
            .ResolveAsync(async ctx =>
            {
                var name = ctx.GetArgument<string>("name");

                var result = await ctx.RequestServices!
                    .GetRequiredService<ISkillService>()
                    .Create(name, ctx.CancellationToken);

                return result.GetValueOrAddError(ctx);
            });

        Field<bool>("deleteSkill")
            .Argument<NonNullGraphType<ByIdOrNameInputType>>("by")
            .ResolveAsync(async ctx =>
            {
                var by = ctx.GetArgument<ByIdOrName>("by");

                return await ctx.RequestServices!
                    .GetRequiredService<ISkillService>()
                    .Delete(by.Id, by.Name, ctx.CancellationToken);
            });
    }
}