using System;
using FirewallVersion2 = WindowsFirewallHelper.FirewallAPIv2.Firewall;
using FirewallVersion1 = WindowsFirewallHelper.FirewallAPIv1.Firewall;
using FirewallVersion2Rules = WindowsFirewallHelper.FirewallAPIv2.Rules;

namespace WindowsFirewallHelper
{
    /// <summary>
    ///     A static class to manage the current active firewall
    /// </summary>
    // ReSharper disable once HollowTypeName
    public static class FirewallManager
    {
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

                        return FirewallVersion1.Instance;
                    case APIVersion.FirewallAPIv2:
                    case APIVersion.FirewallAPIv2Win7:
                    case APIVersion.FirewallAPIv2Win8:

                        return FirewallVersion2.Instance;
                }

                throw new NotSupportedException();
            }
        }

        /// <summary>
        ///     Returns the list of all registered third party firewalls management instances
        /// </summary>
        public static IFirewall[] ThirdPartyFirewalls
        {
            get => new IFirewall[0];
        }

        /// <summary>
        ///     Returns the API version of the current active Windows Firewall
        /// </summary>
        public static APIVersion Version
        {
            get
            {
                if (FirewallVersion2.IsSupported)
                {
                    if (FirewallVersion2Rules.StandardRuleWin8.IsSupported)
                    {
                        return APIVersion.FirewallAPIv2Win8;
                    }

                    if (FirewallVersion2Rules.StandardRuleWin7.IsSupported)
                    {
                        return APIVersion.FirewallAPIv2Win7;
                    }

                    return APIVersion.FirewallAPIv2;
                }

                if (FirewallVersion1.IsSupported)
                {
                    return APIVersion.FirewallAPIv1;
                }

                return APIVersion.None;
            }
        }

        /// <summary>
        ///     Register an instance of a third party firewall management class
        /// </summary>
        /// <param name="firewall">An instance of the firewall management class</param>
        public static void RegisterFirewall(IFirewall firewall)
        {
        }
    }
}