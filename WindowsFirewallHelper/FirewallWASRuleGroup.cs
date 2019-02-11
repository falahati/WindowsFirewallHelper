using System;
using WindowsFirewallHelper.InternalHelpers;

namespace WindowsFirewallHelper
{
    public class FirewallWASRuleGroup : IEquatable<FirewallWASRuleGroup>
    {
        private readonly FirewallWAS _firewall;

        internal FirewallWASRuleGroup(FirewallWAS firewall, string name)
        {
            _firewall = firewall;
            Name = name;
        }

        public string FriendlyName
        {
            get => NativeHelper.ResolveStringResource(Name);
        }

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

        public static bool operator ==(FirewallWASRuleGroup left, FirewallWASRuleGroup right)
        {
            return Equals(left, right) || left?.Equals(right) == true;
        }

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

        public void DisableRuleGroup(FirewallProfiles profiles)
        {
            _firewall.UnderlyingObject.EnableRuleGroup((int) profiles, Name, false);
        }

        public void EnableRuleGroup(FirewallProfiles profiles)
        {
            _firewall.UnderlyingObject.EnableRuleGroup((int) profiles, Name, true);
        }

        public bool IsRuleGroupEnable(FirewallProfiles profiles)
        {
            return _firewall.UnderlyingObject.IsRuleGroupEnabled((int) profiles, Name);
        }
    }
}