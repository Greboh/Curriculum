using Curriculum.Api.Extensions;
using Curriculum.Api.GraphQL.Types;
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
        Field<NonNullGraphType<ListGraphType<NonNullGraphType<CompanyType>>>>("companies")
            .Resolve(ctx => ctx.RequestServices!
                .GetRequiredService<ICompanyService>()
                .GetAll()
            );

        Field<CompanyType>("company")
            .Argument<NonNullGraphType<IdGraphType>>("id")
            .Resolve(ctx =>
            {
                var id = ctx.GetArgument<Guid>("id");
                
                return ctx.RequestServices!
                    .GetRequiredService<ICompanyService>()
                    .GetById(id)
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
            .Argument<NonNullGraphType<IdGraphType>>("id")
            .Resolve(ctx =>
            {
                var id = ctx.GetArgument<Guid>("id");
                
                return ctx.RequestServices!
                    .GetRequiredService<IEducationService>()
                    .GetById(id)
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
            .Argument<NonNullGraphType<IdGraphType>>("id")
            .Resolve(ctx =>
            {
                var id = ctx.GetArgument<Guid>("id");
                
                return ctx.RequestServices!
                    .GetRequiredService<IProjectService>()
                    .GetById(id)
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
            .Argument<NonNullGraphType<IdGraphType>>("id")
            .Resolve(ctx =>
            {
                var id = ctx.GetArgument<Guid>("id");
                
                return ctx.RequestServices!
                    .GetRequiredService<ISkillService>()
                    .GetById(id)
                    .GetValueOrAddError(ctx);
            });
    }
}