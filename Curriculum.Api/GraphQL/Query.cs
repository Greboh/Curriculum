using Curriculum.Api.Extensions;
using Curriculum.Api.GraphQL.InputTypes;
using Curriculum.Api.GraphQL.ObjectTypes;
using Curriculum.Services;
using GraphQL;
using GraphQL.Types;

namespace Curriculum.Api.GraphQL;

public sealed class Query : ObjectGraphType
{
    public Query()
    {
        ResolveCompany();
        ResolveEducation();
        ResolveProject();
        ResolveSkill();
    }

    private void ResolveCompany()
    {
        Field<ListGraphType<NonNullGraphType<CompanyType>>>("companies")
            .ResolveAsync(async ctx => await ctx.RequestServices!
                .GetRequiredService<ICompanyService>()
                .GetAll(ctx.CancellationToken)
            );

        Field<CompanyType>("company")
            .Argument<NonNullGraphType<ByIdOrNameInputType>>("by")
            .ResolveAsync(async ctx =>
            {
                var by = ctx.GetArgument<ByIdOrName>("by");

                var result = await ctx.RequestServices!
                    .GetRequiredService<ICompanyService>()
                    .Get(by.Id, by.Name, ctx.CancellationToken);
                    
                return result.GetValueOrAddError(ctx);
            });
    }

    private void ResolveEducation()
    {
        Field<NonNullGraphType<ListGraphType<NonNullGraphType<EducationType>>>>("educations")
            .ResolveAsync(async ctx => await ctx.RequestServices!
                .GetRequiredService<IEducationService>()
                .GetAll(ctx.CancellationToken)
            );

        Field<EducationType>("education")
            .Argument<NonNullGraphType<EducationByInputType>>("by")
            .ResolveAsync(async ctx =>
            {
                var by = ctx.GetArgument<EducationBy>("by");

                var result = await ctx.RequestServices!
                    .GetRequiredService<IEducationService>()
                    .Get(by.Id, by.Institution, ctx.CancellationToken);

                return result.GetValueOrAddError(ctx);
            });
    }

    private void ResolveProject()
    {
        Field<NonNullGraphType<ListGraphType<NonNullGraphType<ProjectType>>>>("projects")
            .ResolveAsync(async ctx => await ctx.RequestServices!
                .GetRequiredService<IProjectService>()
                .GetAll(ctx.CancellationToken)
            );

        Field<ProjectType>("project")
            .Argument<NonNullGraphType<ByIdOrNameInputType>>("by")
            .ResolveAsync(async ctx =>
            {
                var by = ctx.GetArgument<ByIdOrName>("by");

                var result = await ctx.RequestServices!
                    .GetRequiredService<IProjectService>()
                    .Get(by.Id, by.Name, ctx.CancellationToken);

                return result.GetValueOrAddError(ctx);
            });
    }

    private void ResolveSkill()
    {
        Field<NonNullGraphType<ListGraphType<NonNullGraphType<SkillType>>>>("skills")
            .ResolveAsync(async ctx => await ctx.RequestServices!
                .GetRequiredService<ISkillService>()
                .GetAll(ctx.CancellationToken)
            );

        Field<SkillType>("skill")
            .Argument<NonNullGraphType<ByIdOrNameInputType>>("by")
            .ResolveAsync(async ctx =>
            {
                var by = ctx.GetArgument<ByIdOrName>("by");

                var result = await ctx.RequestServices!
                    .GetRequiredService<ISkillService>()
                    .Get(by.Id, by.Name, ctx.CancellationToken);

                return result.GetValueOrAddError(ctx);
            });
    }
}