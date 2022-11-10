using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityCore.CoroutineSystem
{
    public interface ICoroutinesManager : IDisposable
    {
        ICoroutineProcess StartCoroutine( IEnumerator instruction );

        void StopAllCoroutines();
    }

    public class CoroutinesManager : ICoroutinesManager
    {
        private ITimeProvider timeProvider;

        private List< CoroutineProcess > list = new List< CoroutineProcess >();

        public CoroutinesManager( ITimeProvider timeProvider )
        {
            this.timeProvider = timeProvider;
            timeProvider.OnUpdate += OnUpdateHandler;
        }

        public void Dispose()
        {
            timeProvider.OnUpdate -= OnUpdateHandler;
        }

        public ICoroutineProcess StartCoroutine( IEnumerator instruction )
        {
            var process = new CoroutineProcess( instruction );
            list.Add( process );
            process.Start();
            return process;
        }

        public void StopAllCoroutines()
        {
            for( var i = 0; i < list.Count; i++ )
            {
                list[ i ].Stop();
            }

            list.Clear();
        }

        private void OnUpdateHandler()
        {
            for( var i = list.Count - 1; i >= 0; i-- )
            {
                if( list[ i ].Completed ) list.RemoveAt( i );
            }

            for( var i = 0; i < list.Count; i++ )
            {
                list[ i ].Update();
            }
        }
    }
}
