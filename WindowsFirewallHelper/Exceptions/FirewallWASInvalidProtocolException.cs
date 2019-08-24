using System;

namespace WindowsFirewallHelper.Exceptions
{
    /// <summary>
    ///     The exception that is thrown when an operation is not supported with the selected <see cref="FirewallProtocol" />
    ///     in the Windows Firewall With Advanced Security API
    /// </summary>
    public class FirewallWASInvalidProtocolException : InvalidOperationException
    {
        /// <summary>
        ///     Creates a new instance of the FirewallAPIv2InvalidProtocolException exception
        /// </summary>
        public FirewallWASInvalidProtocolException(string message) : base(message)
        {
        }
    }
}