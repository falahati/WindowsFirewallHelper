using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WindowsFirewallHelper.FirewallRules;

namespace WindowsFirewallHelper.InternalCollections
{
    internal class FirewallLegacyRulesCollection : ICollection<IFirewallRule>
    {
        private readonly Dictionary<FirewallProfiles, FirewallLegacyApplicationCollection>
            _firewallApplicationCollections;

        private readonly Dictionary<FirewallProfiles, FirewallLegacyPortCollection> _firewallPortCollections;
        private readonly Dictionary<FirewallProfiles, FirewallLegacyServiceCollection> _firewallServiceCollections;

        public FirewallLegacyRulesCollection(FirewallLegacyProfile[] profiles)
        {
            _firewallPortCollections = profiles.ToDictionary(
                profile => profile.Type,
                profile => new FirewallLegacyPortCollection(profile.UnderlyingObject.GloballyOpenPorts)
            );

            _firewallApplicationCollections = profiles.ToDictionary(
                profile => profile.Type,
                profile => new FirewallLegacyApplicationCollection(profile.UnderlyingObject.AuthorizedApplications)
            );

            _firewallServiceCollections = profiles.ToDictionary(
                profile => profile.Type,
                profile => new FirewallLegacyServiceCollection(profile.UnderlyingObject.Services)
            );
        }

        /// <inheritdoc />
        // ReSharper disable once MethodNameNotMeaningful
        // ReSharper disable once ExcessiveIndentation
        // ReSharper disable once MethodTooLong
        public void Add(IFirewallRule rule)
        {
            if (rule is FirewallLegacyApplicationRule applicationRule)
            {
                foreach (var firewallProfile in _firewallApplicationCollections.Keys)
                {
                    if (applicationRule.Profiles.HasFlag(firewallProfile))
                    {
                        _firewallApplicationCollections[firewallProfile].Add(
                            applicationRule.GetCOMObject(firewallProfile)
                        );
                    }
                }
            }
            else if (rule is FirewallLegacyPortRule portRule)
            {
                foreach (var firewallProfile in _firewallPortCollections.Keys)
                {
                    if (portRule.Profiles.HasFlag(firewallProfile))
                    {
                        _firewallPortCollections[firewallProfile].Add(
                            portRule.GetCOMObject(firewallProfile)
                        );
                    }
                }
            }
            else
            {
                throw new ArgumentException("Invalid argument type passed.", nameof(rule));
            }
        }

        /// <inheritdoc />
        public void Clear()
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public bool Contains(IFirewallRule item)
        {
            return this.Any(rule => rule.Equals(item));
        }

        /// <inheritdoc />
        public void CopyTo(IFirewallRule[] array, int arrayIndex)
        {
            var sourceArray = this.ToArray();
            Array.Copy(sourceArray, 0, array, arrayIndex, sourceArray.Length);
        }

        /// <inheritdoc />
        public int Count
        {
            // ReSharper disable once InvokeAsExtensionMethod
            get => Enumerable.Count(this);
        }

        /// <inheritdoc />
        public bool IsReadOnly
        {
            get => _firewallApplicationCollections.Any(pair => pair.Value.IsReadOnly) ||
                   _firewallPortCollections.Any(pair => pair.Value.IsReadOnly);
        }

        /// <inheritdoc />
        // ReSharper disable once ExcessiveIndentation
        // ReSharper disable once MethodTooLong
        public bool Remove(IFirewallRule rule)
        {
            if (rule is FirewallLegacyApplicationRule applicationRule)
            {
                foreach (var firewallProfile in _firewallApplicationCollections.Keys)
                {
                    if (applicationRule.Profiles.HasFlag(firewallProfile))
                    {
                        _firewallApplicationCollections[firewallProfile]
                            .Remove(applicationRule.GetCOMObject(firewallProfile));
                    }
                }
            }
            else if (rule is FirewallLegacyPortRule portRule)
            {
                foreach (var firewallProfile in _firewallPortCollections.Keys)
                {
                    if (portRule.Profiles.HasFlag(firewallProfile))
                    {
                        _firewallPortCollections[firewallProfile].Remove(portRule.GetCOMObject(firewallProfile));
                    }
                }
            }

            throw new ArgumentException("Invalid argument type passed.", nameof(rule));
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc />
        // ReSharper disable once TooManyDeclarations
        public IEnumerator<IFirewallRule> GetEnumerator()
        {
            var applicationRules = _firewallApplicationCollections
                .SelectMany(pair => pair.Value.Select(rule => new {Profile = pair.Key, Rule = rule}))
                .GroupBy(
                    arg => Tuple.Create(
                        arg.Rule.ProcessImageFileName,
                        arg.Rule.RemoteAddresses,
                        arg.Rule.Scope,
                        arg.Rule.IpVersion
                    )
                )
                .Select(
                    group => new FirewallLegacyApplicationRule(group.ToDictionary(t => t.Profile, t => t.Rule))
                )
                .OfType<IFirewallRule>();

            var portRules = _firewallPortCollections
                .SelectMany(pair => pair.Value.Select(rule => new {Profile = pair.Key, Rule = rule}))
                .GroupBy(
                    arg => Tuple.Create(
                        arg.Rule.Port,
                        arg.Rule.Protocol,
                        arg.Rule.Scope,
                        arg.Rule.RemoteAddresses,
                        arg.Rule.BuiltIn,
                        arg.Rule.IpVersion
                    )
                )
                .Select(
                    group => new FirewallLegacyPortRule(group.ToDictionary(t => t.Profile, t => t.Rule))
                )
                .OfType<IFirewallRule>();

            return applicationRules.Concat(portRules).GetEnumerator();
        }
    }
}