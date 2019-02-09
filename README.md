# Windows Firewall Helper Class Library
[![](https://img.shields.io/github/license/falahati/WindowsFirewallHelper.svg?style=flat-square)](https://github.com/falahati/WindowsFirewallHelper/blob/master/LICENSE)
[![](https://img.shields.io/github/commit-activity/y/falahati/WindowsFirewallHelper.svg?style=flat-square)](https://github.com/falahati/WindowsFirewallHelper/commits/master)
[![](https://img.shields.io/github/issues/falahati/WindowsFirewallHelper.svg?style=flat-square)](https://github.com/falahati/WindowsFirewallHelper/issues)

A class library to manage the Windows Firewall as well as adding your program to the Windows Firewall Exception list.

This project supports dotNet4.6 and NetStandard2, therefore, is compatible with NetCore2+ and any version of dotNet equal or greater than version 4.6.

Even though it is possible to reference this library under Linux or Mac; it's obviously not going to work.

## WHERE TO FIND
[![](https://img.shields.io/nuget/dt/WindowsFirewallHelper.svg?style=flat-square)](https://www.nuget.org/packages/WindowsFirewallHelper)
[![](https://img.shields.io/nuget/v/WindowsFirewallHelper.svg?style=flat-square)](https://www.nuget.org/packages/WindowsFirewallHelper)

This library is available as a NuGet package at [nuget.org](https://www.nuget.org/packages/WindowsFirewallHelper/).

## Donation
### Buy me a coffee
Donations assist development and are greatly appreciated; also always remember that [every coffee counts!](https://media.makeameme.org/created/one-simply-does-i9k8kx.jpg) :)

[![](https://img.shields.io/badge/fiat-PayPal-8a00a3.svg?style=flat-square)](https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=WR3KK2B6TYYQ4&item_name=Donation&currency_code=USD&source=url)
[![](https://img.shields.io/badge/crypto-CoinPayments-8a00a3.svg?style=flat-square)](https://www.coinpayments.net/index.php?cmd=_donate&reset=1&merchant=820707aded07845511b841f9c4c335cd&item_name=Donate&currency=USD&amountf=20.00000000&allow_amount=1&want_shipping=0&allow_extra=1)
[![](https://img.shields.io/badge/shetab-ZarinPal-8a00a3.svg?style=flat-square)](https://zarinp.al/@falahati)

### Or, help me make it better
You can always donate your time by contributing to the project or by introducing it to others.

## HOW TO USE
Starting point of this library is the `FirewallManager` static class which can be used to get the instance of the class managing the firewall currently on the system.

### `WindowsFirewallHelper.FirewallManager` Class
This static class contains properties about the current active Windows Firewall management class instance or the registered active third party firewall. This class also provides methods to register a third party firewall management object.

#### Static Read Only Properties
*  `FirewallManager.Instance`: Gets the first active third party firewall object or the instance of the Windows Firewall management class.
*  `FirewallManager.Version`: Gets the type of firewall object that `FirewallManager.Instance` property returns.
*  `FirewallManager.ThirdPartyFirewalls`: Gets an array containing all registered third party firewall management objects.

#### Static Methods
*  `FirewallManager.RegisterFirewall()`: Registers an instance of a third party firewall management class implementing the `IFirewall` interface.


### `WindowsFirewallHelper` Namespace
This namespace contains shared and general classes as well as main starting point of this library, `FirewallManager` class.

#### `WindowsFirewallHelper` Classes
*  `WindowsFirewallHelper.FirewallManager`: A static class to manage the current active firewall
*  `WindowsFirewallHelper.FirewallProtocol`: A class representing a Firewall Protocol
*  `WindowsFirewallHelper.ActiveCollection`: Represents a dynamic data collection that provides notifications when items get added or removed.
*  `WindowsFirewallHelper.ActiveCollectionChangedEventArgs`: class containing event data about the `ActiveCollection.ItemsModified` event.

#### `WindowsFirewallHelper` Interfaces
*  `WindowsFirewallHelper.IFirewall`: Defines expected methods and properties of a firewall program or API
*  `WindowsFirewallHelper.IProfile`: Defines expected properties of a firewall profile
*  `WindowsFirewallHelper.IRule`: Defines expected properties of a firewall rule
*  `WindowsFirewallHelper.IAddress`: Defines expected methods of a network address

### `WindowsFirewallHelper.FirewallAPIv1` Namespace
This namespace contains the classes to manage and manipulate the Windows Firewall using the Windows Firewall API v1. Supporting Windows XP and above.

#### `WindowsFirewallHelper.FirewallAPIv1` Classes
*  `FirewallAPIv1.Firewall`: Contains properties and methods of Windows Firewall v1 - Implementing `IFirewall` interface
*  `FirewallAPIv1.FirewallProfile`: Contains properties of a Windows Firewall v1 profile - Implementing `IProfile` interface
*  `FirewallAPIv1.Rules.ApplicationRule`: Contains properties of a Windows Firewall v1 application rule - Implementing `IRule` interface
*  `FirewallAPIv1.Rules.PortRule`: Contains properties of a Windows Firewall v1 port rule - Implementing `IRule` interface
*  `FirewallAPIv1.FirewallAPIv1NotSupportedException`: The exception that is thrown when an invoked method is not supported with the 'Windows Firewall API v1' - Extending `NotSupportedException`

### `WindowsFirewallHelper.FirewallAPIv2` Namespace
This namespace contains the classes to manage and manipulate the Windows Firewall using the Windows Firewall API v2 (aka. Windows Firewall with Advanced Security). Supporting Windows Vista and above.

#### `WindowsFirewallHelper.FirewallAPIv2` Classes
*  `FirewallAPIv2.Firewall`: Contains properties and methods of Windows Firewall with Advanced Security - Implementing `IFirewall` interface
*  `FirewallAPIv2.FirewallProfile`: Contains properties of a Windows Firewall with Advanced Security profile - Implementing `IProfile` interface
*  `FirewallAPIv2.Rules.StandardRule`: Contains properties of a Windows Firewall with Advanced Security rule - Implementing `IRule` interface
*  `FirewallAPIv2.Rules.StandardRuleWin7`: Contains properties of a Windows Firewall with Advanced Security rule for Windows 7+ - Extending `FirewallAPIv2.Rules.StandardRule`
*  `FirewallAPIv2.Rules.StandardRuleWin8`: Contains properties of a Windows Firewall with Advanced Security rule for Windows 8+ - Extending `FirewallAPIv2.Rules.StandardRuleWin7`
*  `FirewallAPIv2.InternetControlMessage`: A class representing an Internet Control Message (ICM) type
*  `FirewallAPIv2.FirewallAPIv2NotSupportedException`: The exception that is thrown when an invoked method is not supported with the 'Windows Firewall with Advanced Security' - Extending `NotSupportedException`

## EXAMPLES
Check the 'WindowsFirewallHelper.Sample' project as a brief example of what can be done using this class library.
![Screenshot](/screenshot.jpg?raw=true "Screenshot")

### BASIC EXAMPLES
*  Creating and registering a new application exception rule for out-bound traffic on the currently enable profile:
```C#
  var rule = FirewallManager.Instance.CreateApplicationRule(
        FirewallManager.Instance.GetProfile().Type, @"MyApp Rule", 
        FirewallAction.Allow, @"C:\MyApp.exe");
  rule.Direction = FirewallDirection.Outbound;
  FirewallManager.Instance.Rules.Add(rule);
```

*  Creating and registering a new port rule for in-bound traffic on the currently enable profile:
```C#
  var rule = FirewallManager.Instance.CreatePortRule(
      FirewallManager.Instance.GetProfile().Type,
      @"Port 80 - Any Protocol", FirewallAction.Allow, 80, 
      FirewallProtocol.Any);
  FirewallManager.Instance.Rules.Add(rule);
```

*  Getting the list of all registered rules:
```C#
  var allRules = FirewallManager.Instance.Rules.ToArray();
```

*  Removing a rule by name:
```C#
  var myRule = FirewallManager.Instance.Rules.SingleOrDefault(r => r.Name == "My Rule");
  if (myRule != null)
  {
    FirewallManager.Instance.Rules.Remove(myRule);
  }
```

*  Disabling notifications for all firewall profiles:
```C#
  foreach (var profile in FirewallManager.Instance.Profiles)
  {
    profile.ShowNotifications = false;
  }
```

### ADVANCED EXAMPLES
*  Creating a heavily customized application rule (Some parts of the following code are only applicable to Windows Vista, Windows 7 and above):
```C#
  var rule = FirewallManager.Instance.CreatePortRule(
      FirewallManager.Instance.GetProfile().Type,
      @"Port 80 - Any Protocol", FirewallAction.Allow, 80, 
      FirewallProtocol.Any);
  if (rule is FirewallAPIv2.Rules.StandardRule)
  {
    ((FirewallAPIv2.Rules.StandardRule)rule).Interfaces =
        NetworkInterface.GetAllNetworkInterfaces()
            .Where(@interface => @interface.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
            .ToArray();
    ((FirewallAPIv2.Rules.StandardRule)rule).IcmpTypesAndCodes = new[]
    {
        new InternetControlMessage(InternetControlMessageKnownTypes.Echo),
        new InternetControlMessage(InternetControlMessageKnownTypes.EchoReply)
    };
    if (rule is FirewallAPIv2.Rules.StandardRuleWin7)
    {
        ((FirewallAPIv2.Rules.StandardRuleWin7)rule).EdgeTraversalOptions = EdgeTraversalAction.Deny;
    }
  }
  rule.Direction = FirewallDirection.Outbound;
  FirewallManager.Instance.Rules.Add(rule);
```

*  Working directly with the desired firewall management class without using the `FirewallManager` to add a new port rule (Following example is limited to Windows 8 and above):
```C#
  if (FirewallAPIv2.Firewall.Instance.IsSupported && StandardRuleWin8.IsSupported)
  {
    rule = new StandardRuleWin8("My Port Rule", 1080, FirewallAction.Allow,
        FirewallDirection.Inbound,
        FirewallProfiles.Domain | FirewallProfiles.Private | FirewallProfiles.Public)
    {
        Description =
            "'My Port Rule' Allows Inbound traffic to my local Proxy Server from Wireless Adapters",
        InterfaceTypes = FirewallInterfaceTypes.Wireless,
        Protocol = FirewallProtocol.TCP
    };
    FirewallAPIv2.Firewall.Instance.Rules.Add(rule);
  }
```

## LICENSE
The MIT License (MIT)

Copyright (c) 2016 Soroush

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
