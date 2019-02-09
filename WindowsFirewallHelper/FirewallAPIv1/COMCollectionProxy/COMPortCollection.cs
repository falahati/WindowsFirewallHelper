using System;
using System.Runtime.InteropServices.ComTypes;
using WindowsFirewallHelper.COMInterop;
using WindowsFirewallHelper.Helpers;

namespace WindowsFirewallHelper.FirewallAPIv1.COMCollectionProxy
{
    internal class COMPortCollection :
        COMCollection<INetFwOpenPorts, INetFwOpenPort, COMPortCollectionKey, Tuple<FirewallProfiles, INetFwOpenPort>>
    {
        private static readonly Random Random = new Random();
        private readonly FirewallProfiles _profile;

        public COMPortCollection(INetFwOpenPorts authorizedPortsCollection, FirewallProfiles profile) :
            base(authorizedPortsCollection)
        {
            _profile = profile;
        }

        /// <inheritdoc />
        public override bool IsReadOnly { get; } = false;

        /// <inheritdoc />
        public override bool Remove(Tuple<FirewallProfiles, INetFwOpenPort> item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var oldPortNumber = item.Item2.Port;

            try
            {
                item.Item2.Port = (ushort) Random.Next(10240, ushort.MaxValue);
                base.Remove(item);

                return true;
            }
            finally
            {
                item.Item2.Port = oldPortNumber;
            }
        }

        /// <inheritdoc />
        protected override INetFwOpenPort ConvertManagedToNative(Tuple<FirewallProfiles, INetFwOpenPort> managed)
        {
            return managed.Item2;
        }

        /// <inheritdoc />
        protected override Tuple<FirewallProfiles, INetFwOpenPort> ConvertNativeToManaged(INetFwOpenPort native)
        {
            return new Tuple<FirewallProfiles, INetFwOpenPort>(_profile, native);
        }

        /// <inheritdoc />
        protected override COMPortCollectionKey GetCollectionKey(Tuple<FirewallProfiles, INetFwOpenPort> managed)
        {
            return new COMPortCollectionKey(managed.Item2.Port, managed.Item2.Protocol);
        }

        /// <inheritdoc />
        protected override IEnumVARIANT GetEnumVariant(INetFwOpenPorts sourceCollection)
        {
            return sourceCollection.GetEnumeratorVariant();
        }
    }
}