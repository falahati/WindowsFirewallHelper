using System;
using System.Linq;
using System.Net;
using WindowsFirewallHelper.Addresses;
using NUnit.Framework;

namespace WindowsFirewallHelper.Tests
{
    internal class SingleIPTests
    {
        [Test]
        public void InvalidParses()
        {
            // Can't parse empty strings
            Assert.Throws<FormatException>(() =>
                {
                    SingleIP.Parse("");
                }
            );

            // Can't parse invalid ip addresses
            Assert.Throws<FormatException>(() =>
                {
                    SingleIP.Parse("-1.0.0.0");
                }
            );
            Assert.Throws<FormatException>(() =>
                {
                    SingleIP.Parse("-1::");
                }
            );
            Assert.Throws<FormatException>(() =>
                {
                    SingleIP.Parse("256.0.0.0");
                }
            );
            Assert.Throws<FormatException>(() =>
                {
                    SingleIP.Parse("10000::");
                }
            );

            // Can't parse ip ranges containing more than one ip address
            Assert.Throws<FormatException>(() =>
                {
                    SingleIP.Parse("192.168.1.1-192.168.2.1");
                }
            );
            Assert.Throws<FormatException>(() =>
                {
                    SingleIP.Parse("2001:1::-2001:2::");
                }
            );

            // Can't parse network addresses containing more than one ip address
            Assert.Throws<FormatException>(() =>
                {
                    SingleIP.Parse("127.0.0.1/28");
                }
            );
            Assert.Throws<FormatException>(() =>
                {
                    SingleIP.Parse("::1/112");
                }
            );
            Assert.Throws<FormatException>(() =>
                {
                    SingleIP.Parse("192.168.1.1/255.255.255.0");
                }
            );
            Assert.Throws<FormatException>(() =>
                {
                    SingleIP.Parse("2001:1::/ffff:ffff:ffff:ffff:ffff:ffff:ffff:0");
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
                "192.168.1.0-192.168.1.0",
                "192.168.2.0/255.255.255.255",
                "::",
                "::1",
                "2001:1::-2001:1::",
                "2001:2::/ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff"
            };

            var expected = new[]
            {
                new SingleIP(IPAddress.Any),
                new SingleIP(IPAddress.Any),
                new SingleIP(IPAddress.Loopback),
                new SingleIP(IPAddress.Parse("192.168.1.0")),
                new SingleIP(IPAddress.Parse("192.168.2.0")),
                new SingleIP(IPAddress.IPv6Any),
                new SingleIP(IPAddress.IPv6Loopback),
                new SingleIP(IPAddress.Parse("2001:1::")),
                new SingleIP(IPAddress.Parse("2001:2::"))
            };


            var actual = addresses.Select(SingleIP.Parse).ToArray();

            Assert.IsTrue(expected.SequenceEqual(actual));

            var addressesInString = string.Join(',', actual.Select(address => address.ToString()).ToArray());

            Assert.AreEqual(
                "*,*,127.0.0.1,192.168.1.0,192.168.2.0,*,::1,2001:1::,2001:2::",
                addressesInString
            );
        }
    }
}