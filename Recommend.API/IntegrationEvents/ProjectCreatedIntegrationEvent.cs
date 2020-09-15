using System;

namespace Recommend.API.IntegrationEvents
{
    public class ProjectCreatedIntegrationEvent
    {
        public int UserId { get; set; }
        public int ProjectId { get; set; }
        public string ProjectAvatar { get; set; }
        public string Company { get; set; }
        public string Introduction { get; set; }
        public string Tags { get; set; }
        public string FinStage { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
