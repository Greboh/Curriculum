using Curriculum.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Curriculum.Infrastructure.Persistence;

public sealed class CurriculumContext(DbContextOptions<CurriculumContext> options)
    : DbContext(options)
{
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Education> Educations => Set<Education>();
    public DbSet<Skill> Skills => Set<Skill>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}