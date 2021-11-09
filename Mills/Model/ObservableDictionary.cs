using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Mills.Model
{
    public class ObservableDictionary<K, V> : IDictionary<K, V>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        private const string CountString = "Count";

        // This must agree with Binding.IndexerName.  It is declared separately
        // here so as to avoid a dependency on PresentationFramework.dll.
        private const string IndexerName = "Item[]";

        private IDictionary<K, V> dictionary =  new Dictionary<K, V>();

        public V this[K key] { get => dictionary[key]; set => dictionary[key] = value; }

        public ICollection<K> Keys => dictionary.Keys;

        public ICollection<V> Values => dictionary.Values;

        public int Count => dictionary.Count;

        public bool IsReadOnly => dictionary.IsReadOnly;

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        public void Add(K key, V value)
        {
            var item = new KeyValuePair<K, V>(key, value);

            dictionary.Add(key, value);
            OnPropertyChanged(CountString);
            OnPropertyChanged(IndexerName);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
        }

        public void Add(KeyValuePair<K, V> item)
        {
            dictionary.Add(item);
            OnPropertyChanged(CountString);
            OnPropertyChanged(IndexerName);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
        }

        public void Clear()
        {
            dictionary.Clear();
            OnPropertyChanged(CountString);
            OnPropertyChanged(IndexerName);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public bool Contains(KeyValuePair<K, V> item)
        {
            return dictionary.Contains(item);
        }

        public bool ContainsKey(K key)
        {
            return dictionary.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex)
        {
            dictionary.CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }

        public bool Remove(K key)
        {
            KeyValuePair<K, V> item;
                
            if(dictionary.TryGetValue(key, out V value))
            {
                item = new KeyValuePair<K, V>(key, value);
            } 
            else
            {
                return false;
            }
            
            var result = dictionary.Remove(key);

            if(result)
            {
                OnPropertyChanged(CountString);
                OnPropertyChanged(IndexerName);
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
            }

            return result;
        }

        public bool Remove(KeyValuePair<K, V> item)
        {
            var result = dictionary.Remove(item);
            if(result)
            {
                OnPropertyChanged(CountString);
                OnPropertyChanged(IndexerName);
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
            }
                

            return result;
        }

        public bool TryGetValue(K key, [MaybeNullWhen(false)] out V value)
        {
            return dictionary.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }
    }
}
