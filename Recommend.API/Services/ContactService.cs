using System;
using System.Collections.Generic;
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
    public class ContactService : IContactService
    {
        private readonly ILogger<ContactService> _logger;
        private readonly string _contactServiceUrl;
        private readonly HttpClient _httpClient;
        private readonly IDnsQuery _dnsQuery;

        public ContactService(IDnsQuery dnsQuery,
            IOptions<ServiceDiscoveryOptions> serviceOption, ILogger<ContactService> logger, HttpClient httpClient)
        {

            this._dnsQuery = dnsQuery;
            try
            {
                var result = dnsQuery.ResolveService("service.consul", serviceOption.Value.ContactServiceName);
                var addressList = result.FirstOrDefault().AddressList;
                var host = addressList.Any()
                    ? addressList.FirstOrDefault().ToString()
                    : result.FirstOrDefault().HostName;
                var port = result.First().Port;
                _contactServiceUrl = $"http://{host}:{port}";
            }
            catch (Exception ex)
            {
                logger.LogError("find contact service from consul occur error " + ex.Message);
            }

            _contactServiceUrl = $"http://localhost:5002";
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<List<Contact>> GetContactsByUserIdAsync(int userId)
        {
            try
            {
                var response = await _httpClient.GetAsync(_contactServiceUrl + $"/api/contacts/{userId}");
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogTrace($"GetContactsByUserIdAsync调用成功");
                    using var result = await response.Content.ReadAsStreamAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    return await JsonSerializer.DeserializeAsync<List<Contact>>(result, options);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetContactsByUserIdAsync:" + ex.Message + ex.StackTrace);
                throw ex;
            }

            return null;
        }
    }
}
