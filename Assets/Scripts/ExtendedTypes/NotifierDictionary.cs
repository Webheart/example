using System;
using System.Collections;
using System.Collections.Generic;
using UnityCore.EventSystem;

namespace UnityCore.NotifierTypes
{
    public class NotifierDictionary< TKey, TValue > : IDictionary< TKey, TValue >
    {
        public event Action< KeyValuePair< TKey, TValue > > OnAddItem
        {
            add => onAddItem.Add( value );
            remove => onAddItem.Remove( value );
        }

        private readonly Event< KeyValuePair< TKey, TValue > > onAddItem = new Event< KeyValuePair< TKey, TValue > >();

        public event Action< IDictionary< TKey, TValue > > OnAddRange
        {
            add => onAddRange.Add( value );
            remove => onAddRange.Remove( value );
        }

        private readonly Event< IDictionary< TKey, TValue > > onAddRange = new Event< IDictionary< TKey, TValue > >();

        public event Action< IDictionary< TKey, TValue > > OnClear
        {
            add => onClear.Add( value );
            remove => onClear.Remove( value );
        }

        private readonly Event< IDictionary< TKey, TValue > > onClear = new Event< IDictionary< TKey, TValue > >();

        public event Action< KeyValuePair< TKey, TValue > > OnItemChanged
        {
            add => onItemChanged.Add( value );
            remove => onItemChanged.Remove( value );
        }

        private readonly Event< KeyValuePair< TKey, TValue > > onItemChanged = new Event< KeyValuePair< TKey, TValue > >();

        public event Action< KeyValuePair< TKey, TValue > > OnItemRemoved
        {
            add => onItemRemoved.Add( value );
            remove => onItemRemoved.Remove( value );
        }

        private readonly Event< KeyValuePair< TKey, TValue > > onItemRemoved = new Event< KeyValuePair< TKey, TValue > >();

        private readonly Dictionary< TKey, TValue > dictionary;

        public int Count => dictionary.Count;

        public bool IsReadOnly => false;

        public TValue this[ TKey key ]
        {
            get => dictionary[ key ];
            set
            {
                dictionary[ key ] = value;
                onItemChanged.Invoke( new KeyValuePair< TKey, TValue >( key, value ) );
            }
        }

        public ICollection< TKey > Keys => dictionary.Keys;

        public ICollection< TValue > Values => dictionary.Values;

        public delegate void ItemEventHandler( TKey key, TValue value );

        public NotifierDictionary()
        {
            dictionary = new Dictionary< TKey, TValue >();
        }

        public IEnumerator< KeyValuePair< TKey, TValue > > GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }

        public void Add( KeyValuePair< TKey, TValue > item )
        {
            dictionary.Add( item.Key, item.Value );
            onAddItem.Invoke( item );
        }

        public void Clear()
        {
            var oldDictionary = new Dictionary< TKey, TValue >( dictionary );
            dictionary.Clear();
            onClear.Invoke( oldDictionary );
        }

        public bool Contains( KeyValuePair< TKey, TValue > item )
        {
            return dictionary.ContainsKey( item.Key );
        }

        public void CopyTo( KeyValuePair< TKey, TValue >[] array, int arrayIndex )
        {
            throw new NotImplementedException();
        }

        public bool Remove( KeyValuePair< TKey, TValue > item )
        {
            var containsKey = dictionary.ContainsKey( item.Key );
            if( containsKey )
            {
                var removedValue = dictionary[ item.Key ];
                dictionary.Remove( item.Key );
                onItemRemoved.Invoke( new KeyValuePair< TKey, TValue >( item.Key, removedValue ) );
            }

            return containsKey;
        }

        public void Add( TKey key, TValue value )
        {
            dictionary.Add( key, value );
            onAddItem.Invoke( new KeyValuePair< TKey, TValue >( key, value ) );
        }

        public bool ContainsKey( TKey key )
        {
            return dictionary.ContainsKey( key );
        }

        public bool Remove( TKey key )
        {
            var containsKey = dictionary.ContainsKey( key );
            if( containsKey )
            {
                var removedValue = dictionary[ key ];
                dictionary.Remove( key );
                onItemRemoved.Invoke( new KeyValuePair< TKey, TValue >( key, removedValue ) );
            }

            return containsKey;
        }

        public bool TryGetValue( TKey key, out TValue value )
        {
            return dictionary.TryGetValue( key, out value );
        }

        public void AddRange( IDictionary< TKey, TValue > dictionary )
        {
            foreach( var value in dictionary )
            {
                this.dictionary.Add( value.Key, value.Value );
            }

            onAddRange?.Invoke( dictionary );
        }

        /// <summary>
        /// Если ключ не найден - вернет null вместо выброса исключения
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TValue GetSafe( TKey key )
        {
            TValue value;
            TryGetValue( key, out value );
            return value;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
