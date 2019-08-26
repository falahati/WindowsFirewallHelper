						              WindowsFirewallHelper
		    A class library to manage the Windows Firewall as well as 
        adding your program to the Windows Firewall Exception list

=========================================================================

USAGE
	Get an instance of the active firewall  using FirewallManager class and
  use the properties to get the list of firewall rules and profiles. 
  You can also  use the methods  on this class  to add a new rule  to the 
  firewall.

CODE SAMPLE FOR ADDING AN APPLICATION RULE TO THE ACTIVE PROFILE:
  var rule = FirewallManager.Instance.CreateApplicationRule(
      @"MyApp Rule",
      FirewallAction.Allow,
      @"C:\MyApp.exe"
  );
  rule.Direction = FirewallDirection.Outbound;
  FirewallManager.Instance.Rules.Add(rule);

CODE SAMPLE FOR ADDING A PORT RULE TO THE ACTIVE PROFILE:
  var rule = FirewallManager.Instance.CreatePortRule(
      @"Port 80 - Any Protocol",
      FirewallAction.Allow,
      80,
      FirewallProtocol.TCP
  );
  FirewallManager.Instance.Rules.Add(rule);

CODE SAMPLE TO GET A LIST OF ALL REGISTERED RULES:
  var allRules = FirewallManager.Instance.Rules.ToArray();

MORE SAMPLES:
  Check the Project's Github page at: 
  https://github.com/falahati/WindowsFirewallHelper