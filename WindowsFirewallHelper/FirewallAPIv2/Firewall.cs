using System;
using System.Collections.Generic;
using WindowsFirewallHelper.FirewallAPIv2.Rules;
using NetFwTypeLib;

namespace WindowsFirewallHelper.FirewallAPIv2
{
    /// <summary>
    ///     Contains properties and methods of Windows Firewall with Advanced Security
    /// </summary>
    public class Firewall : IFirewall
    {
        private static Firewall _instance;
        private static readonly object InstanceLock = new object();

        private readonly ActiveCollection<IRule> _rules = new ActiveCollection<IRule>();

        private Firewall()
        {
            if (Type.GetTypeFromProgID(@"HNetCfg.FwPolicy2", false) != null)
            {
                UnderlyingObject =
                    (INetFwPolicy2) Activator.CreateInstance(Type.GetTypeFromProgID(@"HNetCfg.FwPolicy2"));
                Profiles = new IProfile[]
                {
                    new FirewallProfile(UnderlyingObject, NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_DOMAIN),
                    new FirewallProfile(UnderlyingObject, NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_PRIVATE),
                    new FirewallProfile(UnderlyingObject, NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_PUBLIC)
                };
            }
        }

        /// <summary>
        ///     Gets the current singleton instance of this class
        /// </summary>
        public static Firewall Instance
        {
            get
            {
                lock (InstanceLock)
                {
                    return _instance ?? (_instance = new Firewall());
                }
            }
        }

        internal INetFwPolicy2 UnderlyingObject { get; }

        /// <summary>
        ///     Creates a rule about an executable file (application) to be registered to a firewall profile
        /// </summary>
        /// <param name="profiles">The profile or profiles that the rule belongs to</param>
        /// <param name="name">Name of the rule</param>
        /// <param name="action">Action of the rule</param>
        /// <param name="filename">Address of the executable file that the rule applies to</param>
        /// <param name="protocol">Protocol that the rule applies to</param>
        /// <returns>Returns the newly created rule object implementing <see cref="IRule" /> interface</returns>
        /// <exception cref="NotSupportedException">This class is not supported on this machine</exception>
        public IRule CreateApplicationRule(FirewallProfiles profiles, string name, FirewallAction action,
            string filename,
            FirewallProtocol protocol)
        {
            if (!IsSupported)
                throw new NotSupportedException();
            return new StandardRule(name, filename, action, FirewallDirection.Inbound, profiles) {Protocol = protocol};
        }

        /// <summary>
        ///     Creates a rule about an executable file (application) to be registered to a firewall profile
        /// </summary>
        /// <param name="profiles">The profile or profiles that the rule belongs to</param>
        /// <param name="name">Name of the rule</param>
        /// <param name="action">Action of the rule</param>
        /// <param name="filename">Address of the executable file that the rule applies to</param>
        /// <returns>Returns the newly created rule object implementing <see cref="IRule" /> interface</returns>
        /// <exception cref="NotSupportedException">This class is not supported on this machine</exception>
        public IRule CreateApplicationRule(FirewallProfiles profiles, string name, FirewallAction action,
            string filename)
        {
            return CreateApplicationRule(profiles, name, action, filename, FirewallProtocol.Any);
        }

        /// <summary>
        ///     Creates a rule about an executable file (application) to be registered to a firewall profile
        /// </summary>
        /// <param name="profiles">The profile or profiles that the rule belongs to</param>
        /// <param name="name">Name of the rule</param>
        /// <param name="filename">Address of the executable file that the rule applies to</param>
        /// <returns>Returns the newly created rule object implementing <see cref="IRule" /> interface</returns>
        /// <exception cref="NotSupportedException">This class is not supported on this machine</exception>
        public IRule CreateApplicationRule(FirewallProfiles profiles, string name, string filename)
        {
            return CreateApplicationRule(profiles, name, FirewallAction.Allow, filename);
        }


        /// <summary>
        ///     Creates a rule about a port to be registered to a firewall profile
        /// </summary>
        /// <param name="profiles">The profile or profiles that the rule belongs to</param>
        /// <param name="name">Name of the rule</param>
        /// <param name="action">Action of the rule</param>
        /// <param name="portNumber">Port number that the rule applies to</param>
        /// <param name="protocol">Protocol that the rule applies to</param>
        /// <returns>Returns the newly created rule object implementing <see cref="IRule" /> interface</returns>
        /// <exception cref="NotSupportedException">This class is not supported on this machine</exception>
        public IRule CreatePortRule(FirewallProfiles profiles, string name, FirewallAction action, ushort portNumber,
            FirewallProtocol protocol)
        {
            if (!IsSupported)
                throw new NotSupportedException();
            return new StandardRule(name, portNumber, action, FirewallDirection.Inbound, profiles) {Protocol = protocol};
        }

        /// <summary>
        ///     Creates a rule about a port to be registered to a firewall profile
        /// </summary>
        /// <param name="profiles">The profile or profiles that the rule belongs to</param>
        /// <param name="name">Name of the rule</param>
        /// <param name="action">Action of the rule</param>
        /// <param name="portNumber">Port number that the rule applies to</param>
        /// <returns>Returns the newly created rule object implementing <see cref="IRule" /> interface</returns>
        /// <exception cref="NotSupportedException">This class is not supported on this machine</exception>
        public IRule CreatePortRule(FirewallProfiles profiles, string name, FirewallAction action, ushort portNumber)
        {
            return CreatePortRule(profiles, name, action, portNumber, FirewallProtocol.Any);
        }

        /// <summary>
        ///     Creates a rule about a port to be registered to a firewall profile
        /// </summary>
        /// <param name="profiles">The profile or profiles that the rule belongs to</param>
        /// <param name="name">Name of the rule</param>
        /// <param name="portNumber">Port number that the rule applies to</param>
        /// <returns>Returns the newly created rule object implementing <see cref="IRule" /> interface</returns>
        /// <exception cref="NotSupportedException">This class is not supported on this machine</exception>
        public IRule CreatePortRule(FirewallProfiles profiles, string name, ushort portNumber)
        {
            return CreatePortRule(profiles, name, FirewallAction.Allow, portNumber);
        }

        /// <summary>
        ///     Returns a specific firewall profile
        /// </summary>
        /// <param name="profile">Requested firewall profile</param>
        /// <returns>Firewall profile object implementing <see cref="IProfile" /> interface</returns>
        /// <exception cref="NotSupportedException">This class is not supported on this machine</exception>
        /// <exception cref="FirewallAPIv2NotSupportedException">The asked profile is not supported with this class</exception>
        public IProfile GetProfile(FirewallProfiles profile)
        {
            if (!IsSupported)
                throw new NotSupportedException();
            foreach (var p in Profiles)
                if (p.Type == profile)
                    return p;
            throw new FirewallAPIv2NotSupportedException();
        }

        /// <summary>
        ///     Returns the active firewall profile, if any
        /// </summary>
        /// <returns>
        ///     The active firewall profile object implementing <see cref="IProfile" /> interface or null if no firewall
        ///     profile is currently active
        /// </returns>
        public IProfile GetProfile()
        {
            if (!IsSupported)
                throw new NotSupportedException();
            foreach (var p in Profiles)
                if (p.IsActive)
                    return p;
            return null;
        }

        /// <summary>
        ///     Gets a Boolean value showing if the firewall is supported in this environment.
        /// </summary>
        public bool IsSupported => UnderlyingObject != null;

        /// <summary>
        ///     Gets the name of the firewall
        /// </summary>
        public string Name => "Windows Firewall with Advanced Security";

        /// <summary>
        ///     Gets the list of all available profiles of the firewall
        /// </summary>
        public IProfile[] Profiles { get; }

        /// <summary>
        ///     Gets the list of all registered rules of the firewall
        /// </summary>
        public IList<IRule> Rules
        {
            get
            {
                SyncRules();
                return _rules;
            }
        }

        private void RulesOnItemsModified(object sender, ActiveCollectionChangedEventArgs<IRule> e)
        {
            lock (_rules)
            {
                if (e.ActionType == ActiveCollectionChangeType.Added)
                {
                    var rule = e.Item as StandardRule;
                    if (rule != null)
                        UnderlyingObject.Rules.Add(rule.UnderlyingObject);
                }
                else if (e.ActionType == ActiveCollectionChangeType.Removed)
                {
                    var rule = e.Item as StandardRule;
                    if (rule != null)
                        UnderlyingObject.Rules.Remove(rule.UnderlyingObject.Name);
                }
            }
            SyncRules();
        }


        private void SyncRules()
        {
            lock (_rules)
            {
                var rules = new List<IRule>();
                foreach (var rule in UnderlyingObject.Rules)
                    if (rule is INetFwRule3)
                        rules.Add(new StandardRuleWin8((INetFwRule3) rule));
                    else if (rule is INetFwRule2)
                        rules.Add(new StandardRuleWin7((INetFwRule2) rule));
                    else if (rule is INetFwRule)
                        rules.Add(new StandardRule((INetFwRule) rule));
                _rules.ItemsModified -= RulesOnItemsModified;
                _rules.Sync(rules.ToArray());
                _rules.ItemsModified += RulesOnItemsModified;
            }
        }
    }
}