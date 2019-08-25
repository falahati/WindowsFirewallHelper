using System;
using System.IO;
using System.Linq;
using WindowsFirewallHelper.Exceptions;
using NUnit.Framework;

namespace WindowsFirewallHelper.Tests
{
    public class FirewallLegacyTests
    {
        private const string RulesPrefix = "WFH_TEST_";
        private FirewallLegacy _firewall;

        [Test]
        // ReSharper disable once TooManyDeclarations
        public void CreateMultiProfileApplicationRule()
        {
            var ruleName = RulesPrefix + Guid.NewGuid().ToString("N");
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
        // ReSharper disable once TooManyDeclarations
        public void CreateMultiProfilePortRule()
        {
            var ruleName = RulesPrefix + Guid.NewGuid().ToString("N");
            var portNumber = (ushort) 10011;
            var rule = _firewall.CreatePortRule(
                FirewallProfiles.Domain | FirewallProfiles.Private,
                ruleName,
                portNumber
            );

            _firewall.Rules.Add(rule);

            var checkRule = _firewall.Rules.FirstOrDefault(firewallRule => firewallRule.Name == ruleName);
            Assert.IsNotNull(checkRule);
            Assert.IsTrue(checkRule.LocalPorts.Contains(portNumber));
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

            Assert.Throws<FirewallLegacyNotSupportedException>(() =>
                {
                    _firewall.GetProfile(FirewallProfiles.Public);
                }
            );
        }

        [SetUp]
        public void Setup()
        {
            _firewall = FirewallLegacy.Instance;
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
                var rulesToBeDeleted = _firewall.Rules.Where(rule => rule.Name.StartsWith(RulesPrefix)).ToArray();

                foreach (var firewallRule in rulesToBeDeleted)
                {
                    _firewall.Rules.Remove(firewallRule);
                }
            }
            catch (Exception)
            {
                // ignored
            }

            _firewall = null;
        }
    }
}