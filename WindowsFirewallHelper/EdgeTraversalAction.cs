namespace WindowsFirewallHelper
{
    /// <summary>
    ///     Edge Traversal behavior
    /// </summary>
    public enum EdgeTraversalAction
    {
        /// <summary>
        ///     Fully block application, service, or port to globally addressable and accessible from outside a NAT edge device.
        /// </summary>
        Deny,

        /// <summary>
        ///     Fully allow application, service, or port to globally addressable and accessible from outside a NAT edge device.
        /// </summary>
        Allow,

        /// <summary>
        ///     Application makes the decision to allow unsolicited traffic from the Internet through a network address translation
        ///     (NAT) edge device
        /// </summary>
        DefferToApp,

        /// <summary>
        ///     User makes the decision to allow unsolicited traffic from the Internet through a network address translation (NAT)
        ///     edge device
        /// </summary>
        DefferToUser
    }
}