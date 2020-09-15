using System;
using Project.Domain.AggregatesModel;

namespace Project.API.Application.InetgrationEvents
{
    public class ProjectViewedIntegrationEvent
    {
        public string Company { get; set; }
        public string Introduction { get; set; }
        public ProjectViewer Viewer { get; set; }
    }
}
