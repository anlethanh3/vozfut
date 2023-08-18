using System.Collections.Concurrent;

namespace FootballManager.Domain.Enums
{
    /// <summary>
    /// Enum base class
    /// </summary>
    /// <typeparam name="TEnum">The enum</typeparam>
    /// <typeparam name="TValue">the data type</typeparam>
    public abstract class BaseEnumeration<TEnum, TValue> where TEnum : BaseEnumeration<TEnum, TValue>
    {
        private static readonly ConcurrentDictionary<TValue, TEnum> s_items = new();

        public TValue Value { get; }
        public string Name { get; }

        protected BaseEnumeration(TValue value, string name)
        {
            Value = value;
            Name = name;
        }

        /// <summary>
        /// Get detail value from <typeparamref name="TEnum"/>
        /// </summary>
        /// <param name="value">The key cache in <typeparamref name="TValue"/> </param>
        /// <returns>Get match key/value in concurren dictionary</returns>
        public static TEnum FromValue(TValue value)
        {
            return s_items[value];
        }

        public static bool TryFromValue(TValue value, out TEnum result)
        {
            return s_items.TryGetValue(value, out result);
        }

        /// <summary>
        /// Get all value from <typeparamref name="TEnum"/>
        /// </summary>
        /// <returns>List enum cache in concurren dictionary</returns>
        public static IEnumerable<TEnum> GetAll()
        {
            return s_items.Values;
        }

        protected static TEnum Register(TEnum item)
        {
            return s_items.GetOrAdd(item.Value, item);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
