using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace WindowsFirewallHelper.InternalHelpers
{
    internal static class ThreadedSingleton
    {
        private static readonly Dictionary<Thread, Dictionary<Guid, object>> Instances =
            new Dictionary<Thread, Dictionary<Guid, object>>();

        public static T GetInstance<T>() where T : class, new()
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
                    Instances.Add(currentThreadId, new Dictionary<Guid, object>());
                }

                var typeGuid = typeof(T).GUID;

                if (!Instances[currentThreadId].ContainsKey(typeGuid))
                {
                    T instance = null;

                    try
                    {
                        if (currentThreadId.GetApartmentState() == ApartmentState.MTA)
                        {
                            var compatibleThread = Instances
                                .FirstOrDefault(
                                    pair => pair.Key.IsAlive &&
                                            pair.Key.GetApartmentState() == ApartmentState.MTA &&
                                            pair.Value.ContainsKey(typeGuid)
                                );

                            if (compatibleThread.Value != null)
                            {
                                instance =
                                    compatibleThread.Value.FirstOrDefault(pair => pair.Key == typeGuid).Value as T;
                            }
                        }
                    }
                    catch
                    {
                        // ignore
                    }

                    Instances[currentThreadId].Add(typeGuid, instance ?? new T());
                }

                return Instances[currentThreadId][typeGuid] as T;
            }
        }
    }
}