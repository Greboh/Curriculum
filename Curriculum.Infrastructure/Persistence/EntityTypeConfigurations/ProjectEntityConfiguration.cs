using Curriculum.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Curriculum.Infrastructure.Persistence.EntityTypeConfigurations;

public class ProjectEntityConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Name)
            .IsUnique();
    }
}