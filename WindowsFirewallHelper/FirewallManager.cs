using System;
using System.ServiceProcess;
using WindowsFirewallHelper.Collections;
using WindowsFirewallHelper.COMInterop;
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
        /// <exception cref="NotSupportedException">Thrown if firewall API version is not supported.</exception>
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
        ///     Attempts to get an instance of the active firewall.
        /// </summary>
        /// <param name="instance">Outputs the active firewall instance, if successful.</param>
        /// <returns>Returns true if successful, otherwise false.</returns>
        public static bool TryGetInstance(out IFirewall instance)
        {
            try
            {
                instance = Instance;
                return true;
            }
            catch
            {
                instance = null;
                return false;
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
        /// <exception cref="NotSupportedException">Thrown if third party firewalls are not supported.</exception>
        public static IFirewallProductsCollection RegisteredProducts
        {
            get => GetRegisteredProducts(new COMTypeResolver());
        }

        /// <summary>
        ///     Attempts to get the list of all registered third party firewalls.
        /// </summary>
        /// <param name="collection">Outputs the collection of third party firewalls, if successful.</param>
        /// <returns>Returns true if successful, otherwise false.</returns>
        public static bool TryGetRegisteredProducts(out IFirewallProductsCollection collection)
        {
            try
            {
                collection = RegisteredProducts;
                return true;
            }
            catch
            {
                collection = null;
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
                if (FirewallWAS.IsLocallySupported)
                {
                    if (FirewallWASRuleWin8.IsLocallySupported)
                    {
                        return FirewallAPIVersion.FirewallWASWin8;
                    }

                    if (FirewallWASRuleWin7.IsLocallySupported)
                    {
                        return FirewallAPIVersion.FirewallWASWin7;
                    }

                    return FirewallAPIVersion.FirewallWAS;
                }

                if (FirewallLegacy.IsLocallySupported)
                {
                    return FirewallAPIVersion.FirewallLegacy;
                }

                return FirewallAPIVersion.None;
            }
        }

        /// <summary>
        ///     Register an instance of a third party firewall management class locally
        /// </summary>
        public static FirewallProductRegistrationHandle RegisterProduct(FirewallProduct product, COMTypeResolver typeResolver)
        {
            return new FirewallProductRegistrationHandle(GetProducts(typeResolver).Register(product.GetCOMObject()));
        }

        /// <summary>
        ///     Register an instance of a third party firewall management class remotely
        /// </summary>
        public static FirewallProductRegistrationHandle RegisterProduct(FirewallProduct product)
        {
            return RegisterProduct(product, new COMTypeResolver());
        }

        /// <summary>
        ///     Returns the list of all registered third party firewalls remotely
        /// </summary>
        /// <exception cref="NotSupportedException">Thrown if third party firewalls are not supported.</exception>
        public static IFirewallProductsCollection GetRegisteredProducts(COMTypeResolver typeResolver)
        {
           return new FirewallProductsCollection(GetProducts(typeResolver));
        }

        private static INetFwProducts GetProducts(COMTypeResolver typeResolver)
        {
            if (!FirewallProduct.IsSupported(typeResolver))
            {
                throw new NotSupportedException();
            }

            return typeResolver.CreateInstance<INetFwProducts>();
        }
    }
}