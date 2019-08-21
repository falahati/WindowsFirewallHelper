using System;
using WindowsFirewallHelper.COMInterop;
using WindowsFirewallHelper.InternalHelpers;

namespace WindowsFirewallHelper.FirewallRules
{
    /// <inheritdoc cref="FirewallWASRule" />
    /// <summary>
    ///     Contains properties of a Windows Firewall with Advanced Security rule in Windows 7 and above
    /// </summary>
    public class FirewallWASRuleWin7 : FirewallWASRule, IEquatable<FirewallWASRuleWin7>
    {
        /// <summary>
        ///     Creates a new application rule for Windows Firewall with Advanced Security
        /// </summary>
        /// <param name="name">Name of the rule</param>
        /// <param name="filename">Address of the executable file</param>
        /// <param name="action">Action that this rule defines</param>
        /// <param name="direction">Data direction in which this rule applies to</param>
        /// <param name="profiles">The profile that this rule belongs to</param>
        // ReSharper disable once TooManyDependencies
        public FirewallWASRuleWin7(
            string name,
            string filename,
            FirewallAction action,
            FirewallDirection direction,
            FirewallProfiles profiles) : base(name, filename, action, direction, profiles)
        {
        }

        /// <summary>
        ///     Creates a new port rule for Windows Firewall with Advanced Security
        /// </summary>
        /// <param name="name">Name of the rule</param>
        /// <param name="port">Port number of the rule</param>
        /// <param name="action">Action that this rule defines</param>
        /// <param name="direction">Data direction in which this rule applies to</param>
        /// <param name="profiles">The profile that this rule belongs to</param>
        // ReSharper disable once TooManyDependencies
        public FirewallWASRuleWin7(
            string name,
            ushort port,
            FirewallAction action,
            FirewallDirection direction,
            FirewallProfiles profiles) : base(name, port, action, direction, profiles)
        {
        }

        /// <summary>
        ///     Creates a new general rule for Windows Firewall with Advanced Security
        /// </summary>
        /// <param name="name">Name of the rule</param>
        /// <param name="action">Action that this rule defines</param>
        /// <param name="direction">Data direction in which this rule applies to</param>
        /// <param name="profiles">The profile that this rule belongs to</param>
        // ReSharper disable once TooManyDependencies
        public FirewallWASRuleWin7(
            string name,
            FirewallAction action,
            FirewallDirection direction,
            FirewallProfiles profiles) :
            base(name, action, direction, profiles)
        {
        }

        // ReSharper disable once SuggestBaseTypeForParameter
        internal FirewallWASRuleWin7(INetFwRule2 rule) : base(rule)
        {
        }

        /// <summary>
        ///     Gets or sets the behavior of this rule about the Edge Traversal
        /// </summary>
        public EdgeTraversalAction EdgeTraversalOptions
        {
            get
            {
                if (!Enum.IsDefined(typeof(EdgeTraversalAction), UnderlyingObject.EdgeTraversalOptions))
                {
                    throw new ArgumentOutOfRangeException();
                }

                return (EdgeTraversalAction) UnderlyingObject.EdgeTraversalOptions;
            }
            set
            {
                if (!Enum.IsDefined(typeof(EdgeTraversalAction), value))
                {
                    throw new ArgumentOutOfRangeException();
                }

                UnderlyingObject.EdgeTraversalOptions = (int) value;
            }
        }

        /// <summary>
        ///     Returns a Boolean value indicating if these class is available in the current machine
        /// </summary>
        public new static bool IsSupported
        {
            get => FirewallWASRule.IsSupported && ComHelper.IsSupported<INetFwRule2>();
        }

        /// <summary>
        ///     Returns the underlying Windows Firewall Object
        /// </summary>
        protected new INetFwRule2 UnderlyingObject
        {
            get => base.UnderlyingObject as INetFwRule2;
        }

        /// <inheritdoc />
        public bool Equals(FirewallWASRuleWin7 other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (!base.Equals(other))
            {
                return false;
            }

            return UnderlyingObject.EdgeTraversalOptions == other.UnderlyingObject.EdgeTraversalOptions;
        }

        /// <summary>
        ///     Compares two <see cref="FirewallWASRuleWin7" /> objects for equality
        /// </summary>
        /// <param name="left">A <see cref="FirewallWASRuleWin7" /> object</param>
        /// <param name="right">A <see cref="FirewallWASRuleWin7" /> object</param>
        /// <returns>true if two sides are equal; otherwise false</returns>
        public static bool operator ==(FirewallWASRuleWin7 left, FirewallWASRuleWin7 right)
        {
            return Equals(left, right) || left?.Equals(right) == true;
        }

        /// <summary>
        ///     Compares two <see cref="FirewallWASRuleWin7" /> objects for inequality
        /// </summary>
        /// <param name="left">A <see cref="FirewallWASRuleWin7" /> object</param>
        /// <param name="right">A <see cref="FirewallWASRuleWin7" /> object</param>
        /// <returns>true if two sides are not equal; otherwise false</returns>
        public static bool operator !=(FirewallWASRuleWin7 left, FirewallWASRuleWin7 right)
        {
            return !(left == right);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return Equals(obj as FirewallWASRuleWin7);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = base.GetHashCode();
                hashCode = hashCode * 467 + UnderlyingObject.EdgeTraversalOptions;

                return hashCode;
            }
        }

        /// <summary>
        ///     Returns the underlying COM object
        /// </summary>
        /// <returns>The underlying COM object</returns>
        public new INetFwRule2 GetCOMObject()
        {
            return UnderlyingObject;
        }
    }
}