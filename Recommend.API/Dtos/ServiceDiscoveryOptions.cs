using System;

namespace Recommend.API.Dtos
{
    public class ServiceDiscoveryOptions
    {
        public string ContactServiceName { get; set; }
        public string UserServiceName { get; set; }

        public ConsulOptions Consul { get; set; }
    }
}
