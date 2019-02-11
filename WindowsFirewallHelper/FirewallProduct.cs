using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WindowsFirewallHelper.COMInterop;
using WindowsFirewallHelper.InternalHelpers;

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

            UnderlyingObject = ComHelper.CreateInstance<INetFwProduct>();
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
            get => ComHelper.IsSupported<INetFwProduct>();
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

        private INetFwProduct UnderlyingObject { get; }


        public INetFwProduct GetCOMObject()
        {
            return UnderlyingObject;
        }
    }
}