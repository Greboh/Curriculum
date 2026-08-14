using Curriculum.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Curriculum.Infrastructure.Persistence.EntityTypeConfigurations;

public class CompanyEntityConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Name)
            .IsUnique();
    }
}