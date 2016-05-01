using System;
using System.Collections.Generic;
using Firewallv2 = WindowsFirewallHelper.FirewallAPIv2.Firewall;
using Firewallv1 = WindowsFirewallHelper.FirewallAPIv1.Firewall;
using Firewallv2Rules = WindowsFirewallHelper.FirewallAPIv2.Rules;

namespace WindowsFirewallHelper
{
    /// <summary>
    ///     A static class to manage the current active firewall
    /// </summary>
    public static class FirewallManager
    {
        private static readonly List<IFirewall> ThirdPartyFirewallsBackField = new List<IFirewall>();

        /// <summary>
        ///     Returns the list of all registered third party firewalls management instances
        /// </summary>
        public static IFirewall[] ThirdPartyFirewalls => ThirdPartyFirewallsBackField.ToArray();

        /// <summary>
        ///     Returns the API version of the current active Windows Firewall
        /// </summary>
        public static APIVersion Version
        {
            get
            {
                foreach (var thirdPartyFirewall in ThirdPartyFirewallsBackField)
                {
                    if (thirdPartyFirewall.IsSupported)
                    {
                        return APIVersion.ThirdParty;
                    }
                }
                if (Firewallv2.Instance.IsSupported)
                {
                    if (Firewallv2Rules.StandardRuleWin8.IsSupported)
                    {
                        return APIVersion.FirewallAPIv2Win8;
                    }
                    if (Firewallv2Rules.StandardRuleWin7.IsSupported)
                    {
                        return APIVersion.FirewallAPIv2Win7;
                    }
                    return APIVersion.FirewallAPIv2;
                }
                if (Firewallv1.Instance.IsSupported)
                {
                    return APIVersion.FirewallAPIv1;
                }
                return APIVersion.None;
            }
        }

        /// <summary>
        ///     Returns a instance of the active firewall
        /// </summary>
        public static IFirewall Instance
        {
            get
            {
                switch (Version)
                {
                    case APIVersion.FirewallAPIv1:
                        return Firewallv1.Instance;
                    case APIVersion.FirewallAPIv2:
                    case APIVersion.FirewallAPIv2Win7:
                    case APIVersion.FirewallAPIv2Win8:
                        return Firewallv2.Instance;
                    case APIVersion.ThirdParty:
                        foreach (var thirdPartyFirewall in ThirdPartyFirewallsBackField)
                        {
                            if (thirdPartyFirewall.IsSupported)
                            {
                                return thirdPartyFirewall;
                            }
                        }
                        break;
                }
                throw new NotSupportedException();
            }
        }

        /// <summary>
        ///     Register an instance of a third party firewall management class
        /// </summary>
        /// <param name="firewall">An instance of the firewall management class</param>
        public static void RegisterFirewall(IFirewall firewall)
        {
            if (!ThirdPartyFirewallsBackField.Contains(firewall))
            {
                ThirdPartyFirewallsBackField.Add(firewall);
            }
        }
    }
}