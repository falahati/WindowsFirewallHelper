using System;
using WindowsFirewallHelper.Helpers;

namespace WindowsFirewallHelper.FirewallAPIv2
{
    internal class FirewallRuleCollectionKey : COMCollectionKey
    {
        public FirewallRuleCollectionKey(string name)
        {
            Name = name;
        }

        public string Name { get; }

        /// <inheritdoc />
        public override Type[] Types
        {
            get => new[] {typeof(string)};
        }

        /// <inheritdoc />
        public override object[] Values
        {
            get => new object[] {Name};
        }
    }
}