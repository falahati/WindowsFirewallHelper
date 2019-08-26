using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace WindowsFirewallHelper.InternalHelpers
{
    internal static class ThreadSafeSingleton
    {
        private static readonly IList<ThreadInfo> Threads = new List<ThreadInfo>();

        public static T GetInstance<T>() where T : class, new()
        {
            lock (Threads)
            {
                try
                {
                    var currentThread = Thread.CurrentThread;
                    var threadInfo = Threads.FirstOrDefault(info => info.ThreadId == currentThread.ManagedThreadId);

                    if (threadInfo == null)
                    {
                        threadInfo = new ThreadInfo(currentThread);
                        Threads.Add(threadInfo);
                    }

                    var instance = threadInfo.GetInstance<T>();

                    if (instance != null)
                    {
                        return instance;
                    }

                    if (threadInfo.ApartmentState == ApartmentState.MTA)
                    {
                        instance = Threads
                            .Where(info => info.ApartmentState == ApartmentState.MTA)
                            .Select(info => info.GetInstance<T>())
                            .FirstOrDefault(arg => arg != null);

                        if (instance != null)
                        {
                            return threadInfo.AddInstance(instance);
                        }
                    }

                    return threadInfo.AddInstance<T>();
                }
                finally
                {
                    var deadThreads = Threads.Where(info => !info.IsAlive).ToArray();

                    foreach (var deadThread in deadThreads)
                    {
                        Threads.Remove(deadThread);
                    }
                }
            }
        }

        private class ThreadInfo : IEquatable<ThreadInfo>
        {
            private readonly Thread _thread;
            private readonly Dictionary<Guid, object> _types;

            public ThreadInfo(Thread thread)
            {
                ApartmentState = thread.GetApartmentState();
                ThreadId = thread.ManagedThreadId;
                _thread = thread;
                _types = new Dictionary<Guid, object>();
            }

            public ApartmentState ApartmentState { get; }

            public bool IsAlive
            {
                get
                {
                    try
                    {
                        return _thread.IsAlive;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
            }

            public int ThreadId { get; }

            /// <inheritdoc />
            public bool Equals(ThreadInfo other)
            {
                if (ReferenceEquals(null, other))
                {
                    return false;
                }

                if (ReferenceEquals(this, other))
                {
                    return true;
                }

                return ThreadId == other.ThreadId;
            }

            public static bool operator ==(ThreadInfo left, ThreadInfo right)
            {
                return Equals(left, right);
            }

            public static bool operator !=(ThreadInfo left, ThreadInfo right)
            {
                return !Equals(left, right);
            }

            /// <inheritdoc />
            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj))
                {
                    return false;
                }

                if (ReferenceEquals(this, obj))
                {
                    return true;
                }

                return Equals(obj as ThreadInfo);
            }

            /// <inheritdoc />
            public override int GetHashCode()
            {
                return ThreadId;
            }

            public T AddInstance<T>(T instance = null) where T : class, new()
            {
                var type = typeof(T).GUID;
                instance = instance ?? new T();
                _types[type] = instance;

                return instance;
            }

            // ReSharper disable once MemberHidesStaticFromOuterClass
            public T GetInstance<T>() where T : class, new()
            {
                var type = typeof(T).GUID;

                return _types.FirstOrDefault(pair => pair.Key.Equals(type)).Value as T;
            }
        }
    }
}