﻿namespace EasyHash
{
    using System;
    using System.Threading;
    using Expressions;
    using JetBrains.Annotations;

    public static class EasyHash<T>
    {
        private static Func<T, int> defaultHasher;
        private static Func<T, int> hasher;
        private static Exception initializationException;

        static EasyHash()
        {
            Reset();
        }

        internal static void Reset()
        {
            try
            {
                hasher = defaultHasher = new GetHashCodeExpressionBuilder<T>().Build().Compile();
                initializationException = null;
            }
            catch (Exception e)
            {
                initializationException = e;
            }
        }

        public static void Register([NotNull]Action<GetHashCodeConfiguration<T>> configure)
        {
            try
            {
                var configuration = new GetHashCodeConfiguration<T>();
                configure(configuration);
                Func<T, int> hashFn = new GetHashCodeExpressionBuilder<T>(configuration).Build().Compile();

                if (Interlocked.CompareExchange(ref hasher, hashFn, defaultHasher) != defaultHasher)
                {
                    throw new InvalidOperationException($"Type '{typeof(T)}' was already registered with FastEasyHash");
                }
            }
            catch (Exception ex)
            {
                initializationException = ex;
            }
        }

        //TODO: autogenerate Equals as well
        public static bool Equals(T obj, object other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(obj, other)) return true;
            return other is T && obj.GetHashCode() == other.GetHashCode();
        }

        public static int GetHashCode(T obj)
        {
            if (initializationException != null) { throw initializationException; }
            if (obj == null) { throw new ArgumentNullException(nameof(obj)); }

            return hasher(obj);
        }
    }
}
