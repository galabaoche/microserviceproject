using System;
using System.Threading.Tasks;
using Project.Domain.AggregatesModel;
using Project.Domain.SeedWork;
using ProjectEntity = Project.Domain.AggregatesModel.Project;
using Microsoft.EntityFrameworkCore;

namespace Project.Infrastructure.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ProjectContext _projectContext;

        public ProjectRepository(ProjectContext projectContext)
        {
            _projectContext = projectContext;
        }

        public IUnitOfWork UnitOfWork => _projectContext;

        public async Task<ProjectEntity> AddAsync(ProjectEntity project)
        {
            if (project.IsTransient())
            {
                return (await _projectContext.AddAsync(project)).Entity;
            }
            return project;
        }

        public async Task<ProjectEntity> GetAsync(int id)
        {
            return await _projectContext.Projects
                .Include(p => p.Contributors)
                .Include(p => p.Viewers)
                .Include(p => p.VisibleRule)
                .Include(p => p.Properties)
                .SingleOrDefaultAsync(p => p.Id == id);
        }

        public async Task UpdateAsync(ProjectEntity project)
        {
            _projectContext.Update(project);
            await Task.CompletedTask;
        }
    }
}
