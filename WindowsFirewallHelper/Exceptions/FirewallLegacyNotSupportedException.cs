using System;

namespace WindowsFirewallHelper.Exceptions
{
    /// <summary>
    ///     The exception that is thrown when an invoked method or operation is not supported with the Windows Firewall Legacy
    ///     API
    /// </summary>
    public class FirewallLegacyNotSupportedException : NotSupportedException
    {
        /// <summary>
        ///     Creates a new instance of the <see cref="FirewallLegacyNotSupportedException" /> class
        /// </summary>
        public FirewallLegacyNotSupportedException() : this(
            "Specific operation is not supported when working with Windows Firewall Legacy.")
        {
        }

        /// <summary>
        ///     Creates a new instance of the <see cref="FirewallLegacyNotSupportedException" /> class with a string as the message
        /// </summary>
        /// <param name="message">A <see cref="T:System.String" /> to be used as the message of the exception</param>
        public FirewallLegacyNotSupportedException(string message) : base(message)
        {
        }
    }
}