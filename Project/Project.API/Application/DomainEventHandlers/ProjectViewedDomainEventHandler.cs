using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Project.Domain.AggregatesModel;
using Project.Domain.Events;
using Project.API.Application.InetgrationEvents;
using DotNetCore.CAP;

namespace Project.API.Application.DomainEventHandlers
{
    public class ProjectViewedDomainEventHandler : INotificationHandler<ProjectViewedEvent>
    {
        private readonly ICapPublisher _capPublisher;

        public ProjectViewedDomainEventHandler(ICapPublisher capPublisher)
        {
            _capPublisher = capPublisher;
        }

        public async Task Handle(ProjectViewedEvent notification, CancellationToken cancellationToken)
        {
            var @event = new ProjectViewedIntegrationEvent
            {
                Viewer = notification.Viewer,
                Introduction = notification.Introduction,
                Company = notification.Company
            };
            await _capPublisher.PublishAsync("projectapi.project.viewed", @event);
        }
    }
}
