using System;
using WindowsFirewallHelper.Helpers;

namespace WindowsFirewallHelper.FirewallAPIv1.COMCollectionProxy
{
    internal class COMApplicationCollectionKey : COMCollectionKey
    {
        /// <inheritdoc />
        public COMApplicationCollectionKey(string fileName)
        {
            FileName = fileName;
        }

        public string FileName { get; }


        /// <inheritdoc />
        public override Type[] Types { get; } = {typeof(string)};

        /// <inheritdoc />
        public override object[] Values
        {
            get => new object[] {FileName};
        }
    }
}