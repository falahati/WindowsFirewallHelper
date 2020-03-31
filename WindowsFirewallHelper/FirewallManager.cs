using System;
using System.ServiceProcess;
using WindowsFirewallHelper.Collections;
using WindowsFirewallHelper.COMInterop;
using WindowsFirewallHelper.FirewallRules;
using WindowsFirewallHelper.InternalHelpers;

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
                    case FirewallAPIVersion.FirewallLegacy:

                        return FirewallLegacy.Instance;
                    case FirewallAPIVersion.FirewallWAS:
                    case FirewallAPIVersion.FirewallWASWin7:
                    case FirewallAPIVersion.FirewallWASWin8:

                        return FirewallWAS.Instance;
                }

                throw new NotSupportedException();
            }
        }

        /// <summary>
        ///     Gets a Boolean value indicating if the firewall service is currently running
        /// </summary>
        public static bool IsServiceRunning
        {
            get
            {
                switch (Version)
                {
                    case FirewallAPIVersion.FirewallLegacy:

                        return new ServiceController("SharedAccess").Status == ServiceControllerStatus.Running;
                    case FirewallAPIVersion.FirewallWAS:
                    case FirewallAPIVersion.FirewallWASWin7:
                    case FirewallAPIVersion.FirewallWASWin8:

                        return new ServiceController("MpsSvc").Status == ServiceControllerStatus.Running;
                }

                return false;
            }
        }

        /// <summary>
        ///     Returns the list of all registered third party firewalls
        /// </summary>
        public static IFirewallProductsCollection RegisteredProducts
        {
            get => new FirewallProductsCollection(GetProducts());
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
                        return FirewallAPIVersion.FirewallWASWin8;
                    }

                    if (FirewallWASRuleWin7.IsSupported)
                    {
                        return FirewallAPIVersion.FirewallWASWin7;
                    }

                    return FirewallAPIVersion.FirewallWAS;
                }

                if (FirewallLegacy.IsSupported)
                {
                    return FirewallAPIVersion.FirewallLegacy;
                }

                return FirewallAPIVersion.None;
            }
        }

        /// <summary>
        ///     Register an instance of a third party firewall management class
        /// </summary>
        public static FirewallProductRegistrationHandle RegisterProduct(FirewallProduct product)
        {
            return new FirewallProductRegistrationHandle(GetProducts().Register(product.GetCOMObject()));
        }

        private static INetFwProducts GetProducts()
        {
            if (!FirewallProduct.IsSupported)
            {
                throw new NotSupportedException();
            }

            return ComHelper.CreateInstance<INetFwProducts>();
        }
    }
}