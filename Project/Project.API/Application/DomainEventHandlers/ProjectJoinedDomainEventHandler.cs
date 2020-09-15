using System;
using System.Threading;
using System.Threading.Tasks;
using DotNetCore.CAP;
using MediatR;
using Project.Domain.AggregatesModel;
using Project.API.Application.InetgrationEvents;
using Project.Domain.Events;

namespace Project.API.Application.DomainEventHandlers
{
    public class ProjectJoinedDomainEventHandler : INotificationHandler<ProjectJoinedEvent>
    {
        private readonly ICapPublisher _capPublisher;

        public ProjectJoinedDomainEventHandler(ICapPublisher capPublisher)
        {
            _capPublisher = capPublisher;
        }

        public async Task Handle(ProjectJoinedEvent notification, CancellationToken cancellationToken)
        {
            var @event = new ProjectJoinedIntegrationEvent
            {
                Contributor = notification.Contributor,
                Introduction = notification.Introduction,
                Company = notification.Company
            };
            await _capPublisher.PublishAsync("projectapi.project.joined", @event);
        }
    }
}
