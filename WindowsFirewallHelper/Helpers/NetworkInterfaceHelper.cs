using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using WindowsFirewallHelper.FirewallAPIv2;

namespace WindowsFirewallHelper.Helpers
{
    internal static class NetworkInterfaceHelper
    {
        public static NetworkInterface[] StringToInterfaces(string[] str)
        {
            if (str == null)
            {
                return new NetworkInterface[0];
            }
            var interfaces = new List<NetworkInterface>();
            var availableInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (var interfaceName in str)
            {
                foreach (var @interface in availableInterfaces)
                {
                    if (string.Equals(@interface.Name.Trim(), interfaceName.Trim(), StringComparison.OrdinalIgnoreCase))
                    {
                        interfaces.Add(@interface);
                    }
                }
            }
            return interfaces.ToArray();
        }


        public static string[] InterfacesToString(NetworkInterface[] interfaces)
        {
            if (interfaces.Length == 0)
            {
                return null;
            }
            var interfacesString = new List<string>();
            foreach (var @interface in interfaces)
            {
                interfacesString.Add(@interface.Name);
            }
            return interfacesString.ToArray();
        }

        public static FirewallInterfaceTypes StringToInterfaceTypes(string str)
        {
            if (string.IsNullOrEmpty(str?.Trim()))
            {
                return FirewallInterfaceTypes.Lan | FirewallInterfaceTypes.RemoteAccess |
                       FirewallInterfaceTypes.Wireless;
            }
            FirewallInterfaceTypes value = 0;
            foreach (var interfaceType in str.Split(','))
            {
                if (string.Equals(interfaceType.Trim(), @"All", StringComparison.OrdinalIgnoreCase))
                {
                    return FirewallInterfaceTypes.Lan | FirewallInterfaceTypes.RemoteAccess |
                           FirewallInterfaceTypes.Wireless;
                }
                if (string.Equals(interfaceType.Trim(), @"RemoteAccess", StringComparison.OrdinalIgnoreCase))
                {
                    value |= FirewallInterfaceTypes.RemoteAccess;
                }
                else if (string.Equals(interfaceType.Trim(), @"Wireless", StringComparison.OrdinalIgnoreCase))
                {
                    value |= FirewallInterfaceTypes.Wireless;
                }
                else if (string.Equals(interfaceType.Trim(), @"Lan", StringComparison.OrdinalIgnoreCase))
                {
                    value |= FirewallInterfaceTypes.Lan;
                }
            }
            if (value == 0)
            {
                return FirewallInterfaceTypes.Lan | FirewallInterfaceTypes.RemoteAccess |
                       FirewallInterfaceTypes.Wireless;
            }
            return value;
        }


        public static string InterfaceTypesToString(FirewallInterfaceTypes types)
        {
            var interfaceTypes = new List<string>();
            if (EnumHelper.HasFlag(types, FirewallInterfaceTypes.Lan) &&
                EnumHelper.HasFlag(types, FirewallInterfaceTypes.Wireless) &&
                EnumHelper.HasFlag(types, FirewallInterfaceTypes.RemoteAccess))
            {
                return "All";
            }
            if (EnumHelper.HasFlag(types, FirewallInterfaceTypes.Lan))
            {
                interfaceTypes.Add("Lan");
            }
            if (EnumHelper.HasFlag(types, FirewallInterfaceTypes.Wireless))
            {
                interfaceTypes.Add("Wireless");
            }
            if (EnumHelper.HasFlag(types, FirewallInterfaceTypes.RemoteAccess))
            {
                interfaceTypes.Add("RemoteAccess");
            }
            return string.Join(",", interfaceTypes.ToArray());
        }
    }
}