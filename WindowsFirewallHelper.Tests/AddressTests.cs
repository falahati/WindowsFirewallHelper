using System.Linq;
using System.Net;
using WindowsFirewallHelper.Addresses;
using WindowsFirewallHelper.InternalHelpers;
using NUnit.Framework;

namespace WindowsFirewallHelper.Tests
{
    internal class AddressTests
    {
        [Test]
        // ReSharper disable once TooManyDeclarations
        public void AddressParse()
        {
            var addresses = new IAddress[]
            {
                new DHCPService(),
                new DNSService(),
                new DefaultGateway(),
                new WINSService(),
                new LocalSubnet(),
                new IPRange(IPAddress.Parse("192.168.0.0"), IPAddress.Parse("192.168.1.0")),
                new NetworkAddress(IPAddress.Parse("10.10.0.0"), IPAddress.Parse("255.255.255.0")),
                new SingleIP(IPAddress.Parse("192.168.1.1")),
                SingleIP.Loopback
            };

            var addressesInString = AddressHelper.AddressesToString(addresses);

            // Check if all addresses resulted in an entry
            var commas = addressesInString.Count(c => c == ',');
            Assert.AreEqual(addresses.Length - 1, commas);

            var addressesParsed = AddressHelper.StringToAddresses(addressesInString);

            // Check if parsing result in the same output
            Assert.IsTrue(addresses.SequenceEqual(addressesParsed));

            addressesInString = AddressHelper.AddressesToString(
                addresses.Concat(new IAddress[] {SingleIP.Any}).ToArray()
            );

            // Check if adding `SingleIP.Any` results in ignoring all other addresses
            Assert.AreEqual("*", addressesInString);
        }

        [Test]
        // ReSharper disable once InconsistentNaming
        public void IPv4MaxMin()
        {
            var ip1 = IPAddress.Parse("192.168.1.1");
            var ip2 = IPAddress.Parse("190.168.1.1");
            var max = AddressHelper.Max(ip1, ip2);
            var min = AddressHelper.Min(ip1, ip2);
            Assert.AreEqual(ip1, max);
            Assert.AreEqual(ip2, min);
        }

        [Test]
        // ReSharper disable once InconsistentNaming
        public void IPv6MaxMin()
        {
            var ip1 = IPAddress.Parse("2607:fea8:4260:31a::9");
            var ip2 = IPAddress.Parse("2607:fea8:4260:315::9");
            var max = AddressHelper.Max(ip1, ip2);
            var min = AddressHelper.Min(ip1, ip2);
            Assert.AreEqual(ip1, max);
            Assert.AreEqual(ip2, min);
        }
    }
}