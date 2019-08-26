using System;
using WindowsFirewallHelper.Addresses;
using NUnit.Framework;

namespace WindowsFirewallHelper.Tests
{
    public class SpecialAddressTests
    {
        [Test]
        public void DefaultGatewayValidParse()
        {
            // ReSharper disable once StringLiteralTypo
            var str = "Defaultgateway";
            var address = SpecialAddress.Parse(str);

            Assert.AreEqual(new DefaultGateway(), address);
            Assert.AreEqual(str, address.ToString());
        }

        [Test]
        public void DHCPServiceValidParse()
        {
            var str = "DHCP";
            var address = SpecialAddress.Parse(str);

            Assert.AreEqual(new DHCPService(), address);
            Assert.AreEqual(str, address.ToString());
        }

        [Test]
        public void DNSServiceValidParse()
        {
            var str = "DNS";
            var address = SpecialAddress.Parse(str);

            Assert.AreEqual(new DNSService(), address);
            Assert.AreEqual(str, address.ToString());
        }

        [Test]
        public void LocalSubnetValidParse()
        {
            var str = "LocalSubnet";
            var address = SpecialAddress.Parse(str);

            Assert.AreEqual(new LocalSubnet(), address);
            Assert.AreEqual(str, address.ToString());
        }

        [Test]
        public void SpecialAddressInvalidParses()
        {
            Assert.Throws<FormatException>(() =>
                {
                    SpecialAddress.Parse("");
                }
            );
            Assert.Throws<FormatException>(() =>
                {
                    SpecialAddress.Parse("SOME_UNKNOWN_STRING");
                }
            );
            Assert.Throws<FormatException>(() =>
                {
                    SpecialAddress.Parse("*");
                }
            );

            Assert.Throws<FormatException>(() =>
                {
                    SpecialAddress.Parse("192.168.1.0");
                }
            );
            Assert.Throws<FormatException>(() =>
                {
                    SpecialAddress.Parse("2001:1::");
                }
            );

            Assert.Throws<FormatException>(() =>
                {
                    SpecialAddress.Parse("192.168.2.0/24");
                }
            );
            Assert.Throws<FormatException>(() =>
                {
                    SpecialAddress.Parse("2001:1::/112");
                }
            );

            Assert.Throws<FormatException>(() =>
                {
                    SpecialAddress.Parse("192.168.3.0-192.168.4.0");
                }
            );
            Assert.Throws<FormatException>(() =>
                {
                    SpecialAddress.Parse("2001:2::-2001:3::");
                }
            );
        }

        [Test]
        public void WINSServiceValidParse()
        {
            var str = "WINS";
            var address = SpecialAddress.Parse(str);

            Assert.AreEqual(new WINSService(), address);
            Assert.AreEqual(str, address.ToString());
        }
    }
}