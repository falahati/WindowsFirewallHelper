using System;
using System.Linq;
using System.Net;
using WindowsFirewallHelper.Addresses;
using WindowsFirewallHelper.InternalHelpers;
using NUnit.Framework;

namespace WindowsFirewallHelper.Tests
{
    internal class NetworkAddressTests
    {
        [Test]
        public void InvalidParses()
        {
            // Can't parse empty strings
            Assert.Throws<FormatException>(() =>
                {
                    NetworkAddress.Parse("");
                }
            );

            // Can't parse combined ip address families
            Assert.Throws<FormatException>(() =>
                {
                    NetworkAddress.Parse("127.0.0.1/ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff");
                }
            );
            Assert.Throws<FormatException>(() =>
                {
                    NetworkAddress.Parse("::1/255.255.255.255");
                }
            );

            // Can't parse invalid ip addresses
            Assert.Throws<FormatException>(() =>
                {
                    NetworkAddress.Parse("-1.0.0.0");
                }
            );
            Assert.Throws<FormatException>(() =>
                {
                    NetworkAddress.Parse("-1::");
                }
            );
            Assert.Throws<FormatException>(() =>
                {
                    NetworkAddress.Parse("256.0.0.0");
                }
            );
            Assert.Throws<FormatException>(() =>
                {
                    NetworkAddress.Parse("10000::");
                }
            );

            // Can't parse multi ip subnet with `any` addresses
            Assert.Throws<FormatException>(() =>
                {
                    NetworkAddress.Parse("0.0.0.0/24");
                }
            );
            Assert.Throws<FormatException>(() =>
                {
                    NetworkAddress.Parse("::/112");
                }
            );

            // Can't parse ip addresses with zero subnet mask (which means `any`)
            Assert.Throws<FormatException>(() =>
                {
                    NetworkAddress.Parse("127.0.0.1/0");
                }
            );
            Assert.Throws<FormatException>(() =>
                {
                    NetworkAddress.Parse("::1/0");
                }
            );

            // Can't parse ip addresses with higher than possible subnet mask
            Assert.Throws<FormatException>(() =>
                {
                    NetworkAddress.Parse("127.0.0.1/33");
                }
            );
            Assert.Throws<FormatException>(() =>
                {
                    NetworkAddress.Parse("::1/129");
                }
            );

            // Can't parse ip ranges
            Assert.Throws<FormatException>(() =>
                {
                    NetworkAddress.Parse("192.168.1.1-192.168.2.1");
                }
            );
            Assert.Throws<FormatException>(() =>
                {
                    NetworkAddress.Parse("2001:1::-2001:2::");
                }
            );
        }

        [Test]
        public void ValidParses()
        {
            var addresses = new[]
            {
                "*",
                "0.0.0.0",
                "127.0.0.1",
                "192.168.1.0/255.255.255.255",
                "192.168.2.0/24",
                "192.168.3.0/255.255.0.0",
                "::",
                "::1",
                "2001:1::/ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff",
                "2001:2::/112",
                "2001:3::/ffff:ffff:ffff:ffff:ffff:ffff::"
            };

            var expected = new[]
            {
                new NetworkAddress(IPAddress.Any),
                new NetworkAddress(IPAddress.Any),
                new NetworkAddress(IPAddress.Loopback),
                new NetworkAddress(IPAddress.Parse("192.168.1.0")),
                new NetworkAddress(IPAddress.Parse("192.168.2.0"), IPAddress.Parse("255.255.255.0")),
                new NetworkAddress(IPAddress.Parse("192.168.3.0"), IPAddress.Parse("255.255.0.0")),
                new NetworkAddress(IPAddress.IPv6Any),
                new NetworkAddress(IPAddress.IPv6Loopback),
                new NetworkAddress(IPAddress.Parse("2001:1::")),
                new NetworkAddress(
                    IPAddress.Parse("2001:2::"),
                    IPAddress.Parse("ffff:ffff:ffff:ffff:ffff:ffff:ffff:0")
                ),
                new NetworkAddress(
                    IPAddress.Parse("2001:3::"),
                    IPAddress.Parse("ffff:ffff:ffff:ffff:ffff:ffff::")
                )
            };

            var actual = addresses.Select(NetworkAddress.Parse).ToArray();

            Assert.IsTrue(expected.SequenceEqual(actual));

            var addressesInString = AddressHelper.AddressesToString(actual.OfType<IAddress>().ToArray());

            Assert.AreEqual(
                "*,*,127.0.0.1,192.168.1.0,192.168.2.0/255.255.255.0,192.168.3.0/255.255.0.0," +
                "*,::1,2001:1::,2001:2::/ffff:ffff:ffff:ffff:ffff:ffff:ffff:0,2001:3::/ffff:ffff:ffff:ffff:ffff:ffff::",
                addressesInString
            );
        }
    }
}