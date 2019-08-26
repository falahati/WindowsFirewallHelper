using System;
using System.IO;
using System.Linq;
using WindowsFirewallHelper.Exceptions;
using NUnit.Framework;

namespace WindowsFirewallHelper.Tests
{
    public class FirewallLegacyTests
    {
        private const string RulesPrefix = "_WFH_TEST_";
        private FirewallLegacy _firewall;

        [Test]
        // ReSharper disable once TooManyDeclarations
        public void CreateDirectMultiProfileApplicationRule()
        {
            var ruleName = RulesPrefix + Guid.NewGuid().ToString("N");
            var fileName = Path.GetTempFileName();
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
        public void CreateDirectMultiProfilePortRule()
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
        // ReSharper disable once TooManyDeclarations
        public void CreateIndirectApplicationRule()
        {
            var firewall = (IFirewall) _firewall;

            var ruleName1 = RulesPrefix + Guid.NewGuid().ToString("N");
            var ruleName2 = RulesPrefix + Guid.NewGuid().ToString("N");
            var ruleName3 = RulesPrefix + Guid.NewGuid().ToString("N");

            var fileName1 = Path.GetTempFileName();
            var fileName2 = Path.GetTempFileName();
            var fileName3 = Path.GetTempFileName();

            var rule1 = firewall.CreateApplicationRule(
                FirewallProfiles.Private,
                ruleName1,
                fileName1
            );
            var rule2 = firewall.CreateApplicationRule(
                FirewallProfiles.Private,
                ruleName2,
                FirewallAction.Allow,
                fileName2
            );
            var rule3 = firewall.CreateApplicationRule(
                FirewallProfiles.Private,
                ruleName3,
                FirewallAction.Allow,
                fileName3,
                FirewallProtocol.Any
            );

            firewall.Rules.Add(rule1);
            firewall.Rules.Add(rule2);
            firewall.Rules.Add(rule3);

            var checkRule = firewall.Rules.FirstOrDefault(firewallRule => firewallRule.Name == ruleName1);
            Assert.IsNotNull(checkRule);
            Assert.AreEqual(checkRule.ApplicationName.ToLower(), fileName1.ToLower());
            Assert.AreEqual(checkRule.Profiles, FirewallProfiles.Private);
            Assert.AreEqual(checkRule.Action, FirewallAction.Allow);
            Assert.AreEqual(checkRule.Protocol, FirewallProtocol.Any);

            checkRule = firewall.Rules.FirstOrDefault(firewallRule => firewallRule.Name == ruleName2);
            Assert.IsNotNull(checkRule);
            Assert.AreEqual(checkRule.ApplicationName.ToLower(), fileName2.ToLower());
            Assert.AreEqual(checkRule.Profiles, FirewallProfiles.Private);
            Assert.AreEqual(checkRule.Action, FirewallAction.Allow);
            Assert.AreEqual(checkRule.Protocol, FirewallProtocol.Any);

            checkRule = firewall.Rules.FirstOrDefault(firewallRule => firewallRule.Name == ruleName3);
            Assert.IsNotNull(checkRule);
            Assert.AreEqual(checkRule.ApplicationName.ToLower(), fileName3.ToLower());
            Assert.AreEqual(checkRule.Profiles, FirewallProfiles.Private);
            Assert.AreEqual(checkRule.Action, FirewallAction.Allow);
            Assert.AreEqual(checkRule.Protocol, FirewallProtocol.Any);

            firewall.Rules.Remove(rule1);
            firewall.Rules.Remove(rule2);
            firewall.Rules.Remove(rule3);

            checkRule = firewall.Rules.FirstOrDefault(firewallRule => firewallRule.Name == ruleName1);
            Assert.IsNull(checkRule);
            checkRule = firewall.Rules.FirstOrDefault(firewallRule => firewallRule.Name == ruleName2);
            Assert.IsNull(checkRule);
            checkRule = firewall.Rules.FirstOrDefault(firewallRule => firewallRule.Name == ruleName3);
            Assert.IsNull(checkRule);
        }

        [Test]
        public void CreateIndirectBadApplicationRule()
        {
            var firewall = (IFirewall) _firewall;
            var ruleName = RulesPrefix + Guid.NewGuid().ToString("N");
            var appName = Path.GetTempFileName();

            // Public profile is not supported with Windows Firewall Legacy
            Assert.Throws<FirewallLegacyNotSupportedException>(() =>
                {
                    firewall.CreateApplicationRule(
                        FirewallProfiles.Public,
                        ruleName,
                        appName
                    );
                }
            );
            Assert.Throws<FirewallLegacyNotSupportedException>(() =>
                {
                    firewall.CreateApplicationRule(
                        FirewallProfiles.Public | FirewallProfiles.Private,
                        ruleName,
                        appName
                    );
                }
            );

            // Blocking rules are not supported with Windows Firewall Legacy
            Assert.Throws<FirewallLegacyNotSupportedException>(() =>
                {
                    firewall.CreateApplicationRule(
                        FirewallProfiles.Private,
                        ruleName,
                        FirewallAction.Block,
                        appName
                    );
                }
            );

            // Any protocol except Any is not supported with Windows Firewall Legacy
            Assert.Throws<FirewallLegacyNotSupportedException>(() =>
                {
                    firewall.CreateApplicationRule(
                        FirewallProfiles.Private,
                        ruleName,
                        FirewallAction.Allow,
                        appName,
                        FirewallProtocol.TCP
                    );
                }
            );
        }

        [Test]
        public void CreateIndirectBadPortRule()
        {
            var firewall = (IFirewall) _firewall;
            var ruleName = RulesPrefix + Guid.NewGuid().ToString("N");
            ushort portNumber = 10001;

            // Public profile is not supported with Windows Firewall Legacy
            Assert.Throws<FirewallLegacyNotSupportedException>(() =>
                {
                    firewall.CreatePortRule(
                        FirewallProfiles.Public,
                        ruleName,
                        portNumber
                    );
                }
            );
            Assert.Throws<FirewallLegacyNotSupportedException>(() =>
                {
                    firewall.CreatePortRule(
                        FirewallProfiles.Public | FirewallProfiles.Private,
                        ruleName,
                        portNumber
                    );
                }
            );

            // Blocking rules are not supported with Windows Firewall Legacy
            Assert.Throws<FirewallLegacyNotSupportedException>(() =>
                {
                    firewall.CreatePortRule(
                        FirewallProfiles.Private,
                        ruleName,
                        FirewallAction.Block,
                        portNumber
                    );
                }
            );

            // `Any` protocol is not supported with Windows Firewall Legacy in compatibility mode
            if (FirewallWAS.IsSupported)
            {
                Assert.Throws<FirewallLegacyNotSupportedException>(() =>
                    {
                        firewall.CreatePortRule(
                            FirewallProfiles.Private,
                            ruleName,
                            FirewallAction.Allow,
                            portNumber,
                            FirewallProtocol.Any
                        );
                    }
                );
            }

            // Any protocols except TCP, UDP and Any are not supported with Windows Firewall Legacy
            Assert.Throws<FirewallLegacyNotSupportedException>(() =>
                {
                    firewall.CreatePortRule(
                        FirewallProfiles.Private,
                        ruleName,
                        FirewallAction.Allow,
                        portNumber,
                        FirewallProtocol.ICMPv4
                    );
                }
            );
        }

        [Test]
        // ReSharper disable once TooManyDeclarations
        public void CreateIndirectPortRule()
        {
            var firewall = (IFirewall) _firewall;

            var ruleName1 = RulesPrefix + Guid.NewGuid().ToString("N");
            var ruleName2 = RulesPrefix + Guid.NewGuid().ToString("N");
            var ruleName3 = RulesPrefix + Guid.NewGuid().ToString("N");
            var ruleName4 = RulesPrefix + Guid.NewGuid().ToString("N");

            ushort portNumber1 = 10001;
            ushort portNumber2 = 10002;
            ushort portNumber3 = 10003;
            ushort portNumber4 = 10004;

            var rule1 = firewall.CreatePortRule(
                FirewallProfiles.Private,
                ruleName1,
                portNumber1
            );
            var rule2 = firewall.CreatePortRule(
                FirewallProfiles.Private,
                ruleName2,
                FirewallAction.Allow,
                portNumber2
            );
            var rule3 = firewall.CreatePortRule(
                FirewallProfiles.Private,
                ruleName3,
                FirewallAction.Allow,
                portNumber3,
                FirewallProtocol.TCP
            );
            var rule4 = firewall.CreatePortRule(
                FirewallProfiles.Domain,
                ruleName4,
                FirewallAction.Allow,
                portNumber4,
                FirewallProtocol.UDP
            );

            firewall.Rules.Add(rule1);
            firewall.Rules.Add(rule2);
            firewall.Rules.Add(rule3);
            firewall.Rules.Add(rule4);

            var checkRule = firewall.Rules.FirstOrDefault(firewallRule => firewallRule.Name == ruleName1);
            Assert.IsNotNull(checkRule);
            Assert.AreEqual(checkRule.LocalPorts, new[] {portNumber1});
            Assert.AreEqual(checkRule.Profiles, FirewallProfiles.Private);
            Assert.AreEqual(checkRule.Action, FirewallAction.Allow);
            Assert.AreEqual(checkRule.Protocol, FirewallProtocol.TCP);

            checkRule = firewall.Rules.FirstOrDefault(firewallRule => firewallRule.Name == ruleName2);
            Assert.IsNotNull(checkRule);
            Assert.AreEqual(checkRule.LocalPorts, new[] {portNumber2});
            Assert.AreEqual(checkRule.Profiles, FirewallProfiles.Private);
            Assert.AreEqual(checkRule.Action, FirewallAction.Allow);
            Assert.AreEqual(checkRule.Protocol, FirewallProtocol.TCP);

            checkRule = firewall.Rules.FirstOrDefault(firewallRule => firewallRule.Name == ruleName3);
            Assert.IsNotNull(checkRule);
            Assert.AreEqual(checkRule.LocalPorts, new[] {portNumber3});
            Assert.AreEqual(checkRule.Profiles, FirewallProfiles.Private);
            Assert.AreEqual(checkRule.Action, FirewallAction.Allow);
            Assert.AreEqual(checkRule.Protocol, FirewallProtocol.TCP);

            checkRule = firewall.Rules.FirstOrDefault(firewallRule => firewallRule.Name == ruleName4);
            Assert.IsNotNull(checkRule);
            Assert.AreEqual(checkRule.LocalPorts, new[] {portNumber4});
            Assert.AreEqual(checkRule.Profiles, FirewallProfiles.Domain);
            Assert.AreEqual(checkRule.Action, FirewallAction.Allow);
            Assert.AreEqual(checkRule.Protocol, FirewallProtocol.UDP);

            firewall.Rules.Remove(rule1);
            firewall.Rules.Remove(rule2);
            firewall.Rules.Remove(rule3);
            firewall.Rules.Remove(rule4);

            checkRule = firewall.Rules.FirstOrDefault(firewallRule => firewallRule.Name == ruleName1);
            Assert.IsNull(checkRule);
            checkRule = firewall.Rules.FirstOrDefault(firewallRule => firewallRule.Name == ruleName2);
            Assert.IsNull(checkRule);
            checkRule = firewall.Rules.FirstOrDefault(firewallRule => firewallRule.Name == ruleName3);
            Assert.IsNull(checkRule);
            checkRule = firewall.Rules.FirstOrDefault(firewallRule => firewallRule.Name == ruleName4);
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