using System.Collections.Generic;

namespace Sharp.Core
{
    /// <summary>
    /// Abstract base class for a cache from arbitrary keys to values computed via member function.
    /// Most useful to implement acceleration structures.
    /// </summary>
    /// <typeparam name="K">The type of the key.</typeparam>
    /// <typeparam name="V">The type of the value.</typeparam>
    public abstract class AbstractCache<K, V>
    {
        private readonly Dictionary<K, V> _values = new();

        /// <summary>
        /// The function used to compute the value that should be placed into the cache for the given key.
        /// </summary>
        /// <param name="key">The key to compute the cached value for.</param>
        /// <returns>The value to inject into the cache.</returns>
        protected abstract V ComputeCacheValue(K key);

        /// <summary>
        /// Get the value in the cache associated with the given key, computing it if needed.
        /// </summary>
        /// <param name="key">The key to get the cached value for.</param>
        /// <returns>The value from the cache.</returns>
        public V Get(K key)
        {
            if (_values.TryGetValue(key, out var cached))
            {
                return cached;
            }

            var result = ComputeCacheValue(key);
            _values.Add(key, result);

            return result;
        }

        /// <summary>
        /// Invalidate the cache for the provided key.
        /// </summary>
        /// <param name="key">The key.</param>
        public void Invalidate(K key)
        {
            _values.Remove(key);
        }

        /// <summary>
        /// Invalidate all cache entries.
        /// </summary>
        public void InvalidateAll()
        {
            _values.Clear();
        }
    }
}