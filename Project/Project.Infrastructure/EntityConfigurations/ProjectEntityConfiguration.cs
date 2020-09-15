using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectEntity = Project.Domain.AggregatesModel.Project;

namespace Project.Infrastructure.EntityConfigurations
{
    public class ProjectEntityConfiguration : IEntityTypeConfiguration<ProjectEntity>
    {
        public void Configure(EntityTypeBuilder<ProjectEntity> builder)
        {           
            builder.ToTable("Projects").HasKey(p => p.Id);
        }
    }
}
