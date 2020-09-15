using System;

namespace Recommend.API.IntegrationEvents
{
    public class ProjectJoinedIntegrationEvent 
    {
        public string Company { get; set; }
        public string Introduction { get; set; }
       // public ProjectContributor Contributor { get; set; }
    }
}
