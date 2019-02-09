using System;
using WindowsFirewallHelper.Helpers;

namespace WindowsFirewallHelper.FirewallAPIv2
{
    public class FirewallRuleGroup : IEquatable<FirewallRuleGroup>
    {
        private readonly Firewall _firewall;

        internal FirewallRuleGroup(Firewall firewall, string name)
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
        public bool Equals(FirewallRuleGroup other)
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

        public static bool operator ==(FirewallRuleGroup left, FirewallRuleGroup right)
        {
            return Equals(left, right) || left?.Equals(right) == true;
        }

        public static bool operator !=(FirewallRuleGroup left, FirewallRuleGroup right)
        {
            return !(left == right);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return Equals(obj as FirewallRuleGroup);
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