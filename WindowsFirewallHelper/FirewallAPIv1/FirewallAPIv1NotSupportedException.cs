using System;

namespace WindowsFirewallHelper.FirewallAPIv1
{
    // ReSharper disable once InconsistentNaming
    /// <inheritdoc />
    /// <summary>
    ///     The exception that is thrown when an invoked method is not supported with the Windows Firewall API v1
    /// </summary>
    public class FirewallAPIv1NotSupportedException : NotSupportedException
    {
        /// <inheritdoc />
        /// <summary>
        ///     Creates a new instance of the FirewallAPIv1NotSupportedException class
        /// </summary>
        public FirewallAPIv1NotSupportedException()
        {
        }

        /// <inheritdoc />
        /// <summary>
        ///     Creates a new instance of the FirewallAPIv1NotSupportedException class with a string as the message
        /// </summary>
        /// <param name="message">A <see cref="T:System.String" /> to be used as the message of the exception</param>
        public FirewallAPIv1NotSupportedException(string message) : base(message)
        {
        }
    }
}