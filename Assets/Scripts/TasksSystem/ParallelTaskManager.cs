using System;
using System.Collections.Generic;
using UnityCore.EventSystem;
using UnityCore.CoroutineSystem;

namespace UnityCore.TaskSystem
{
    public class ParallelTaskManager : AbstractTaskManager
    {
        protected readonly Dictionary< ITask, ICoroutineProcess > runningTasks = new Dictionary< ITask, ICoroutineProcess >();

        public override event Action< ITaskManager > OnAllTasksCompleted
        {
            add => onAllTasksCompleted.Add( value );
            remove => onAllTasksCompleted.Remove( value );
        }

        private readonly Event< ITaskManager > onAllTasksCompleted = new Event< ITaskManager >();

        public ParallelTaskManager( ICoroutinesManager coroutineManager ) : base( coroutineManager ) {}

        public override void Start()
        {
            foreach( var task in tasks ) StartTask( task );
            State = ProcessState.Running;
        }

        public override void Pause()
        {
            foreach( var task in tasks ) PauseTask( task );
            State = ProcessState.Paused;
        }

        public override void Stop()
        {
            foreach( var task in tasks ) StopTask( task );
            State = ProcessState.Stopped;
        }

        protected override void StartTask( ITask task )
        {
            task.Start();

            if( runningTasks.ContainsKey( task ) )
            {
                runningTasks[ task ].Start();
            }
            else
            {
                runningTasks.Add( task, coroutineManager.StartCoroutine( task.GetCoroutine() ) );
            }
        }

        public override bool PauseTask( ITask task )
        {
            if( !runningTasks.ContainsKey( task ) ) return false;
            task.Pause();
            runningTasks[ task ].Pause();
            return true;
        }

        protected override void StopTask( ITask task )
        {
            if( !runningTasks.ContainsKey( task ) ) return;
            task.Stop();
            runningTasks[ task ].Stop();
            runningTasks.Remove( task );
        }

        protected override void FinishTask( ITask task )
        {
            if( !runningTasks.ContainsKey( task ) ) return;
            runningTasks.Remove( task );
            if( runningTasks.Count == 0 )
            {
                State = ProcessState.Completed;
                onAllTasksCompleted?.Invoke( this );
            }
        }
    }
}
