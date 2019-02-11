using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;

namespace WindowsFirewallHelper.InternalHelpers
{
    // ReSharper disable once HollowTypeName
    internal static class NetworkInterfaceHelper
    {
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


        // ReSharper disable once FlagArgument
        public static string InterfaceTypesToString(NetworkInterfaceTypes types)
        {
            var interfaceTypes = new List<string>();

            if (types.HasFlag(NetworkInterfaceTypes.Lan) &&
                types.HasFlag(NetworkInterfaceTypes.Wireless) &&
                types.HasFlag(NetworkInterfaceTypes.RemoteAccess))
            {
                return "All";
            }

            if (types.HasFlag(NetworkInterfaceTypes.Lan))
            {
                interfaceTypes.Add("Lan");
            }

            if (types.HasFlag(NetworkInterfaceTypes.Wireless))
            {
                interfaceTypes.Add("Wireless");
            }

            if (types.HasFlag(NetworkInterfaceTypes.RemoteAccess))
            {
                interfaceTypes.Add("RemoteAccess");
            }

            return string.Join(",", interfaceTypes.ToArray());
        }

        public static NetworkInterface[] StringToInterfaces(string[] str)
        {
            if (str == null)
            {
                return new NetworkInterface[0];
            }

            var interfaces = new List<NetworkInterface>();
            var availableInterfaces = NetworkInterface.GetAllNetworkInterfaces();

            foreach (var interfaceName in str)
            foreach (var @interface in availableInterfaces)
            {
                if (string.Equals(@interface.Name.Trim(), interfaceName.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    interfaces.Add(@interface);
                }
            }

            return interfaces.ToArray();
        }

        // ReSharper disable once ExcessiveIndentation
        public static NetworkInterfaceTypes StringToInterfaceTypes(string str)
        {
            if (string.IsNullOrEmpty(str?.Trim()))
            {
                return NetworkInterfaceTypes.Lan |
                       NetworkInterfaceTypes.RemoteAccess |
                       NetworkInterfaceTypes.Wireless;
            }

            NetworkInterfaceTypes value = 0;

            foreach (var interfaceType in str.Split(','))
            {
                if (string.Equals(interfaceType.Trim(), @"All", StringComparison.OrdinalIgnoreCase))
                {
                    return NetworkInterfaceTypes.Lan |
                           NetworkInterfaceTypes.RemoteAccess |
                           NetworkInterfaceTypes.Wireless;
                }

                if (string.Equals(interfaceType.Trim(), @"RemoteAccess", StringComparison.OrdinalIgnoreCase))
                {
                    value |= NetworkInterfaceTypes.RemoteAccess;
                }
                else if (string.Equals(interfaceType.Trim(), @"Wireless", StringComparison.OrdinalIgnoreCase))
                {
                    value |= NetworkInterfaceTypes.Wireless;
                }
                else if (string.Equals(interfaceType.Trim(), @"Lan", StringComparison.OrdinalIgnoreCase))
                {
                    value |= NetworkInterfaceTypes.Lan;
                }
            }

            if (value == 0)
            {
                return NetworkInterfaceTypes.Lan |
                       NetworkInterfaceTypes.RemoteAccess |
                       NetworkInterfaceTypes.Wireless;
            }

            return value;
        }
    }
}