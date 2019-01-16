using System;
using System.Collections.Generic;
using System.Net;
using WindowsFirewallHelper.Addresses;

namespace WindowsFirewallHelper.Helpers
{
    internal static class AddressHelper
    {
        public static string AddressesToString(IAddress[] rules)
        {
            var addresses = new List<string>();
            foreach (var address in rules)
            {
                if (SingleIP.Any.Equals(address))
                {
                    addresses.Clear();
                    addresses.Add(address.ToString());
                    break;
                }
                addresses.Add(address.ToString());
            }
            return string.Join(",", addresses.ToArray());
        }

        public static IPAddress Max(IPAddress val1, IPAddress val2)
        {
            if (val1.AddressFamily != val2.AddressFamily)
                throw new ArgumentException("Addresses of different family can not be compared.");
            var bytes1 = val1.GetAddressBytes();
            var bytes2 = val2.GetAddressBytes();
            for (var i = 0; i < bytes1.Length; i++)
            {
                if (bytes1[i] > bytes2[i])
                    return val1;
                if (bytes2[i] > bytes1[i])
                    return val2;
            }
            return val1;
        }

        public static IPAddress Min(IPAddress val1, IPAddress val2)
        {
            if (val1.AddressFamily != val2.AddressFamily)
                throw new ArgumentException("Addresses of different family can not be compared.");
            var bytes1 = val1.GetAddressBytes();
            var bytes2 = val2.GetAddressBytes();
            for (var i = 0; i < bytes1.Length; i++)
            {
                if (bytes1[i] < bytes2[i])
                    return val1;
                if (bytes2[i] < bytes1[i])
                    return val2;
            }
            return val1;
        }


        public static string PortsToString(ushort[] ports)
        {
            var strList = new string[ports.Length];
            for (var i = 0; i < ports.Length; i++)
                strList[i] = ports[i].ToString();
            return strList.Length == 0 ? null : string.Join(",", strList);
        }

        public static IAddress[] StringToAddresses(string str)
        {
            var remoteAddresses = new List<IAddress>();
            foreach (var remoteAddress in str.Split(','))
            {
                DNSService dns;
                DHCPService dhcp;
                WINSService wins;
                LocalSubnet localSubnet;
                DefaultGateway defaultGateway;
                IPRange range;
                SingleIP ip;
                NetworkAddress network;
                if (DNSService.TryParse(remoteAddress, out dns))
                    remoteAddresses.Add(dns);
                else if (DHCPService.TryParse(remoteAddress, out dhcp))
                    remoteAddresses.Add(dhcp);
                else if (WINSService.TryParse(remoteAddress, out wins))
                    remoteAddresses.Add(wins);
                else if (LocalSubnet.TryParse(remoteAddress, out localSubnet))
                    remoteAddresses.Add(localSubnet);
                else if (DefaultGateway.TryParse(remoteAddress, out defaultGateway))
                    remoteAddresses.Add(defaultGateway);
                else if (IPRange.TryParse(remoteAddress, out range))
                    remoteAddresses.Add(range);
                else if (SingleIP.TryParse(remoteAddress, out ip))
                    remoteAddresses.Add(ip);
                else if (NetworkAddress.TryParse(remoteAddress, out network))
                    remoteAddresses.Add(network);
            }
            return remoteAddresses.ToArray();
        }

        public static ushort[] StringToPorts(string str)
        {
            if (string.IsNullOrEmpty(str?.Trim()))
                return new ushort[0];
            var ports = new List<ushort>();
            foreach (var port in str.Split(','))
            {
                if (port.Contains('-'))
                {
                    var portParts = port.Split('-');
                    ushort s, e;
                    if (ushort.TryParse(portParts[0], out s) && ushort.TryParse(portParts[1], out e))
                    {
                        ports.AddRange(Enumerable.Range(s, e).Select(p => (ushort)p));
                    }
                }
                else
                {
                    ushort p;
                    if (ushort.TryParse(port, out p))
                        ports.Add(p);
                }
            }
            return ports.ToArray();
        }
    }
}
