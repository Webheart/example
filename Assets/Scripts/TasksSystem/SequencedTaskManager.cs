using System;
using UnityCore.EventSystem;
using UnityCore.CoroutineSystem;

namespace UnityCore.TaskSystem
{
    public class SequencedTaskManager : AbstractTaskManager
    {
        private ITask currentTask;
        private ICoroutineProcess currentProcess;

        public override event Action< ITaskManager > OnAllTasksCompleted
        {
            add => onAllTasksCompleted.Add( value );
            remove => onAllTasksCompleted.Remove( value );
        }

        private readonly Event< ITaskManager > onAllTasksCompleted = new Event< ITaskManager >();

        public SequencedTaskManager( ICoroutinesManager coroutineManager ) : base( coroutineManager ) {}

        public override void Start()
        {
            if( tasks.Count == 0 ) return;
            if( currentTask != null )
            {
                currentTask.Start();
                currentProcess.Start();
            }
            else StartTask( tasks[ 0 ] );

            State = ProcessState.Running;
        }

        public override void Pause()
        {
            if( currentTask == null ) return;
            PauseTask( currentTask );
            State = ProcessState.Paused;
        }

        public override void Stop()
        {
            if( currentTask == null ) return;
            StopTask( currentTask );
            State = ProcessState.Stopped;
        }

        protected override void StartTask( ITask task )
        {
            currentTask = task;
            currentProcess = coroutineManager.StartCoroutine( task.GetCoroutine() );
        }

        public override bool PauseTask( ITask task )
        {
            if( task != currentTask ) return false;
            task.Pause();
            currentProcess.Pause();
            return true;
        }

        protected override void StopTask( ITask task )
        {
            if( task != currentTask ) return;
            task.Stop();
            currentProcess.Stop();
            currentTask = null;
            currentProcess = null;
        }

        protected override void FinishTask( ITask task )
        {
            var index = tasks.IndexOf( task ) + 1;
            if( index < tasks.Count )
            {
                StartTask( tasks[ index ] );
            }
            else
            {
                currentTask = null;
                currentProcess = null;
                State = ProcessState.Completed;
                onAllTasksCompleted.Invoke( this );
            }
        }
    }
}
