using Curriculum.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Curriculum.Infrastructure.Persistence.EntityTypeConfigurations;

public class EducationEntityConfiguration : IEntityTypeConfiguration<Education>
{
    public void Configure(EntityTypeBuilder<Education> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Institution);

        builder.Property(x => x.Degree)
            .IsRequired();

        builder.Property(x => x.StartDate)
            .IsRequired();
    }
}