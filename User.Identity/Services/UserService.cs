using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text;
using DnsClient;
using Microsoft.Extensions.Options;
using User.Identity.Dtos;
using System.Linq;
using Resilience;
using Microsoft.Extensions.Logging;

namespace User.Identity.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly string _userServiceUrl;
        private IHttpClient _httpClient;
        private readonly IDnsQuery _dnsQuery;

        public UserService(IHttpClient httpClient, IDnsQuery dnsQuery,
            IOptions<ServiceDiscoveryOptions> serviceOption, ILogger<UserService> logger)
        {
            _httpClient = httpClient;
            this._dnsQuery = dnsQuery;
            try
            {
                var result = dnsQuery.ResolveService("service.consul", serviceOption.Value.UserServiceName);
                var addressList = result.FirstOrDefault().AddressList;
                var host = addressList.Any()
                    ? addressList.FirstOrDefault().ToString()
                    : result.FirstOrDefault().HostName;
                var port = result.First().Port;
                _userServiceUrl = $"http://{host}:{port}";
            }
            catch (Exception ex)
            {
                logger.LogError("find user service from consul occur error " + ex.Message);
            }

            _userServiceUrl = $"http://localhost:5000";
            _logger = logger;
        }

        public async Task<UserIdentity> CheckOrCreate(string phone)
        {
            //var stringContent = new StringContent(JsonSerializer.Serialize(phone), Encoding.UTF8, "application/json");
            //var form = new Dictionary<string, string> { { "phone", phone } };
            try
            {
                var response = await _httpClient.PostAsync(_userServiceUrl + "/api/users/check-or-create", phone);
                if (response.IsSuccessStatusCode)
                {
                    using var result = await response.Content.ReadAsStreamAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    return await JsonSerializer.DeserializeAsync<UserIdentity>(result, options);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CheckOrCreateAsync重试失败:" + ex.Message + ex.StackTrace);
                throw ex;
            }

            return null;
        }
    }
}
