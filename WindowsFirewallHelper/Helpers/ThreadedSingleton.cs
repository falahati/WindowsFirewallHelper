using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace WindowsFirewallHelper.Helpers
{
    public class ThreadedSingleton<T> where T : class, new()
    {
        protected static Dictionary<Thread, T> Instances = new Dictionary<Thread, T>();

        protected static T GetInstance()
        {
            lock (Instances)
            {
                foreach (var thread in Instances.Keys.ToArray())
                {
                    if (!thread.IsAlive)
                    {
                        Instances.Remove(thread);
                    }
                }

                var currentThreadId = Thread.CurrentThread;

                if (!Instances.ContainsKey(currentThreadId))
                {
                    T instance = null;

                    try
                    {
                        if (currentThreadId.GetApartmentState() == ApartmentState.MTA)
                        {
                            instance = Instances
                                .FirstOrDefault(
                                    pair => pair.Key.IsAlive && pair.Key.GetApartmentState() == ApartmentState.MTA
                                ).Value;
                        }
                    }
                    catch
                    {
                        // ignore
                    }

                    Instances.Add(currentThreadId, instance ?? new T());
                }

                return Instances[currentThreadId];
            }
        }
    }
}