using System;
using System.IO;
using System.Linq;
using WindowsFirewallHelper.Exceptions;
using NUnit.Framework;

namespace WindowsFirewallHelper.Tests
{
    public class FirewallLegacyTests
    {
        private FirewallLegacy _firewall;

        [Test]
        // ReSharper disable once TooManyDeclarations
        public void CreateMultiProfileApplicationRule()
        {
            var ruleName = Guid.NewGuid().ToString("N");
            var fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "ping.exe");
            var rule = _firewall.CreateApplicationRule(
                FirewallProfiles.Domain | FirewallProfiles.Private,
                ruleName,
                fileName
            );

            _firewall.Rules.Add(rule);

            var checkRule = _firewall.Rules.FirstOrDefault(firewallRule => firewallRule.Name == ruleName);
            Assert.IsNotNull(checkRule);
            Assert.AreEqual(checkRule.ApplicationName.ToLower(), fileName.ToLower());
            Assert.AreEqual(checkRule.Profiles, FirewallProfiles.Domain | FirewallProfiles.Private);

            _firewall.Rules.Remove(rule);

            checkRule = _firewall.Rules.FirstOrDefault(firewallRule => firewallRule.Name == ruleName);
            Assert.IsNull(checkRule);
        }

        [Test]
        public void LegacyProfiles()
        {
            var profiles = _firewall.Profiles.Select(profile => profile.Type).ToArray();
            Assert.AreEqual(profiles.Length, 2);
            Assert.IsTrue(profiles.All(
                type => new[] {FirewallProfiles.Domain, FirewallProfiles.Private}.Contains(type))
            );
            Assert.IsNotNull(_firewall.GetProfile(FirewallProfiles.Domain));
            Assert.IsNotNull(_firewall.GetProfile(FirewallProfiles.Private));

            try
            {
                Assert.IsNull(_firewall.GetProfile(FirewallProfiles.Public));
                Assert.Fail("No exception is thrown.");
            }
            catch (FirewallLegacyNotSupportedException e)
            {
                Assert.Pass(e.Message);
            }
        }

        [SetUp]
        public void Setup()
        {
            _firewall = FirewallLegacy.Instance;
        }

        [TearDown]
        public void TearDown()
        {
            _firewall = null;
        }
    }
}