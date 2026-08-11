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
            .Resolve(ctx => ctx.RequestServices!
                .GetRequiredService<ICompanyService>()
                .GetAll()
            );

        Field<CompanyType>("company")
            .Argument<NonNullGraphType<ByIdOrNameInputType>>("by")
            .Resolve(ctx =>
            {
                var by = ctx.GetArgument<ByIdOrName>("by");

                return ctx.RequestServices!
                    .GetRequiredService<ICompanyService>()
                    .Get(by.Id, by.Name)
                    .GetValueOrAddError(ctx);
            });
    }

    private void ResolveEducation()
    {
        Field<NonNullGraphType<ListGraphType<NonNullGraphType<EducationType>>>>("educations")
            .Resolve(ctx => ctx.RequestServices!
                .GetRequiredService<IEducationService>()
                .GetAll()
            );

        Field<EducationType>("education")
            .Argument<NonNullGraphType<EducationByInputType>>("by")
            .Resolve(ctx =>
            {
                var by = ctx.GetArgument<EducationBy>("by");
                
                return ctx.RequestServices!
                    .GetRequiredService<IEducationService>()
                    .Get(by.Id, by.Institution)
                    .GetValueOrAddError(ctx);
            });
    }
    
    private void ResolveProject()
    {
        Field<NonNullGraphType<ListGraphType<NonNullGraphType<ProjectType>>>>("projects")
            .Resolve(ctx => ctx.RequestServices!
                .GetRequiredService<IProjectService>()
                .GetAll()
            );

        Field<ProjectType>("project")
            .Argument<NonNullGraphType<ByIdOrNameInputType>>("by")
            .Resolve(ctx =>
            {
                var by = ctx.GetArgument<ByIdOrName>("by");
                
                return ctx.RequestServices!
                    .GetRequiredService<IProjectService>()
                    .Get(by.Id, by.Name)
                    .GetValueOrAddError(ctx);
            });
    }
    
    private void ResolveSkill()
    {
        Field<NonNullGraphType<ListGraphType<NonNullGraphType<SkillType>>>>("skills")
            .Resolve(ctx => ctx.RequestServices!
                .GetRequiredService<ISkillService>()
                .GetAll()
            );

        Field<SkillType>("skill")
            .Argument<NonNullGraphType<ByIdOrNameInputType>>("by")
            .Resolve(ctx =>
            {
                var by = ctx.GetArgument<ByIdOrName>("by");
                
                return ctx.RequestServices!
                    .GetRequiredService<ISkillService>()
                    .Get(by.Id, by.Name)
                    .GetValueOrAddError(ctx);
            });
    }
}