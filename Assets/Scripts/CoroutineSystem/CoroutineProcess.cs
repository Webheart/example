using System;
using System.Collections;

namespace UnityCore.CoroutineSystem
{
    public class CoroutineProcess : ICoroutineProcess
    {
        public CoroutineProcess NextProcess;

        public bool Completed => state == ProcessState.Completed || state == ProcessState.Stopped;

        public event Action< ICoroutineProcess > OnCompleted;

        private readonly IEnumerator enumerator;

        private ProcessState state;

        private CoroutineProcess innerProcess;

        public CoroutineProcess( IEnumerator instruction )
        {
            enumerator = new YieldInstruction( instruction ).GetEnumerator();
        }

        private CoroutineProcess( YieldInstruction instruction )
        {
            enumerator = instruction.GetEnumerator();
        }

        public void Start()
        {
            if( Completed || state == ProcessState.Stopped ) return;
            state = ProcessState.Running;
            innerProcess?.Start();
        }

        public void Pause()
        {
            if( Completed || state == ProcessState.Stopped ) return;
            state = ProcessState.Paused;
            innerProcess?.Pause();
        }

        public void Stop()
        {
            if( Completed ) return;
            state = ProcessState.Stopped;
            innerProcess?.Stop();
        }

        public void Update()
        {
            if( state != ProcessState.Running ) return;

            if( innerProcess != null && !innerProcess.Completed )
            {
                innerProcess.Update();
                return;
            }

            if( enumerator.MoveNext() )
            {
                if( !( enumerator.Current is YieldInstruction ) ) return;
                innerProcess = new CoroutineProcess( ( YieldInstruction )enumerator.Current );
                innerProcess.Start();
            }
            else Finish();
        }

        private void Finish()
        {
            state = ProcessState.Completed;
            OnCompleted?.Invoke( this );
        }
    }
}
