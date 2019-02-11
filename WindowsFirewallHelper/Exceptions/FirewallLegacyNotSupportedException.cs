using System;

namespace WindowsFirewallHelper.Exceptions
{
    // ReSharper disable once InconsistentNaming
    /// <inheritdoc />
    /// <summary>
    ///     The exception that is thrown when an invoked method is not supported with the Windows Firewall API v1
    /// </summary>
    public class FirewallLegacyNotSupportedException : NotSupportedException
    {
        /// <inheritdoc />
        /// <summary>
        ///     Creates a new instance of the FirewallAPIv1NotSupportedException class
        /// </summary>
        public FirewallLegacyNotSupportedException()
        {
        }

        /// <inheritdoc />
        /// <summary>
        ///     Creates a new instance of the FirewallAPIv1NotSupportedException class with a string as the message
        /// </summary>
        /// <param name="message">A <see cref="T:System.String" /> to be used as the message of the exception</param>
        public FirewallLegacyNotSupportedException(string message) : base(message)
        {
        }
    }
}