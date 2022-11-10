using System;
using System.Collections;
using System.Collections.Generic;
using UnityCore.EventSystem;

namespace UnityCore.NotifierTypes
{
    public class NotifierStack< T > : Stack< T >
    {
        public event Action< T > OnAddItem
        {
            add => onAddItem.Add( value );
            remove => onAddItem.Remove( value );
        }

        private readonly Event< T > onAddItem = new Event< T >();
        
        public event Action< T > OnItemRemoved
        {
            add => onItemRemoved.Add( value );
            remove => onItemRemoved.Remove( value );
        }
        private readonly Event< T > onItemRemoved = new Event< T >();
        public event Action OnClear
        {
            add => onClear.Add( value );
            remove => onClear.Remove( value );
        }

        private readonly Event onClear = new Event();
        public new void Push( T item )
        {
            base.Push( item );
            onAddItem.Invoke( item );
        }

        public new T Pop()
        {
            var item = base.Pop();
            onItemRemoved.Invoke( item );
            return item;
        }

        public new void Clear()
        {
            base.Clear();
            onClear.Invoke();
        }
    }

    public class NotifierList< T > : List< T >, IList
    {
        public event Action< T > OnAddItem
        {
            add => onAddItem.Add( value );
            remove => onAddItem.Remove( value );
        }

        private readonly Event< T > onAddItem = new Event< T >();

        public event Action< IEnumerable< T > > OnAddRange
        {
            add => onAddRange.Add( value );
            remove => onAddRange.Remove( value );
        }

        private readonly Event< IEnumerable< T > > onAddRange = new Event< IEnumerable< T > >();

        public event Action OnClear
        {
            add => onClear.Add( value );
            remove => onClear.Remove( value );
        }

        private readonly Event onClear = new Event();

        public event Action< T > OnItemChanged
        {
            add => onItemChanged.Add( value );
            remove => onItemChanged.Remove( value );
        }

        private readonly Event< T > onItemChanged = new Event< T >();

        public event Action< T > OnItemRemoved
        {
            add => onItemRemoved.Add( value );
            remove => onItemRemoved.Remove( value );
        }

        private readonly Event< T > onItemRemoved = new Event< T >();

        public new T this[ int index ]
        {
            get => base[ index ];
            set
            {
                base[ index ] = value;
                onItemChanged.Invoke( value );
            }
        }

        public void AddRange( List< T > collection )
        {
            base.AddRange( collection );
            onAddRange.Invoke( collection );
        }

        public new void AddRange( IEnumerable< T > collection )
        {
            base.AddRange( collection );
            onAddRange.Invoke( collection );
        }

        public void AddRange( NotifierList< T > collection )
        {
            base.AddRange( collection );
            onAddRange.Invoke( collection );
        }

        public new void Add( T item )
        {
            base.Add( item );
            onAddItem.Invoke( item );
        }

        public new void Clear()
        {
            base.Clear();
            onClear.Invoke();
        }

        public new bool Remove( T item )
        {
            var containsItem = Contains( item );
            if( containsItem )
            {
                var index = IndexOf( item );
                base.Remove( item );
                onItemRemoved.Invoke( item );
            }

            return containsItem;
        }

        public new void Insert( int index, T item )
        {
            base.Insert( index, item );
            onAddItem.Invoke( item );
        }

        public new void RemoveAt( int index )
        {
            var removedItem = base[ index ];
            base.RemoveAt( index );
            onItemRemoved.Invoke( removedItem );
        }

        void IList.RemoveAt( int index )
        {
            RemoveAt( index );
        }
    }
}
