using System;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace WindowsFirewallHelper.Tests
{
    public class FirewallWASTests
    {
        private const string RulesPrefix = "WFH_TEST_";
        private FirewallWAS _firewall;

        [Test]
        // ReSharper disable once TooManyDeclarations
        public void CreateApplicationRule()
        {
            var ruleName = RulesPrefix + Guid.NewGuid().ToString("N");
            var fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "ping.exe");
            var rule = _firewall.CreateApplicationRule(
                FirewallProfiles.Domain | FirewallProfiles.Private,
                ruleName,
                FirewallAction.Allow,
                FirewallDirection.Inbound,
                fileName,
                FirewallProtocol.Any
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
        public void CreatePortRule()
        {
            var ruleName = RulesPrefix + Guid.NewGuid().ToString("N");
            var portNumber = (ushort) 10011;
            var rule = _firewall.CreatePortRule(
                FirewallProfiles.Domain | FirewallProfiles.Private,
                ruleName,
                FirewallAction.Allow,
                FirewallDirection.Inbound,
                portNumber,
                FirewallProtocol.TCP
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

        [SetUp]
        public void Setup()
        {
            _firewall = FirewallWAS.Instance;
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