using System.Linq;
using System.Threading;
using NUnit.Framework;

namespace WindowsFirewallHelper.Tests
{
    public class ThreadSafeSingletonTests
    {
        [Test]
        public void MultiMTAThreadAccessTest()
        {
            string[] rules1 = null;
            string[] rules2 = null;
            IFirewall instance1 = null;
            IFirewall instance2 = null;
            var threadId1 = 0;
            var threadId2 = 0;

            var thread1 = new Thread(() =>
            {
                Thread.Sleep(1000);
                threadId1 = Thread.CurrentThread.ManagedThreadId;
                instance1 = FirewallManager.Instance;
                rules1 = FirewallManager.Instance.Rules.Select(rule => rule.Name).ToArray();
            });

            var thread2 = new Thread(() =>
            {
                Thread.Sleep(2000);
                threadId2 = Thread.CurrentThread.ManagedThreadId;
                instance2 = FirewallManager.Instance;
                rules2 = FirewallManager.Instance.Rules.Select(rule => rule.Name).ToArray();
            });

            thread1.SetApartmentState(ApartmentState.MTA);
            thread2.SetApartmentState(ApartmentState.MTA);

            thread1.IsBackground = false;
            thread2.IsBackground = false;

            thread1.Start();
            thread2.Start();

            while (thread1.IsAlive || thread2.IsAlive)
            {
                Thread.Sleep(100);
            }

            Assert.AreNotEqual(threadId1, threadId2);
            Assert.AreSame(instance1, instance2);

            Assert.IsTrue(rules1.SequenceEqual(rules2));
        }

        [Test]
        public void MultiSTAThreadAccessTest()
        {
            string[] rules1 = null;
            string[] rules2 = null;
            IFirewall instance1 = null;
            IFirewall instance2 = null;
            var threadId1 = 0;
            var threadId2 = 0;

            var thread1 = new Thread(() =>
            {
                Thread.Sleep(1000);
                threadId1 = Thread.CurrentThread.ManagedThreadId;
                instance1 = FirewallManager.Instance;
                rules1 = FirewallManager.Instance.Rules.Select(rule => rule.Name).ToArray();
            });

            var thread2 = new Thread(() =>
            {
                Thread.Sleep(2000);
                threadId2 = Thread.CurrentThread.ManagedThreadId;
                instance2 = FirewallManager.Instance;
                rules2 = FirewallManager.Instance.Rules.Select(rule => rule.Name).ToArray();
            });

            thread1.SetApartmentState(ApartmentState.STA);
            thread2.SetApartmentState(ApartmentState.STA);

            thread1.IsBackground = false;
            thread2.IsBackground = false;

            thread1.Start();
            thread2.Start();

            while (thread1.IsAlive || thread2.IsAlive)
            {
                Thread.Sleep(100);
            }

            Assert.AreNotEqual(threadId1, threadId2);
            Assert.AreNotSame(instance1, instance2);

            Assert.IsTrue(rules1.SequenceEqual(rules2));
        }
    }
}