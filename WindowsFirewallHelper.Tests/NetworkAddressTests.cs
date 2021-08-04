using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using WindowsFirewallHelper.Addresses;
using NUnit.Framework;

namespace WindowsFirewallHelper.Tests
{
    public class NetworkAddressTests
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

            var addressesInString = string.Join(',', actual.Select(address => address.ToString()).ToArray());

            Assert.AreEqual(
                "*,*,127.0.0.1,192.168.1.0,192.168.2.0/255.255.255.0,192.168.3.0/255.255.0.0," +
                "*,::1,2001:1::,2001:2::/ffff:ffff:ffff:ffff:ffff:ffff:ffff:0,2001:3::/ffff:ffff:ffff:ffff:ffff:ffff::",
                addressesInString
            );
        }

        [Test]
        public void CheckRanges()
        {
            var expectedRanges = new Dictionary<string, Tuple<string, string>>()
            {
                {
                    "1.1.1.1/32", new Tuple<string, string>("1.1.1.1", "1.1.1.1")
                },
                {
                    "1.1.1.1/31", new Tuple<string, string>("1.1.1.0", "1.1.1.1")
                },
                {
                    "1.1.1.1/30", new Tuple<string, string>("1.1.1.0", "1.1.1.3")
                },
                {
                    "1.1.1.1/29", new Tuple<string, string>("1.1.1.0", "1.1.1.7")
                },
                {
                    "1.1.1.1/28", new Tuple<string, string>("1.1.1.0", "1.1.1.15")
                },
                {
                    "1.1.1.1/27", new Tuple<string, string>("1.1.1.0", "1.1.1.31")
                },
                {
                    "1.1.1.1/26", new Tuple<string, string>("1.1.1.0", "1.1.1.63")
                },
                {
                    "1.1.1.1/25", new Tuple<string, string>("1.1.1.0", "1.1.1.127")
                },
                {
                    "1.1.1.1/24", new Tuple<string, string>("1.1.1.0", "1.1.1.255")
                },
                {
                    "1.1.1.1/23", new Tuple<string, string>("1.1.0.0", "1.1.1.255")
                },
                {
                    "1.1.1.1/9", new Tuple<string, string>("1.0.0.0", "1.127.255.255")
                },
                {
                    "1.1.1.1/8", new Tuple<string, string>("1.0.0.0", "1.255.255.255")
                },
                {
                    "1.1.1.1/7", new Tuple<string, string>("0.0.0.0", "1.255.255.255")
                },
                {
                    "1.1.1.1/6", new Tuple<string, string>("0.0.0.0", "3.255.255.255")
                },
                {
                    "1.1.1.1/5", new Tuple<string, string>("0.0.0.0", "7.255.255.255")
                },
                {
                    "1.1.1.1/4", new Tuple<string, string>("0.0.0.0", "15.255.255.255")
                },
                {
                    "1.1.1.1/3", new Tuple<string, string>("0.0.0.0", "31.255.255.255")
                },
                {
                    "1.1.1.1/2", new Tuple<string, string>("0.0.0.0", "63.255.255.255")
                },
                {
                    "1.1.1.1/1", new Tuple<string, string>("0.0.0.0", "127.255.255.255")
                },
                {
                    "127.127.127.127/32", new Tuple<string, string>("127.127.127.127", "127.127.127.127")
                },
                {
                    "127.127.127.127/31", new Tuple<string, string>("127.127.127.126", "127.127.127.127")
                },
                {
                    "127.127.127.127/30", new Tuple<string, string>("127.127.127.124", "127.127.127.127")
                },
                {
                    "127.127.127.127/29", new Tuple<string, string>("127.127.127.120", "127.127.127.127")
                },
                {
                    "127.127.127.127/25", new Tuple<string, string>("127.127.127.0", "127.127.127.127")
                },
                {
                    "127.127.127.127/24", new Tuple<string, string>("127.127.127.0", "127.127.127.255")
                },
                {
                    "127.127.127.127/23", new Tuple<string, string>("127.127.126.0", "127.127.127.255")
                },
                {
                    "127.127.127.127/3", new Tuple<string, string>("96.0.0.0", "127.255.255.255")
                },
                {
                    "127.127.127.127/2", new Tuple<string, string>("64.0.0.0", "127.255.255.255")
                },
                {
                    "127.127.127.127/1", new Tuple<string, string>("0.0.0.0", "127.255.255.255")
                },
                {
                    "255.255.255.255/32", new Tuple<string, string>("255.255.255.255", "255.255.255.255")
                },
                {
                    "255.255.255.255/31", new Tuple<string, string>("255.255.255.254", "255.255.255.255")
                },
                {
                    "255.255.255.255/30", new Tuple<string, string>("255.255.255.252", "255.255.255.255")
                },
                {
                    "255.255.255.255/29", new Tuple<string, string>("255.255.255.248", "255.255.255.255")
                },
                {
                    "255.255.255.255/25", new Tuple<string, string>("255.255.255.128", "255.255.255.255")
                },
                {
                    "255.255.255.255/24", new Tuple<string, string>("255.255.255.0", "255.255.255.255")
                },
                {
                    "255.255.255.255/23", new Tuple<string, string>("255.255.254.0", "255.255.255.255")
                },
                {
                    "255.255.255.255/3", new Tuple<string, string>("224.0.0.0", "255.255.255.255")
                },
                {
                    "255.255.255.255/2", new Tuple<string, string>("192.0.0.0", "255.255.255.255")
                },
                {
                    "255.255.255.255/1", new Tuple<string, string>("128.0.0.0", "255.255.255.255")
                },
            };

            foreach (var expectedRange in expectedRanges)
            {
                var address = NetworkAddress.Parse(expectedRange.Key);
                Assert.AreEqual(IPAddress.Parse(expectedRange.Value.Item1), address.StartAddress, "Start of {0}", expectedRange.Key);
                Assert.AreEqual(IPAddress.Parse(expectedRange.Value.Item2), address.EndAddress, "End of {0}", expectedRange.Key);
            }
        }
    }
}