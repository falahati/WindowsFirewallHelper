using System;
using System.Linq;
using ConsoleUtilities;

namespace WindowsFirewallHelper.NetCoreSample
{
    internal class Program
    {
        private static void Main()
        {
            ConsoleWriter.Default.PrintMessage($"Firewall Service Running: {FirewallManager.IsServiceRunning}");
            ConsoleWriter.Default.PrintMessage($"Firewall Version: {FirewallManager.Version}");

            if (FirewallManager.Version == FirewallAPIVersion.None)
            {
                ConsoleWriter.Default.PrintMessage("Press any key to exit.");
                Console.ReadKey();

                return;
            }
            
            var firewallInstance = FirewallManager.Instance;

            ConsoleWriter.Default.PrintCaption(firewallInstance.Name);

            ConsoleNavigation.Default.PrintNavigation(new[]
            {
                new ConsoleNavigationItem("Profiles", (i, item) =>
                {
                    ConsoleNavigation.Default.PrintNavigation(firewallInstance.Profiles.ToArray(), (i1, profile) =>
                    {
                        ConsoleWriter.Default.WriteObject(profile);
                        ConsoleWriter.Default.PrintMessage("Press any key to get one step back.");
                        Console.ReadKey();
                    }, "Select a profile to view its settings.");
                }),
                new ConsoleNavigationItem("Rules", (i, item) =>
                {
                    ConsoleNavigation.Default.PrintNavigation(firewallInstance.Rules.OrderBy(rule => rule.FriendlyName).ToArray(), (i1, rule) =>
                    {
                        ConsoleWriter.Default.WriteObject(rule);
                        ConsoleWriter.Default.PrintMessage("Press any key to get one step back.");
                        Console.ReadKey();
                    }, "Select a rule to view its settings.");
                })
            }, "Select an execution path.");
        }
    }
}