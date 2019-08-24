using System;

namespace WindowsFirewallHelper.Exceptions
{
    /// <summary>
    ///     The exception that is thrown when an invoked method or operation is not supported with the Windows Firewall With
    ///     Advanced Security API
    /// </summary>
    public class FirewallWASNotSupportedException : NotSupportedException
    {
        /// <summary>
        ///     Creates a new instance of the <see cref="FirewallWASNotSupportedException" /> class
        /// </summary>
        public FirewallWASNotSupportedException() : this(
            "Specific operation is not supported when working with Windows Firewall With Advanced Security.")
        {
        }

        /// <summary>
        ///     Creates a new instance of the <see cref="FirewallWASNotSupportedException" /> class with a string as the message
        /// </summary>
        /// <param name="message">A <see cref="T:System.String" /> to be used as the message of the exception</param>
        public FirewallWASNotSupportedException(string message) : base(message)
        {
        }
    }
}