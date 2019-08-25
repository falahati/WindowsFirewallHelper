using System;
using WindowsFirewallHelper.Addresses;
using NUnit.Framework;

namespace WindowsFirewallHelper.Tests
{
    internal class SpecialAddressTests
    {
        [Test]
        public void DefaultGatewayTests()
        {
            // ReSharper disable once StringLiteralTypo
            var str = "Defaultgateway";
            var address = DefaultGateway.Parse(str);

            Assert.AreEqual(new DefaultGateway(), address);
            Assert.AreEqual(str, address.ToString());

            Assert.Throws<FormatException>(() =>
                {
                    DefaultGateway.Parse("");
                }
            );

            Assert.Throws<FormatException>(() =>
                {
                    DefaultGateway.Parse("*");
                }
            );

            Assert.Throws<FormatException>(() =>
                {
                    DefaultGateway.Parse("127.0.0.1");
                }
            );

            Assert.Throws<FormatException>(() =>
                {
                    DefaultGateway.Parse("SOME_UNKNOWN_STRING");
                }
            );
        }

        [Test]
        public void DHCPServiceTests()
        {
            var str = "DHCP";
            var address = DHCPService.Parse(str);

            Assert.AreEqual(new DHCPService(), address);
            Assert.AreEqual(str, address.ToString());

            Assert.Throws<FormatException>(() =>
                {
                    DHCPService.Parse("");
                }
            );

            Assert.Throws<FormatException>(() =>
                {
                    DHCPService.Parse("*");
                }
            );

            Assert.Throws<FormatException>(() =>
                {
                    DHCPService.Parse("127.0.0.1");
                }
            );

            Assert.Throws<FormatException>(() =>
                {
                    DHCPService.Parse("SOME_UNKNOWN_STRING");
                }
            );
        }

        [Test]
        public void DNSServiceTests()
        {
            var str = "DNS";
            var address = DNSService.Parse(str);

            Assert.AreEqual(new DNSService(), address);
            Assert.AreEqual(str, address.ToString());

            Assert.Throws<FormatException>(() =>
                {
                    DNSService.Parse("");
                }
            );

            Assert.Throws<FormatException>(() =>
                {
                    DNSService.Parse("*");
                }
            );

            Assert.Throws<FormatException>(() =>
                {
                    DNSService.Parse("127.0.0.1");
                }
            );

            Assert.Throws<FormatException>(() =>
                {
                    DNSService.Parse("SOME_UNKNOWN_STRING");
                }
            );
        }
        
        [Test]
        public void LocalSubnetTests()
        {
            var str = "LocalSubnet";
            var address = LocalSubnet.Parse(str);

            Assert.AreEqual(new LocalSubnet(), address);
            Assert.AreEqual(str, address.ToString());

            Assert.Throws<FormatException>(() =>
                {
                    LocalSubnet.Parse("");
                }
            );

            Assert.Throws<FormatException>(() =>
                {
                    LocalSubnet.Parse("*");
                }
            );

            Assert.Throws<FormatException>(() =>
                {
                    LocalSubnet.Parse("127.0.0.1");
                }
            );

            Assert.Throws<FormatException>(() =>
                {
                    LocalSubnet.Parse("SOME_UNKNOWN_STRING");
                }
            );
        }
        
        [Test]
        public void WINSServiceTests()
        {
            var str = "WINS";
            var address = WINSService.Parse(str);

            Assert.AreEqual(new WINSService(), address);
            Assert.AreEqual(str, address.ToString());

            Assert.Throws<FormatException>(() =>
                {
                    WINSService.Parse("");
                }
            );

            Assert.Throws<FormatException>(() =>
                {
                    WINSService.Parse("*");
                }
            );

            Assert.Throws<FormatException>(() =>
                {
                    WINSService.Parse("127.0.0.1");
                }
            );

            Assert.Throws<FormatException>(() =>
                {
                    WINSService.Parse("SOME_UNKNOWN_STRING");
                }
            );
        }
    }
}