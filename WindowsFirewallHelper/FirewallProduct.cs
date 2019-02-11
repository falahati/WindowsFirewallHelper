using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WindowsFirewallHelper.COMInterop;
using WindowsFirewallHelper.Helpers;

namespace WindowsFirewallHelper
{
    public class FirewallProduct
    {
        public FirewallProduct(string name)
        {
            if (!IsSupported)
            {
                throw new NotSupportedException();
            }

            UnderlyingObject = COMHelper.CreateInstance<INetFwProduct>();
            Name = name;
        }

        internal FirewallProduct(INetFwProduct product)
        {
            UnderlyingObject = product;
        }

        public string FriendlyName
        {
            get => NativeHelper.ResolveStringResource(Name);
        }

        public static bool IsSupported
        {
            get => COMHelper.IsSupported<INetFwProduct>();
        }

        public string Name
        {
            get => UnderlyingObject.DisplayName;
            set => UnderlyingObject.DisplayName = value;
        }

        public FirewallRuleCategory[] RuleCategories
        {
            get
            {
                var arrayObject = UnderlyingObject.RuleCategories;

                if (arrayObject is IEnumerable array)
                {
                    var ruleCategories = new List<FirewallRuleCategory>();

                    foreach (var ruleCategoryObject in array)
                    {
                        var ruleCategoryInt = (int) ruleCategoryObject;
                        ruleCategories.Add((FirewallRuleCategory) ruleCategoryInt);
                    }

                    return ruleCategories.ToArray();
                }

                return new FirewallRuleCategory[0];
            }
            set
            {
                var array = value.Select(category => (int) category).Cast<object>().ToArray();
                UnderlyingObject.RuleCategories = array;
            }
        }

        public string SignedExecutableFilename
        {
            get => UnderlyingObject.PathToSignedProductExe;
        }

        /// <summary>
        ///     Returns the list of all registered third party firewalls management instances
        /// </summary>
        public static ICollection<FirewallProduct> RegisteredProducts
        {
            get => new FirewallProductsCollection(GetProducts());
        }

        private INetFwProduct UnderlyingObject { get; }

        private static INetFwProducts GetProducts()
        {
            if (!IsSupported)
            {
                throw new NotSupportedException();
            }

            return COMHelper.CreateInstance<INetFwProducts>();
        }

        /// <summary>
        ///     Register an instance of a third party firewall management class
        /// </summary>
        public FirewallProductRegistrationHandle Register()
        {
            return new FirewallProductRegistrationHandle(GetProducts().Register(GetCOMObject()));
        }


        public INetFwProduct GetCOMObject()
        {
            return UnderlyingObject;
        }
    }
}