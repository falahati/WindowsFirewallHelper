using System;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using WindowsFirewallHelper.COMInterop;
using WindowsFirewallHelper.Helpers;

namespace WindowsFirewallHelper.FirewallAPIv1.COMCollectionProxy
{
    internal class COMApplicationCollection :
        COMCollection<INetFwAuthorizedApplications, INetFwAuthorizedApplication, COMApplicationCollectionKey,
            Tuple<FirewallProfiles, INetFwAuthorizedApplication>>
    {
        private readonly FirewallProfiles _profile;

        public COMApplicationCollection(
            INetFwAuthorizedApplications authorizedApplicationsCollection,
            FirewallProfiles profile) :
            base(authorizedApplicationsCollection)
        {
            _profile = profile;
        }

        /// <inheritdoc />
        public override bool IsReadOnly { get; } = false;

        /// <inheritdoc />
        public override bool Remove(Tuple<FirewallProfiles, INetFwAuthorizedApplication> item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var oldFilename = item.Item2.ProcessImageFileName;
            var tempFilename = Path.GetTempFileName();

            try
            {
                item.Item2.ProcessImageFileName = tempFilename;
                base.Remove(item);

                return true;
            }
            finally
            {
                item.Item2.ProcessImageFileName = oldFilename;

                if (File.Exists(tempFilename))
                {
                    File.Delete(tempFilename);
                }
            }
        }

        /// <inheritdoc />
        protected override INetFwAuthorizedApplication ConvertManagedToNative(
            Tuple<FirewallProfiles, INetFwAuthorizedApplication> managed)
        {
            return managed.Item2;
        }

        /// <inheritdoc />
        protected override Tuple<FirewallProfiles, INetFwAuthorizedApplication> ConvertNativeToManaged(
            INetFwAuthorizedApplication native)
        {
            return new Tuple<FirewallProfiles, INetFwAuthorizedApplication>(_profile, native);
        }

        /// <inheritdoc />
        protected override COMApplicationCollectionKey GetCollectionKey(
            Tuple<FirewallProfiles, INetFwAuthorizedApplication> managed)
        {
            return new COMApplicationCollectionKey(managed.Item2.ProcessImageFileName);
        }

        /// <inheritdoc />
        protected override IEnumVARIANT GetEnumVariant(INetFwAuthorizedApplications sourceCollection)
        {
            return sourceCollection.GetEnumeratorVariant();
        }
    }
}