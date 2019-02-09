using System;
using System.Collections.Generic;
using System.Linq;
using WindowsFirewallHelper.Addresses;
using WindowsFirewallHelper.COMInterop;
using WindowsFirewallHelper.Helpers;

namespace WindowsFirewallHelper.FirewallAPIv1.Rules
{
    /// <inheritdoc cref="IRule" />
    /// <summary>
    ///     Contains properties of a Windows Firewall v1 application rule
    /// </summary>
    public class ApplicationRule : IRule, IEquatable<ApplicationRule>
    {
        /// <summary>
        ///     Creates a new application rule for Windows Firewall v1
        /// </summary>
        /// <param name="name">Name of the rule</param>
        /// <param name="processAddress">Address of the executable file</param>
        /// <param name="profiles">The profile that this rule belongs to</param>
        public ApplicationRule(string name, string processAddress, FirewallProfiles profiles)
        {
            if (EnumHelper.HasFlag(profiles, FirewallProfiles.Public))
            {
                throw new NotSupportedException("Public profile is not supported.");
            }

            foreach (var profile in Enum.GetValues(typeof(FirewallProfiles)).OfType<FirewallProfiles>())
            {
                if (profiles.HasFlag(profile))
                {
                    UnderlyingObjects.Add(
                        profile,
                        (INetFwAuthorizedApplication) Activator.CreateInstance(
                            Type.GetTypeFromProgID(@"HNetCfg.FwAuthorizedApplication"))
                    );
                }
            }

            if (UnderlyingObjects.Count == 0)
            {
                throw new ArgumentException("At least one profile is required.", nameof(profiles));
            }

            Name = name;
            ExecutableAddress = processAddress;
            IsEnable = true;
            Scope = FirewallScope.All;
            IsEnable = true;
        }

        internal ApplicationRule(Dictionary<FirewallProfiles, INetFwAuthorizedApplication> authorizedApplications)
        {
            UnderlyingObjects = authorizedApplications;
        }

        /// <summary>
        ///     Gets or sets the address of the executable file that this rule is about
        /// </summary>
        public string ExecutableAddress
        {
            get => UnderlyingObjects.Values.First().ProcessImageFileName;
            set
            {
                foreach (var authorizedApplication in UnderlyingObjects.Values)
                {
                    authorizedApplication.ProcessImageFileName = value;
                }
            }
        }

        /// <summary>
        ///     Returns a Boolean value indicating if these class is available in the current machine
        /// </summary>
        public static bool IsSupported
        {
            get => Type.GetTypeFromProgID(@"HNetCfg.FwAuthorizedApplication") != null;
        }

        public FirewallProfiles Profiles
        {
            get
            {
                return UnderlyingObjects.Keys.ToArray()
                    .Aggregate(
                        (FirewallProfiles) 0,
                        (profiles, profile) => profiles & profile
                    );
            }
        }

        /// <summary>
        ///     Returns the underlying Windows Firewall Object
        /// </summary>
        private Dictionary<FirewallProfiles, INetFwAuthorizedApplication> UnderlyingObjects { get; }

        /// <summary>
        ///     Determines whether the specified<see cref="ApplicationRule" /> is equal to the current
        ///     <see cref="ApplicationRule" />.
        /// </summary>
        /// <param name="other"> The object to compare with the current object.</param>
        /// <returns>
        ///     true if the specified <see cref="ApplicationRule" /> is equal to the current<see cref="ApplicationRule" />;
        ///     otherwise, false.
        /// </returns>
        public bool Equals(ApplicationRule other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (UnderlyingObjects.Count != other.UnderlyingObjects.Count)
            {
                return false;
            }

            if (!UnderlyingObjects.Keys.SequenceEqual(other.UnderlyingObjects.Keys))
            {
                return false;
            }

            foreach (var profile in UnderlyingObjects.Keys)
            {
                var thisPort = UnderlyingObjects[profile];
                var otherPort = other.UnderlyingObjects[profile];

                if (!string.Equals(thisPort.ProcessImageFileName, otherPort.ProcessImageFileName))
                {
                    return false;
                }
            }

            return true;
        }

        public bool Equals(IRule other)
        {
            return Equals(other as ApplicationRule);
        }

        /// <inheritdoc />
        /// <exception cref="FirewallAPIv1NotSupportedException">Setting a value for this property is not supported</exception>
        FirewallAction IRule.Action
        {
            get => FirewallAction.Allow;
            set => throw new FirewallAPIv1NotSupportedException();
        }


        /// <inheritdoc />
        /// <exception cref="FirewallAPIv1NotSupportedException">Setting a value for this property is not supported</exception>
        FirewallDirection IRule.Direction
        {
            get => FirewallDirection.Inbound;
            set => throw new FirewallAPIv1NotSupportedException();
        }

        /// <inheritdoc />
        public string FriendlyName
        {
            get => NativeHelper.ResolveStringResource(Name);
        }


        /// <inheritdoc />
        public bool IsEnable
        {
            get => UnderlyingObjects.Values.All(port => port.Enabled);
            set
            {
                foreach (var authorizedApplication in UnderlyingObjects.Values)
                {
                    authorizedApplication.Enabled = value;
                }
            }
        }


        /// <inheritdoc />
        /// <exception cref="FirewallAPIv1NotSupportedException">Setting a value for this property is not supported</exception>
        IAddress[] IRule.LocalAddresses
        {
            get => new IAddress[] {SingleIP.Any};
            set => throw new FirewallAPIv1NotSupportedException();
        }


        /// <inheritdoc />
        /// <exception cref="FirewallAPIv1NotSupportedException">Setting a value for this property is not supported</exception>
        ushort[] IRule.LocalPorts
        {
            get => new ushort[0];
            set => throw new FirewallAPIv1NotSupportedException();
        }

        /// <inheritdoc />
        /// <exception cref="FirewallAPIv1NotSupportedException">Setting a value for this property is not supported</exception>
        FirewallPortType IRule.LocalPortType
        {
            get => FirewallPortType.All;
            set => throw new FirewallAPIv1NotSupportedException();
        }

        /// <inheritdoc />
        public string Name
        {
            get => UnderlyingObjects.Values.First().Name;
            set
            {
                foreach (var authorizedApplication in UnderlyingObjects.Values)
                {
                    authorizedApplication.Name = value;
                }
            }
        }

        /// <inheritdoc />
        /// <exception cref="FirewallAPIv1NotSupportedException">Changing the profile of a rule is not supported in FirewallAPIv1.</exception>
        FirewallProfiles IRule.Profiles
        {
            get => Profiles;
            set => throw new FirewallAPIv1NotSupportedException(
                "Changing the profile of a rule is not supported in FirewallAPIv1. Consider creating another rule."
            );
        }


        /// <inheritdoc />
        /// <exception cref="FirewallAPIv1NotSupportedException">Setting a value for this property is not supported</exception>
        FirewallProtocol IRule.Protocol
        {
            get => FirewallProtocol.FirewallV1_TCP_UDP;
            set => throw new FirewallAPIv1NotSupportedException();
        }


        /// <inheritdoc />
        public IAddress[] RemoteAddresses
        {
            get => UnderlyingObjects.Values
                .SelectMany(application => AddressHelper.StringToAddresses(application.RemoteAddresses))
                .Distinct()
                .ToArray();
            set
            {
                foreach (var authorizedApplication in UnderlyingObjects.Values)
                {
                    authorizedApplication.RemoteAddresses = AddressHelper.AddressesToString(value);
                }
            }
        }


        /// <inheritdoc />
        /// <exception cref="FirewallAPIv1NotSupportedException">Setting a value for this property is not supported</exception>
        ushort[] IRule.RemotePorts
        {
            get => new ushort[0];
            set => throw new FirewallAPIv1NotSupportedException();
        }


        /// <inheritdoc />
        public FirewallScope Scope
        {
            get => UnderlyingObjects.Values.First().Scope == NET_FW_SCOPE.NET_FW_SCOPE_LOCAL_SUBNET
                ? FirewallScope.LocalSubnet
                : FirewallScope.All;
            set
            {
                if (value == FirewallScope.Specific)
                {
                    throw new ArgumentException("Use the RemoteAddresses property to set the exact remote addresses");
                }

                if (value == FirewallScope.LocalSubnet)
                {
                    RemoteAddresses = new IAddress[] {new LocalSubnet()};

                    foreach (var authorizedApplication in UnderlyingObjects.Values)
                    {
                        authorizedApplication.Scope = NET_FW_SCOPE.NET_FW_SCOPE_LOCAL_SUBNET;
                    }
                }
                else if (value == FirewallScope.All)
                {
                    RemoteAddresses = new IAddress[] {SingleIP.Any};

                    foreach (var authorizedApplication in UnderlyingObjects.Values)
                    {
                        authorizedApplication.Scope = NET_FW_SCOPE.NET_FW_SCOPE_ALL;
                    }
                }
                else
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
        }


        /// <summary>
        ///     Compares two <see cref="ApplicationRule" /> objects for equality
        /// </summary>
        /// <param name="left">A <see cref="ApplicationRule" /> object</param>
        /// <param name="right">A <see cref="ApplicationRule" /> object</param>
        /// <returns>true if two sides are equal; otherwise false</returns>
        public static bool operator ==(ApplicationRule left, ApplicationRule right)
        {
            return Equals(left, right) || left?.Equals(right) == true;
        }

        /// <summary>
        ///     Compares two <see cref="ApplicationRule" /> objects for inequality
        /// </summary>
        /// <param name="left">A <see cref="ApplicationRule" /> object</param>
        /// <param name="right">A <see cref="ApplicationRule" /> object</param>
        /// <returns>true if two sides are not equal; otherwise false</returns>
        public static bool operator !=(ApplicationRule left, ApplicationRule right)
        {
            return !(left == right);
        }

        /// <inheritdoc />
        public override bool Equals(object other)
        {
            return Equals(other as IRule);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return UnderlyingObjects.Values.Aggregate(0, (hashCode, port) => hashCode + port.GetHashCode());
            }
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return FriendlyName;
        }

        // ReSharper disable once FlagArgument
        public INetFwAuthorizedApplication GetCOMObject(FirewallProfiles profile)
        {
            if (UnderlyingObjects.ContainsKey(profile))
            {
                return UnderlyingObjects[profile];
            }

            return null;
        }
    }
}