namespace WindowsFirewallHelper
{
    /// <summary>
    ///     Firewall port types
    /// </summary>
    public enum FirewallPortType
    {
        /// <summary>
        ///     All local ports
        /// </summary>
        All,

        /// <summary>
        ///     Specific list of local port numbers
        /// </summary>
        Specific,

        /// <summary>
        ///     TCP Dynamic RPC port range
        /// </summary>
        RPCDynamicPorts,

        /// <summary>
        ///     TCP RPC endpoint mapper port range
        /// </summary>
        RPCEndpointMapper,

        /// <summary>
        ///     TCP IP-HTTPS port (443)
        /// </summary>
        IPHTTPS,

        /// <summary>
        ///     UDP Teredo traversed packets
        /// </summary>
        EdgeTraversal,

        /// <summary>
        ///     UDP Play To Discovery packets
        /// </summary>
        PlayToDiscovery
    }
}