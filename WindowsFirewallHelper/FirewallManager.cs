using System;
using System.ServiceProcess;
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

        public static bool IsServiceRunning
        {
            get
            {
                switch (Version)
                {
                    case APIVersion.FirewallAPIv1:

                        return new ServiceController("SharedAccess").Status == ServiceControllerStatus.Running;
                    case APIVersion.FirewallAPIv2:
                    case APIVersion.FirewallAPIv2Win7:
                    case APIVersion.FirewallAPIv2Win8:

                        return new ServiceController("MpsSvc").Status == ServiceControllerStatus.Running;
                }

                return false;
            }
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
    }
}