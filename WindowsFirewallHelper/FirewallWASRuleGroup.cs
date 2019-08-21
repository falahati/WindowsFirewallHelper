using System;
using WindowsFirewallHelper.InternalHelpers;

namespace WindowsFirewallHelper
{
    /// <summary>
    ///     Represents a Windows firewall with advanced security rule group
    /// </summary>
    public class FirewallWASRuleGroup : IEquatable<FirewallWASRuleGroup>
    {
        private readonly FirewallWAS _firewall;

        internal FirewallWASRuleGroup(FirewallWAS firewall, string name)
        {
            _firewall = firewall;
            Name = name;
        }

        /// <summary>
        ///     Gets the resolved name of this rule group
        /// </summary>
        public string FriendlyName
        {
            get => NativeHelper.ResolveStringResource(Name);
        }

        /// <summary>
        ///     Gets the name of this rule group
        /// </summary>
        public string Name { get; }

        /// <inheritdoc />
        public bool Equals(FirewallWASRuleGroup other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return string.Equals(Name, other.Name);
        }

        /// <summary>
        ///     Compares two <see cref="FirewallWASRuleGroup" /> objects for equality
        /// </summary>
        /// <param name="left">A <see cref="FirewallWASRuleGroup" /> object</param>
        /// <param name="right">A <see cref="FirewallWASRuleGroup" /> object</param>
        /// <returns>true if two sides are equal; otherwise false</returns>
        public static bool operator ==(FirewallWASRuleGroup left, FirewallWASRuleGroup right)
        {
            return Equals(left, right) || left?.Equals(right) == true;
        }

        /// <summary>
        ///     Compares two <see cref="FirewallWASRuleGroup" /> objects for inequality
        /// </summary>
        /// <param name="left">A <see cref="FirewallWASRuleGroup" /> object</param>
        /// <param name="right">A <see cref="FirewallWASRuleGroup" /> object</param>
        /// <returns>true if two sides are not equal; otherwise false</returns>
        public static bool operator !=(FirewallWASRuleGroup left, FirewallWASRuleGroup right)
        {
            return !(left == right);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return Equals(obj as FirewallWASRuleGroup);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Name != null ? Name.GetHashCode() : 0;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return FriendlyName;
        }

        /// <summary>
        ///     Disables all rules in specific firewall profile that are part of this rule group
        /// </summary>
        /// <param name="profiles">The firewall profile to disable rules in</param>
        public void DisableRuleGroup(FirewallProfiles profiles)
        {
            _firewall.UnderlyingObject.EnableRuleGroup((int) profiles, Name, false);
        }

        /// <summary>
        ///     Enables all rules in specific firewall profile that are part of this rule group
        /// </summary>
        /// <param name="profiles">The firewall profile to enable rules in</param>
        public void EnableRuleGroup(FirewallProfiles profiles)
        {
            _firewall.UnderlyingObject.EnableRuleGroup((int) profiles, Name, true);
        }

        /// <summary>
        ///     Returns a Boolean value indicating if the rules in the specific firewall profile that belong to this rule group are
        ///     enable
        /// </summary>
        /// <param name="profiles">The firewall profile to check the rules status from</param>
        /// <returns>true if rules are enable; otherwise false.</returns>
        public bool IsRuleGroupEnable(FirewallProfiles profiles)
        {
            return _firewall.UnderlyingObject.IsRuleGroupEnabled((int) profiles, Name);
        }
    }
}