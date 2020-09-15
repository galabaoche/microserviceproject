using System;
using MediatR;
using ProjectEntity = Project.Domain.AggregatesModel.Project;

namespace Project.API.Application.Commands
{
    public class CreateProjectCommand : IRequest<ProjectEntity>
    {
        public ProjectEntity Project { get; set; }
    }
}
