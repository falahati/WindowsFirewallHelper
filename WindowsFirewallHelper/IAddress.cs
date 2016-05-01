namespace WindowsFirewallHelper
{
    /// <summary>
    ///     Defines expected methods of a network address
    /// </summary>
    public interface IAddress
    {
        /// <summary>
        ///     Converts the value of the <see cref="IAddress" /> to a human and machine readable format
        /// </summary>
        /// <returns>String value describing the value of the object</returns>
        string ToString();
    }
}