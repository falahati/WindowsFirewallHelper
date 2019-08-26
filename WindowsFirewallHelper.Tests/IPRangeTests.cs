using System;
using System.Linq;
using System.Net;
using WindowsFirewallHelper.Addresses;
using NUnit.Framework;

namespace WindowsFirewallHelper.Tests
{
    public class IPRangeTests
    {
        [Test]
        public void InvalidParses()
        {
            // Can't parse empty strings
            Assert.Throws<FormatException>(() =>
                {
                    IPRange.Parse("");
                }
            );

            // Can't parse combined ip address families
            Assert.Throws<FormatException>(() =>
                {
                    IPRange.Parse("127.0.0.1-ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff");
                }
            );
            Assert.Throws<FormatException>(() =>
                {
                    IPRange.Parse("::1-255.255.255.255");
                }
            );

            // Can't parse invalid ip addresses
            Assert.Throws<FormatException>(() =>
                {
                    IPRange.Parse("-1.0.0.0");
                }
            );
            Assert.Throws<FormatException>(() =>
                {
                    IPRange.Parse("-1::");
                }
            );
            Assert.Throws<FormatException>(() =>
                {
                    IPRange.Parse("256.0.0.0");
                }
            );
            Assert.Throws<FormatException>(() =>
                {
                    IPRange.Parse("10000::");
                }
            );

            // Can't parse ip ranges with `any` addresses inside
            Assert.Throws<FormatException>(() =>
                {
                    IPRange.Parse("0.0.0.0-192.168.1.1");
                }
            );
            Assert.Throws<FormatException>(() =>
                {
                    IPRange.Parse("::-2001::");
                }
            );
            Assert.Throws<FormatException>(() =>
                {
                    IPRange.Parse("192.168.1.1-0.0.0.0");
                }
            );
            Assert.Throws<FormatException>(() =>
                {
                    IPRange.Parse("2001::-::");
                }
            );

            // Can't parse network addresses
            Assert.Throws<FormatException>(() =>
                {
                    IPRange.Parse("127.0.0.1/28");
                }
            );
            Assert.Throws<FormatException>(() =>
                {
                    IPRange.Parse("::1/112");
                }
            );
            Assert.Throws<FormatException>(() =>
                {
                    IPRange.Parse("192.168.1.1/255.255.255.0");
                }
            );
            Assert.Throws<FormatException>(() =>
                {
                    IPRange.Parse("2001:1::/ffff:ffff:ffff:ffff:ffff:ffff:ffff:0");
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
                "192.168.2.0-192.168.2.255",
                "192.168.3.30-192.168.4.100",
                "::",
                "::1",
                "2001:1::-2001:1::",
                "2001:2::-2001:2::ffff",
                "2001:3::1212-2001:4::e1e1"
            };

            var expected = new[]
            {
                new IPRange(IPAddress.Any),
                new IPRange(IPAddress.Any),
                new IPRange(IPAddress.Loopback),
                new IPRange(IPAddress.Parse("192.168.1.0")),
                new IPRange(IPAddress.Parse("192.168.2.0"), IPAddress.Parse("192.168.2.255")),
                new IPRange(IPAddress.Parse("192.168.3.30"), IPAddress.Parse("192.168.4.100")),
                new IPRange(IPAddress.IPv6Any),
                new IPRange(IPAddress.IPv6Loopback),
                new IPRange(IPAddress.Parse("2001:1::")),
                new IPRange(
                    IPAddress.Parse("2001:2::"),
                    IPAddress.Parse("2001:2::ffff")
                ),
                new IPRange(
                    IPAddress.Parse("2001:3::1212"),
                    IPAddress.Parse("2001:4::e1e1")
                )
            };


            var actual = addresses.Select(IPRange.Parse).ToArray();

            Assert.IsTrue(expected.SequenceEqual(actual));

            var addressesInString = string.Join(',', actual.Select(address => address.ToString()).ToArray());

            Assert.AreEqual(
                "*,*,127.0.0.1,192.168.1.0,192.168.2.0-192.168.2.255,192.168.3.30-192.168.4.100," +
                "*,::1,2001:1::,2001:2::-2001:2::ffff,2001:3::1212-2001:4::e1e1",
                addressesInString
            );
        }
    }
}