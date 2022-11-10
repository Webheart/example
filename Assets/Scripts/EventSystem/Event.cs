using System;
using System.Collections.Generic;

namespace UnityCore.EventSystem
{
    public class Event : IDisposable
    {
        private readonly List< Action > actions = new List< Action >();

        public void Invoke()
        {
            for( var i = 0; i < actions.Count; i++ )
            {
                actions[ i ]();
            }
        }

        public void Add( Action action )
        {
            if( action == null ) return;
            actions.Add( action );
        }

        public bool Remove( Action action )
        {
            return actions.Remove( action );
        }

        public void Add( Event repeater )
        {
            Add( repeater.Invoke );
        }

        public bool Remove( Event repeater )
        {
            return Remove( repeater.Invoke );
        }

        public void Dispose()
        {
            actions.Clear();
        }
    }

    public class Event< T > : IDisposable
    {
        private readonly List< Action< T > > actions = new List< Action< T > >();

        public void Invoke( T eventArgs )
        {
            for( var i = 0; i < actions.Count; i++ )
            {
                actions[ i ]( eventArgs );
            }
        }

        public void Add( Action< T > action )
        {
            if( action == null ) return;
            actions.Add( action );
        }

        public bool Remove( Action< T > action )
        {
            return actions.Remove( action );
        }

        public void Add( Event< T > repeater )
        {
            Add( repeater.Invoke );
        }

        public bool Remove( Event< T > repeater )
        {
            return Remove( repeater.Invoke );
        }

        public void Dispose()
        {
            actions.Clear();
        }
    }
}
