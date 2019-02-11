using System;
using System.Runtime.InteropServices.ComTypes;
using WindowsFirewallHelper.COMInterop;
using WindowsFirewallHelper.FirewallAPIv2.Rules;
using WindowsFirewallHelper.Helpers;

namespace WindowsFirewallHelper.FirewallAPIv2
{
    internal class
        FirewallRulesCollection<TManaged> : COMCollection<INetFwRules, INetFwRule, string, TManaged>
        where TManaged : class, IRule
    {
        /// <inheritdoc />
        public FirewallRulesCollection(INetFwRules rulesCollection) : base(rulesCollection)
        {
        }

        /// <inheritdoc />
        public override bool IsReadOnly { get; } = false;

        /// <inheritdoc />
        public override bool Remove(TManaged item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var oldName = item.Name;

            try
            {
                item.Name = Guid.NewGuid().ToString("N");
                base.Remove(item);

                return true;
            }
            catch
            {
                item.Name = oldName;

                throw;
            }
        }

        /// <inheritdoc />
        protected override INetFwRule ConvertManagedToNative(TManaged managed)
        {
            if (!(managed is StandardRule))
            {
                throw new ArgumentException("Passed argument is invalid.", nameof(managed));
            }

            return (managed as StandardRule).GetCOMObject();
        }

        /// <inheritdoc />
        protected override TManaged ConvertNativeToManaged(INetFwRule native)
        {
            if (native == null)
            {
                return null;
            }


            switch (native)
            {
                case INetFwRule3 nativeRule3:

                    return new StandardRuleWin8(nativeRule3) as TManaged;
                case INetFwRule2 nativeRule2:

                    return new StandardRuleWin7(nativeRule2) as TManaged;
                default:

                    return new StandardRule(native) as TManaged;
            }
        }

        /// <inheritdoc />
        protected override string GetCollectionKey(TManaged managed)
        {
            return ConvertManagedToNative(managed).Name;
        }

        /// <inheritdoc />
        protected override IEnumVARIANT GetEnumVariant()
        {
            return NativeEnumerable.GetEnumeratorVariant();
        }

        /// <inheritdoc />
        protected override void InternalAdd(INetFwRule native)
        {
            NativeEnumerable.Add(native);
        }

        /// <inheritdoc />
        protected override int InternalCount()
        {
            return NativeEnumerable.Count;
        }

        /// <inheritdoc />
        protected override INetFwRule InternalItem(string key)
        {
            return NativeEnumerable.Item(key);
        }

        /// <inheritdoc />
        protected override void InternalRemove(string key)
        {
            NativeEnumerable.Remove(key);
        }
    }
}