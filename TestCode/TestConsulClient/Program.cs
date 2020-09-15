using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DnsClient;

namespace TestConsulClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                IDnsQuery dnsQuery = new LookupClient(IPAddress.Parse("127.0.0.1"), 8600);
                var result = dnsQuery.ResolveService("service.consul", "TestConsulApi");
                foreach (var item in result)
                {
                    var addressList = item.AddressList;

                    var address = addressList.Any() ? addressList.First().ToString() : item.HostName;

                    var port = item.Port;

                    Console.WriteLine($"consul地址: http://:{address}:{port}");

                    Console.WriteLine($"consul地址: http://:{address}:{port}/Values");
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Console.ReadKey();
            // var dnsQuery = new LookupClient(IPAddress.Parse("127.0.0.1"), 8600);
            // var result = dnsQuery.ResolveService("service.consul", "TestConsulApi");
            // foreach (var item in result)
            // {

            // }
            // System.Console.WriteLine(result);
            // Console.ReadLine();

        }
    }
}
