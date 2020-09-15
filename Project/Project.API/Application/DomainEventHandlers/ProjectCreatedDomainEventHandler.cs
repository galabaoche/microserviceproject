using System;
using MediatR;
using Project.Domain.Events;
using System.Threading.Tasks;
using System.Threading;
using DotNetCore.CAP;
using Project.API.Application.IntegrationEvents;

namespace Project.API.Application.DomainEventHandlers
{
    public class ProjectCreatedDomainEventHandler : INotificationHandler<ProjectCreatedEvent>
    {
        private readonly ICapPublisher _capPublisher;

        public ProjectCreatedDomainEventHandler(ICapPublisher capPublisher)
        {
            _capPublisher = capPublisher;
        }

        public async Task Handle(ProjectCreatedEvent notification, CancellationToken cancellationToken)
        {
            var @event = new ProjectCreatedIntegrationEvent
            {
                UserId = notification.Project.UserId,
                Company = notification.Project.Company,
                FinStage = notification.Project.FinStage,
                Introduction = notification.Project.Introduction,
                ProjectAvatar = notification.Project.Avatar,
                Tags = notification.Project.Tags,
                CreatedTime = DateTime.Now,
                ProjectId = notification.Project.Id
            };
            await _capPublisher.PublishAsync("projectapi.project.created", @event);
        }
    }
}
