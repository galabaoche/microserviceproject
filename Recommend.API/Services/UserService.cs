using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using DnsClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Recommend.API.Dtos;

namespace Recommend.API.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly string _userServiceUrl;
        private readonly HttpClient _httpClient;
        private readonly IDnsQuery _dnsQuery;

        public UserService(IDnsQuery dnsQuery,
            IOptions<ServiceDiscoveryOptions> serviceOption, ILogger<UserService> logger, HttpClient httpClient)
        {
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
            _httpClient = httpClient;
        }
        public async Task<UserIdentity> GetUserIdentityAsync(int userId)
        {
            try
            {
                var response = await _httpClient.GetAsync(_userServiceUrl + $"/api/users/baseinfo/{userId}");
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogTrace($"GetUserIdentityAsync调用成功");
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
                _logger.LogError(ex, "GetUserIdentityAsync:" + ex.Message + ex.StackTrace);
                throw ex;
            }

            return null;
        }
    }
}
