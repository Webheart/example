using System;
using UnityCore.EventSystem;

namespace UnityCore.NotifierTypes
{
    public interface INotifierProperty< T >
    {
        event Action< T > OnValueChanged;

        T Value { get; set; }
    }

    public class NotifierProperty< T > : INotifierProperty< T >
    {
        public event Action< T > OnValueChanged
        {
            add => onValueChanged.Add( value );
            remove => onValueChanged.Remove( value );
        }

        private readonly Event< T > onValueChanged = new Event< T >();

        private T value;

        public T Value
        {
            get => value;
            set
            {
                this.value = value;
                onValueChanged.Invoke( this.value );
            }
        }

        public NotifierProperty()
        {
            value = default;
        }

        public NotifierProperty( T value )
        {
            this.value = value;
        }

        public void Subscribe( Action< T > action, bool invokeImmediately )
        {
            if( invokeImmediately ) action( value );
            onValueChanged.Add( action );
        }

        public void Unsubscribe( Action< T > action ) => onValueChanged.Remove( action );

        public override string ToString()
        {
            return value != null ? value.ToString() : "null";
        }
        
        public static implicit operator T( NotifierProperty< T > property )
        {
            return property.Value;
        }
    }
}
