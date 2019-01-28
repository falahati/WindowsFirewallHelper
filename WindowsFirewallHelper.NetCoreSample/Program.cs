using System;
using System.Linq;
using ConsoleUtilities;

namespace WindowsFirewallHelper.NetCoreSample
{
    internal class Program
    {
        private static void Main()
        {
            var firewallInstance = FirewallManager.Instance;

            ConsoleWriter.Default.PrintMessage($"Firewall Version: {FirewallManager.Version}");
            ConsoleWriter.Default.PrintMessage($"Firewall Name: {firewallInstance?.Name ?? "[NULL]"}");
            ConsoleWriter.Default.PrintMessage(
                $"Firewall Status: {(firewallInstance?.IsSupported == true ? "Supported" : "NotSupported")}"
            );

            if (firewallInstance?.IsSupported != true)
            {
                ConsoleWriter.Default.PrintMessage("Press any key to exit.");
                Console.ReadKey();

                return;
            }

            ConsoleWriter.Default.PrintCaption(firewallInstance.Name);

            ConsoleNavigation.Default.PrintNavigation(new[]
            {
                new ConsoleNavigationItem("Profiles", (i, item) =>
                {
                    ConsoleNavigation.Default.PrintNavigation(firewallInstance.Profiles, (i1, profile) =>
                    {
                        ConsoleWriter.Default.WriteObject(profile);
                        ConsoleWriter.Default.PrintMessage("Press any key to get one step back.");
                        Console.ReadKey();
                    }, "Select a profile to view its settings.");
                }),
                new ConsoleNavigationItem("Rules", (i, item) =>
                {
                    ConsoleNavigation.Default.PrintNavigation(firewallInstance.Rules.ToArray(), (i1, rule) =>
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