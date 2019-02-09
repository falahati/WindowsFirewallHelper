using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace WindowsFirewallHelper.Helpers
{
    public abstract class COMCollectionKey : IStructuralEquatable
    {
        public abstract Type[] Types { get; }

        public abstract object[] Values { get; }

        bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
        {
            return Equals(other, comparer);
        }

        int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
        {
            return GetHashCode(comparer);
        }

        private static IEqualityComparer CreateComparer(Type genericParameter)
        {
#if NET40
            return typeof(EqualityComparer<>)
                .MakeGenericType(genericParameter)
                .GetProperty(nameof(EqualityComparer<int>.Default), BindingFlags.Static)
                ?.GetGetMethod()
                ?.Invoke(null, new object[0]) as IEqualityComparer;
#else
            return typeof(EqualityComparer<>)
                .MakeGenericType(genericParameter)
                .GetProperty(nameof(EqualityComparer<int>.Default), BindingFlags.Static)
                ?.GetMethod
                ?.Invoke(null, new object[0]) as IEqualityComparer;
#endif
        }

        public override bool Equals(object obj)
        {
            return Equals(obj, null);
        }

        public override int GetHashCode()
        {
            return GetHashCode(null);
        }

        private bool Equals(object other, IEqualityComparer comparer)
        {
            if (!(other is COMCollectionKey otherKey))
            {
                return false;
            }

            if (Types.Length != otherKey.Types.Length)
            {
                return false;
            }

            for (var i = 0; i < Types.Length; i++)
            {
                if (Types[i] != otherKey.Types[i])
                {
                    return false;
                }

                var typeComparer = comparer ?? CreateComparer(Types[i]);

                if (!typeComparer.Equals(Values[i], otherKey.Values[i]))
                {
                    return false;
                }
            }

            return true;
        }

        private int GetHashCode(IEqualityComparer comparer)
        {
            var hashCode = 181846194;

            for (var i = 0; i < Types.Length; i++)
            {
                var typeComparer = comparer ?? CreateComparer(Types[i]);

                unchecked
                {
                    hashCode = hashCode * -1521134295 + typeComparer.GetHashCode(Values[i]);
                }
            }

            return hashCode;
        }
    }
}