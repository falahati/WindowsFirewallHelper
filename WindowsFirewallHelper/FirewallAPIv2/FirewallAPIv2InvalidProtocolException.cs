using System;

namespace WindowsFirewallHelper.FirewallAPIv2
{
    /// <inheritdoc />
    // ReSharper disable once InconsistentNaming
    /// <summary>
    ///     The exception that is thrown when an operation is not supported with the selected Protocol in the Windows Firewall
    ///     API v2
    /// </summary>
    public class FirewallAPIv2InvalidProtocolException : InvalidOperationException
    {
        /// <inheritdoc />
        /// <summary>
        ///     Creates a new instance of the FirewallAPIv2InvalidProtocolException exception
        /// </summary>
        public FirewallAPIv2InvalidProtocolException(string message) : base(message)
        {
        }
    }
}