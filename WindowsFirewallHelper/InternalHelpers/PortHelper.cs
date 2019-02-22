using System.Linq;

namespace WindowsFirewallHelper.InternalHelpers
{
    // ReSharper disable once HollowTypeName
    internal static class PortHelper
    {
        // ReSharper disable once TooManyDeclarations

        public static string PortsToString(ushort[] ports)
        {
            var portStrings = ports
                .Distinct()
                .OrderBy(port => port)
                .Select((port, index) => new {PortNumber = port, GroupId = port - index})
                .GroupBy(pair => pair.GroupId)
                .Select(
                    groups => groups.Count() >= 3
                        ? groups.First().PortNumber + "-" + groups.Last().PortNumber
                        : string.Join(",", groups.Select(pair => pair.PortNumber.ToString("")).ToArray())
                )
                .ToArray();

            return string.Join(",", portStrings);
        }

        // ReSharper disable once TooManyDeclarations

        public static ushort[] StringToPorts(string str)
        {
            if (string.IsNullOrEmpty(str?.Trim()))
            {
                return new ushort[0];
            }

            return str.Trim().Split(',')
                .SelectMany(port =>
                    {
                        var portParts = port.Trim().Split('-');

                        if (portParts.Length == 2 &&
                            ushort.TryParse(portParts[0].Trim(), out var start) &&
                            ushort.TryParse(portParts[1].Trim(), out var end))
                        {
                            return Enumerable.Range(start, end - start + 1).Select(p => (ushort) p);
                        }

                        if (portParts.Length == 1 && ushort.TryParse(port.Trim(), out var portNumber))
                        {
                            return new[] {portNumber};
                        }

                        return new ushort[0];
                    }
                )
                .Distinct()
                .OrderBy(port => port)
                .ToArray();
        }
    }
}