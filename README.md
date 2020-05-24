# Windows Firewall Helper Class Library
[![Financial Contributors on Open Collective](https://opencollective.com/WindowsFirewallHelper/all/badge.svg?label=financial+contributors)](https://opencollective.com/WindowsFirewallHelper) [![](https://img.shields.io/github/license/falahati/WindowsFirewallHelper.svg?style=flat-square)](https://github.com/falahati/WindowsFirewallHelper/blob/master/LICENSE)
[![](https://img.shields.io/github/commit-activity/y/falahati/WindowsFirewallHelper.svg?style=flat-square)](https://github.com/falahati/WindowsFirewallHelper/commits/master)
[![](https://img.shields.io/github/issues/falahati/WindowsFirewallHelper.svg?style=flat-square)](https://github.com/falahati/WindowsFirewallHelper/issues)

A class library to manage the Windows Firewall as well as adding your program to the Windows Firewall Exception list.

This project supports dotNet4.6 and NetStandard2, therefore, is compatible with NetCore2+ and any version of dotNet 
equal or greater than version 4.6.

Even though it is possible to reference this library under Linux or Mac; it's obviously not going to work.

**This readme file is for the version 2 of this library. Please check the `V1` branch for older readme file.**

## How to get
[![](https://img.shields.io/nuget/dt/WindowsFirewallHelper.svg?style=flat-square)](https://www.nuget.org/packages/WindowsFirewallHelper)
[![](https://img.shields.io/nuget/v/WindowsFirewallHelper.svg?style=flat-square)](https://www.nuget.org/packages/WindowsFirewallHelper)

This library is available as a NuGet package at [nuget.org](https://www.nuget.org/packages/WindowsFirewallHelper/).

## Help me fund my own Death Star

[![](https://img.shields.io/badge/crypto-CoinPayments-8a00a3.svg?style=flat-square)](https://www.coinpayments.net/index.php?cmd=_donate&reset=1&merchant=820707aded07845511b841f9c4c335cd&item_name=Donate&currency=USD&amountf=20.00000000&allow_amount=1&want_shipping=0&allow_extra=1)
[![](https://img.shields.io/badge/shetab-ZarinPal-8a00a3.svg?style=flat-square)](https://zarinp.al/@falahati)
[![](https://img.shields.io/badge/usd-Paypal-8a00a3.svg?style=flat-square)](https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=ramin.graphix@gmail.com&lc=US&item_name=Donate&no_note=0&cn=&curency_code=USD&bn=PP-DonationsBF:btn_donateCC_LG.gif:NonHosted)

**--OR--**

You can always donate your time by contributing to the project or by introducing it to others.

## How to use
The starting point of this library is the `FirewallManager` static class which can be used to get the instance 
of the class managing the firewall currently on the system.

If you are only targeting WinVista+ consider using the `FirewallWAS.Instance` static property to access the library's functionality. It allows for more flexibility and is easier to work with.

### `WindowsFirewallHelper.FirewallManager` Class
This static class contains properties about the currently active Windows Firewall management class instance or the 
registered third party firewall products. This class also provides methods to register a third-party firewall product.

#### `WindowsFirewallHelper.FirewallManager` Static Properties
*  `FirewallManager.Instance`: Gets an instance of the Windows Firewall management class.
*  `FirewallManager.Version`: Gets the type of firewall that `FirewallManager.Instance` property returns.
*  `FirewallManager.IsServiceRunning`: Gets a boolean value indicating if the Windows Firewall Service is currently running.
*  `FirewallManager.RegisteredProducts`: Gets an array containing all registered third party firewall products.

#### `WindowsFirewallHelper.FirewallManager` Static Methods
*  `FirewallManager.RegisterProduct()`: Registers a third-party firewall product returning a handle that will unregisters the product while getting disposed.

### `WindowsFirewallHelper` Namespace
This namespace contains shared and general classes as well as the main starting point of this library, `FirewallManager` class.

#### `WindowsFirewallHelper` Classes
*  `FirewallManager`: A static class to manage the current active firewall
*  `FirewallProtocol`: A class representing a Firewall Protocol
*  `FirewallLegacy`: Contains properties and methods of Windows Firewall v1 - Implementing the `IFirewall` interface
*  `FirewallLegacyProfile`: Contains properties of a Windows Firewall v1 profile - Implementing the `IFirewallProfile` interface
*  `FirewallWAS`: Contains properties and methods of Windows Firewall with Advanced Security (Vista+) - Implementing the `IFirewall` interface
*  `FirewallWASProfile`: Contains properties of a Windows Firewall with Advanced Security profile (Vista+) - Implementing the `IFirewallProfile` interface
*  `FirewallWASRuleGroup`: Contains properties and methods for managing a Windows Firewall with Advanced Security rule group (Vista+)
*  `FirewallWASInternetControlMessage`: Representing an Internet Control Message (ICM) type
*  `FirewallProduct`: Representing a third-party firewall product
*  `FirewallProductRegistrationHandle`: Representing a third-party firewall product registration handle that will automatically unregisters the product while getting disposed.

#### `WindowsFirewallHelper` Interfaces
*  `IFirewall`: Defines expected methods and properties of a firewall program or API
*  `IFirewallProfile`: Defines expected properties of a firewall profile
*  `IFirewallRule`: Defines expected properties of a firewall rule
*  `IAddress`: Defines expected methods of a network address

### `WindowsFirewallHelper.FirewallRules` Namespace
This namespace contains classes that can be used for direct manipulation of a firewall rule.

#### `WindowsFirewallHelper.FirewallRules` Classes
*  `FirewallLegacyApplicationRule`: Contains properties of a Windows Firewall v1 application rule - Implementing the `IFirewallRule` interface
*  `FirewallLegacyPortRule`: Contains properties of a Windows Firewall v1 port rule - Implementing the `IFirewallRule` interface
*  `FirewallWASRule`: Contains properties of a Windows Firewall with Advanced Security rule - Implementing the `IFirewallRule` interface
*  `FirewallWASRuleWin7`: Contains properties of a Windows Firewall with Advanced Security rule for Windows 7+ - Extending the `FirewallWASRule` class
*  `FirewallWASRuleWin8`: Contains properties of a Windows Firewall with Advanced Security rule for Windows 8+ - Extending the `FirewallWASRuleWin7` class

### `WindowsFirewallHelper.Exceptions` Namespace
This namespace contains exception classes that might be thrown when using this library

#### `WindowsFirewallHelper.Exceptions` Classes
*  `FirewallLegacyNotSupportedException`: The exception that is thrown when an invoked method or action is not supported with the 'Windows Firewall API v1' - Extending the `NotSupportedException` class
*  `FirewallWASNotSupportedException`: The exception that is thrown when an invoked method or action is not supported with the 'Windows Firewall with Advanced Security' - Extending the `NotSupportedException` class
*  `FirewallWASInvalidProtocolException`: The exception that is thrown when a passed FirewallProtocol is invalid for a 'Windows Firewall with Advanced Security' action or method - Extending the `InvalidOperationException`` class

### `WindowsFirewallHelper.Addresses` Namespace
This namespace contains the classes needed for manipulating or understanding a network address or a network service.

#### `WindowsFirewallHelper.Addresses` Classes
*  `SingleIP`: Represents a single network IP address - Implementing the `IAddress` interface
*  `IPRange`: Represents a range of network IP addresses - Implementing the `IAddress` interface
*  `NetworkAddress`: Represents a range of network IP addresses by subnet - Implementing the `IAddress` interface
*  `SpecialAddress`: An abstract class represents a special network address or network service - Implementing the `IAddress` interface
*  `DefaultGateway`: Represents the default network gateway - Extending the `SpecialAddress`` class
*  `LocalSubnet`: Represents thelocal network subnet - Extending the `SpecialAddress`` class
*  `DHCPService`: Represents the DHCP service - Extending the `SpecialAddress`` class
*  `DNSService`: Represents the DNS service - Extending the `SpecialAddress`` class
*  `WINSService`: Represents the WINS service - Extending the `SpecialAddress`` class

### `WindowsFirewallHelper.COMInterop` Namespace
This namespace contains the `interface`s and `enum`s that is used to access the underlying COM objects. Some of these types are public and can be used to directly modify a COM object. Usually firewall rules. Rest of types are internal to this library.

## Examples
Check the 'WindowsFirewallHelper.Sample' and 'WindowsFirewallHelper.NetCoreSample' projects as a brief example of what can be done using this class library.
![Screenshot](/screenshot.jpg?raw=true "Screenshot")

### Basic examples
*  Creating and registering a new application exception rule for outbound traffic on the currently active profile:
```C#
var rule = FirewallManager.Instance.CreateApplicationRule(
    @"MyApp Rule",
    FirewallAction.Allow,
    @"C:\MyApp.exe"
);
rule.Direction = FirewallDirection.Outbound;
FirewallManager.Instance.Rules.Add(rule);
```

*  Creating and registering a new port rule for inbound traffic on the currently active profile:
```C#
var rule = FirewallManager.Instance.CreatePortRule(
    @"Port 80 - Any Protocol",
    FirewallAction.Allow,
    80,
    FirewallProtocol.TCP
);
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

### Advanced examples
*  Creating a heavily customized application rule (Some parts of the following code are only applicable to Windows Vista, Windows 7 and above):
```C#
var rule = FirewallManager.Instance.CreatePortRule(
    @"Port 80 - Any Protocol",
    FirewallAction.Allow,
    80,
    FirewallProtocol.Any
);
if (rule is FirewallWASRule wasRule)
{
    wasRule.Interfaces = NetworkInterface.GetAllNetworkInterfaces()
        .Where(i => i.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
        .ToArray();
    wasRule.ICMPTypesAndCodes = new[]
    {
        new FirewallWASInternetControlMessage(InternetControlMessageKnownTypes.Echo),
        new FirewallWASInternetControlMessage(InternetControlMessageKnownTypes.EchoReply)
    };
    if (rule is FirewallWASRuleWin7 wasRuleWin7)
    {
        wasRuleWin7.EdgeTraversalOptions = EdgeTraversalAction.Deny;
    }
}
rule.Direction = FirewallDirection.Outbound;
FirewallManager.Instance.Rules.Add(rule);
```

*  Working directly with the desired firewall management class without using the `FirewallManager` to add a new port rule (Following example is limited to Windows 8 and above):
```C#
if (FirewallWAS.IsSupported && FirewallWASRuleWin8.IsSupported)
{
    var rule = new FirewallWASRuleWin8(
        "My Port Rule",
        1080,
        FirewallAction.Allow,
        FirewallDirection.Inbound,
        FirewallProfiles.Domain | FirewallProfiles.Private | FirewallProfiles.Public
    )
    {
        Description = "'My Port Rule' Allows Inbound traffic to my local Proxy Server from Wireless Adapters",
        NetworkInterfaceTypes = NetworkInterfaceTypes.Wireless,
        Protocol = FirewallProtocol.TCP
    };
    FirewallWAS.Instance.Rules.Add(rule);
}
```

## Contributors

### Code Contributors

This project exists thanks to all the people who contribute. [[Contribute](CONTRIBUTING.md)].
<a href="https://github.com/falahati/WindowsFirewallHelper/graphs/contributors"><img src="https://opencollective.com/WindowsFirewallHelper/contributors.svg?width=890&button=false" /></a>

### Financial Contributors

Become a financial contributor and help us sustain our community. [[Contribute](https://opencollective.com/WindowsFirewallHelper/contribute)]

#### Individuals

<a href="https://opencollective.com/WindowsFirewallHelper"><img src="https://opencollective.com/WindowsFirewallHelper/individuals.svg?width=890"></a>

#### Organizations

Support this project with your organization. Your logo will show up here with a link to your website. [[Contribute](https://opencollective.com/WindowsFirewallHelper/contribute)]

<a href="https://opencollective.com/WindowsFirewallHelper/organization/0/website"><img src="https://opencollective.com/WindowsFirewallHelper/organization/0/avatar.svg"></a>
<a href="https://opencollective.com/WindowsFirewallHelper/organization/1/website"><img src="https://opencollective.com/WindowsFirewallHelper/organization/1/avatar.svg"></a>
<a href="https://opencollective.com/WindowsFirewallHelper/organization/2/website"><img src="https://opencollective.com/WindowsFirewallHelper/organization/2/avatar.svg"></a>
<a href="https://opencollective.com/WindowsFirewallHelper/organization/3/website"><img src="https://opencollective.com/WindowsFirewallHelper/organization/3/avatar.svg"></a>
<a href="https://opencollective.com/WindowsFirewallHelper/organization/4/website"><img src="https://opencollective.com/WindowsFirewallHelper/organization/4/avatar.svg"></a>
<a href="https://opencollective.com/WindowsFirewallHelper/organization/5/website"><img src="https://opencollective.com/WindowsFirewallHelper/organization/5/avatar.svg"></a>
<a href="https://opencollective.com/WindowsFirewallHelper/organization/6/website"><img src="https://opencollective.com/WindowsFirewallHelper/organization/6/avatar.svg"></a>
<a href="https://opencollective.com/WindowsFirewallHelper/organization/7/website"><img src="https://opencollective.com/WindowsFirewallHelper/organization/7/avatar.svg"></a>
<a href="https://opencollective.com/WindowsFirewallHelper/organization/8/website"><img src="https://opencollective.com/WindowsFirewallHelper/organization/8/avatar.svg"></a>
<a href="https://opencollective.com/WindowsFirewallHelper/organization/9/website"><img src="https://opencollective.com/WindowsFirewallHelper/organization/9/avatar.svg"></a>

## License
The MIT License (MIT)

Copyright (c) 2016-2020 Soroush

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
