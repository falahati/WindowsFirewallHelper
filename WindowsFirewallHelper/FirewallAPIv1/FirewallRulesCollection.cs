using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WindowsFirewallHelper.COMInterop;
using WindowsFirewallHelper.FirewallAPIv1.COMCollectionProxy;
using WindowsFirewallHelper.FirewallAPIv1.Rules;

namespace WindowsFirewallHelper.FirewallAPIv1
{
    public class FirewallRulesCollection : ICollection<IRule>
    {
        private readonly Dictionary<FirewallProfiles, COMApplicationCollection> _firewallApplicationCollections;
        private readonly Dictionary<FirewallProfiles, COMPortCollection> _firewallPortCollections;
        private readonly Dictionary<FirewallProfiles, COMServiceCollection> _firewallServiceCollections;

        public FirewallRulesCollection(FirewallProfile[] profiles)
        {
            _firewallPortCollections = profiles.ToDictionary(
                profile => profile.Type,
                profile => new COMPortCollection(
                    profile.UnderlyingObject.GloballyOpenPorts,
                    profile.Type
                )
            );
            _firewallApplicationCollections = profiles.ToDictionary(
                profile => profile.Type,
                profile => new COMApplicationCollection(
                    profile.UnderlyingObject.AuthorizedApplications,
                    profile.Type
                )
            );
            _firewallServiceCollections = profiles.ToDictionary(
                profile => profile.Type,
                profile => new COMServiceCollection(
                    profile.UnderlyingObject.Services,
                    profile.Type
                )
            );
        }

        /// <inheritdoc />
        // ReSharper disable once MethodNameNotMeaningful
        // ReSharper disable once ExcessiveIndentation
        // ReSharper disable once MethodTooLong
        public void Add(IRule rule)
        {
            if (rule is ApplicationRule applicationRule)
            {
                foreach (var firewallProfile in _firewallApplicationCollections.Keys)
                {
                    if (applicationRule.Profiles.HasFlag(firewallProfile))
                    {
                        _firewallApplicationCollections[firewallProfile].Add(
                            new Tuple<FirewallProfiles, INetFwAuthorizedApplication>(
                                firewallProfile,
                                applicationRule.GetCOMObject(firewallProfile)
                            )
                        );
                    }
                }
            }
            else if (rule is PortRule portRule)
            {
                foreach (var firewallProfile in _firewallPortCollections.Keys)
                {
                    if (portRule.Profiles.HasFlag(firewallProfile))
                    {
                        _firewallPortCollections[firewallProfile].Add(
                            new Tuple<FirewallProfiles, INetFwOpenPort>(
                                firewallProfile,
                                portRule.GetCOMObject(firewallProfile)
                            )
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
        public bool Contains(IRule item)
        {
            return this.Any(rule => rule.Equals(item));
        }

        /// <inheritdoc />
        public void CopyTo(IRule[] array, int arrayIndex)
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
        public bool Remove(IRule rule)
        {
            if (rule is ApplicationRule applicationRule)
            {
                foreach (var firewallProfile in _firewallApplicationCollections.Keys)
                {
                    if (applicationRule.Profiles.HasFlag(firewallProfile))
                    {
                        _firewallApplicationCollections[firewallProfile].Remove(
                            applicationRule.GetCOMKey(firewallProfile)
                        );
                    }
                }
            }
            else if (rule is PortRule portRule)
            {
                foreach (var firewallProfile in _firewallPortCollections.Keys)
                {
                    if (portRule.Profiles.HasFlag(firewallProfile))
                    {
                        _firewallPortCollections[firewallProfile].Remove(
                            portRule.GetCOMKey(firewallProfile)
                        );
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
        public IEnumerator<IRule> GetEnumerator()
        {
            var services = _firewallServiceCollections.SelectMany(pair => pair.Value).ToArray();

            var applicationRules = _firewallApplicationCollections
                .SelectMany(pair => pair.Value)
                .GroupBy(t => Tuple.Create(
                        t.Item2.ProcessImageFileName,
                        t.Item2.RemoteAddresses,
                        t.Item2.Scope
                    )
                )
                .Select(
                    group => new ApplicationRule(group.ToDictionary(t => t.Item1, t => t.Item2))
                )
                .OfType<IRule>();

            var portRules = _firewallPortCollections
                .SelectMany(pair => pair.Value)
                .GroupBy(
                    t => Tuple.Create(
                        t.Item2.Port,
                        t.Item2.Protocol,
                        t.Item2.Scope,
                        t.Item2.RemoteAddresses,
                        t.Item2.BuiltIn
                    )
                )
                .Select(
                    group => new PortRule(group.ToDictionary(t => t.Item1, t => t.Item2))
                )
                .OfType<IRule>();

            return applicationRules.Concat(portRules).GetEnumerator();
        }
    }
}