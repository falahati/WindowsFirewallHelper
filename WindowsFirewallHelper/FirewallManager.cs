using System;
using System.ServiceProcess;
using WindowsFirewallHelper.FirewallRules;

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
                    case FirewallAPIVersion.FirewallAPIv1:

                        return FirewallLegacy.Instance;
                    case FirewallAPIVersion.FirewallAPIv2:
                    case FirewallAPIVersion.FirewallAPIv2Win7:
                    case FirewallAPIVersion.FirewallAPIv2Win8:

                        return FirewallWAS.Instance;
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
                    case FirewallAPIVersion.FirewallAPIv1:

                        return new ServiceController("SharedAccess").Status == ServiceControllerStatus.Running;
                    case FirewallAPIVersion.FirewallAPIv2:
                    case FirewallAPIVersion.FirewallAPIv2Win7:
                    case FirewallAPIVersion.FirewallAPIv2Win8:

                        return new ServiceController("MpsSvc").Status == ServiceControllerStatus.Running;
                }

                return false;
            }
        }


        /// <summary>
        ///     Returns the API version of the current active Windows Firewall
        /// </summary>
        public static FirewallAPIVersion Version
        {
            get
            {
                if (FirewallWAS.IsSupported)
                {
                    if (FirewallWASRuleWin8.IsSupported)
                    {
                        return FirewallAPIVersion.FirewallAPIv2Win8;
                    }

                    if (FirewallWASRuleWin7.IsSupported)
                    {
                        return FirewallAPIVersion.FirewallAPIv2Win7;
                    }

                    return FirewallAPIVersion.FirewallAPIv2;
                }

                if (FirewallLegacy.IsSupported)
                {
                    return FirewallAPIVersion.FirewallAPIv1;
                }

                return FirewallAPIVersion.None;
            }
        }
    }
}