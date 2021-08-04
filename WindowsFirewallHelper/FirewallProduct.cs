using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WindowsFirewallHelper.COMInterop;
using WindowsFirewallHelper.InternalHelpers;

namespace WindowsFirewallHelper
{
    /// <summary>
    ///     Represents a third party firewall product
    /// </summary>
    public class FirewallProduct
    {
        /// <summary>
        ///     Creates a new <see cref="FirewallProduct" /> instance locally to be registered later
        /// </summary>
        /// <param name="name">The name of the product</param>
        public FirewallProduct(string name) : this(name, new COMTypeResolver())
        {
        }

        /// <summary>
        ///     Creates a new <see cref="FirewallProduct" /> instance remotely to be registered later
        /// </summary>
        /// <param name="name">The name of the product</param>
        /// <param name="typeResolver">The object resolver for COM+ objects</param>
        public FirewallProduct(string name, COMTypeResolver typeResolver)
        {
            TypeResolver = typeResolver;
            if (!TypeResolver.IsSupported<INetFwProduct>())
            {
                throw new NotSupportedException();
            }

            UnderlyingObject = TypeResolver.CreateInstance<INetFwProduct>();
            Name = name;
        }

        internal FirewallProduct(INetFwProduct product)
        {
            UnderlyingObject = product;
        }

        /// <summary>
        ///     Gets the resolved name of this firewall product
        /// </summary>
        public string FriendlyName
        {
            get => NativeHelper.ResolveStringResource(Name);
        }

        /// <summary>
        ///     Gets a Boolean value indicating if third party firewall product registration is locally available and supported
        /// </summary>
        public static bool IsLocallySupported
        {
            get => IsSupported(new COMTypeResolver());
        }

        /// <summary>
        ///     Gets a Boolean value indicating if third party firewall product registration is remotely available and supported
        /// </summary>
        public static bool IsSupported(COMTypeResolver typeResolver)
        {
            return typeResolver.IsSupported<INetFwProduct>();
        }

        /// <summary>
        ///     Gets or sets the name of this firewall product
        /// </summary>
        public string Name
        {
            get => UnderlyingObject.DisplayName;
            set => UnderlyingObject.DisplayName = value;
        }

        /// <summary>
        ///     Gets the active COM+ Object resolver instance
        /// </summary>
        public COMTypeResolver TypeResolver { get; }

        /// <summary>
        ///     Gets or sets the rule categories that this firewall product is capable of handling
        /// </summary>
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

        /// <summary>
        ///     Gets the address of signed executable file that is responsible for this firewall product functionalities
        /// </summary>
        public string SignedExecutableFilename
        {
            get => UnderlyingObject.PathToSignedProductExe;
        }

        private INetFwProduct UnderlyingObject { get; }

        /// <summary>
        ///     Gets the underlying COM object for this firewall product
        /// </summary>
        /// <returns>The underlying COM object</returns>
        public INetFwProduct GetCOMObject()
        {
            return UnderlyingObject;
        }
    }
}