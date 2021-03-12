using NUnit.Framework;

namespace WindowsFirewallHelper.Tests
{
    public class FirewallManagerTests
    {
        [Test]
        public void TryGetInstanceTest()
        {
            // NOTE: this may vary depending on the machine on which it is run.
            var successful = FirewallManager.TryGetInstance(out var instance);

            if (successful)
            {
                Assert.IsNotNull(instance);
            }
            else
            {
                Assert.IsNull(instance);
            }
        }

        [Test]
        public void TryGetRegisteredProductsTest()
        {
            // NOTE: this may vary depending on the machine on which it is run.
            var successful = FirewallManager.TryGetRegisteredProducts(out var collection);

            if (successful)
            {
                Assert.IsNotNull(collection);
            }
            else
            {
                Assert.IsNull(collection);
            }
        }
    }
}
