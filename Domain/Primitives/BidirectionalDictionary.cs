namespace Domain.Primitives
{
    public class BidirectionalDictionary<TKey, TValue>
        where TKey : notnull
        where TValue : notnull
    {
        private readonly Dictionary<TKey, TValue> forwardDictionary = new Dictionary<TKey, TValue>();
        private readonly Dictionary<TValue, TKey> reverseDictionary = new Dictionary<TValue, TKey>();

        public BidirectionalDictionary(IEnumerable<KeyValuePair<TKey, TValue>> keyValuePairs)
        {
            foreach (var kvp in keyValuePairs)
            {
                Add(kvp.Key, kvp.Value);
            }
        }

        public void Add(TKey key, TValue value)
        {
            forwardDictionary[key] = value;
            reverseDictionary[value] = key;
        }

        public bool TryGetByFirstKey(TKey key, out TValue value)
        {
            return forwardDictionary.TryGetValue(key, out value!);
        }

        public bool TryGetBySecondKey(TValue value, out TKey key)
        {
            return reverseDictionary.TryGetValue(value, out key!);
        }
    }
}
