using System;
using Microsoft.EntityFrameworkCore;
using Recommend.API.Models;

namespace Recommend.API.Data
{
    public class RecommendDbContext : DbContext
    {
        public RecommendDbContext(DbContextOptions<RecommendDbContext> options) : base(options)
        {
        }
        public DbSet<ProjectRecommend> ProjectRecommends { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProjectRecommend>().ToTable("ProjectRecommends")
                .HasKey(p => new { p.Id });
            modelBuilder.Entity<ProjectReferenceUser>().ToTable("ProjectReferenceUsers")
                .HasKey(p => new { p.Id, p.UserId });
            base.OnModelCreating(modelBuilder);
        }
    }
}
