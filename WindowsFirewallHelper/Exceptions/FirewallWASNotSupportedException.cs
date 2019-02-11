using System;

namespace WindowsFirewallHelper.Exceptions
{
    /// <inheritdoc />
    // ReSharper disable once InconsistentNaming
    /// <summary>
    ///     The exception that is thrown when an invoked method is not supported with the Windows Firewall API v2
    /// </summary>
    public class FirewallWASNotSupportedException : NotSupportedException
    {
    }
}