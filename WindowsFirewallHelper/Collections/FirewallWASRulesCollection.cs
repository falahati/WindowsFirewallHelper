using System;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using WindowsFirewallHelper.COMInterop;
using WindowsFirewallHelper.FirewallRules;
using WindowsFirewallHelper.InternalHelpers.Collections;

namespace WindowsFirewallHelper.Collections
{
    internal class
        FirewallWASRulesCollection<TManaged> :
            ComCollectionBase<INetFwRules, INetFwRule, string, TManaged>,
            IFirewallWASRulesCollection<TManaged>
        where TManaged : class, IFirewallRule
    {
        /// <inheritdoc />
        public FirewallWASRulesCollection(INetFwRules rulesCollection) : base(rulesCollection)
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
        public new FirewallWASRule this[string name]
        {
            get => base[name] as FirewallWASRule;
        }

        /// <inheritdoc />
        protected override INetFwRule ConvertManagedToNative(TManaged managed)
        {
            if (!(managed is FirewallWASRule))
            {
                throw new ArgumentException("Passed argument is invalid.", nameof(managed));
            }

            return (managed as FirewallWASRule).GetCOMObject();
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

                    return new FirewallWASRuleWin8(nativeRule3) as TManaged;
                case INetFwRule2 nativeRule2:

                    return new FirewallWASRuleWin7(nativeRule2) as TManaged;
                default:

                    return new FirewallWASRule(native) as TManaged;
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
            try
            {
                return NativeEnumerable.Item(key);
            }
            catch (FileNotFoundException)
            {
                return null;
            }
        }

        /// <inheritdoc />
        protected override void InternalRemove(string key)
        {
            NativeEnumerable.Remove(key);
        }
    }
}