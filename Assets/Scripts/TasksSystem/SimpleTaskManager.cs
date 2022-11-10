using System;
using UnityCore.EventSystem;
using UnityCore.CoroutineSystem;

namespace UnityCore.TaskSystem
{
    public class SimpleTaskManager : ParallelTaskManager
    {
        public override event Action< ITaskManager > OnAllTasksCompleted
        {
            add => onAllTasksCompleted.Add( value );
            remove => onAllTasksCompleted.Remove( value );
        }

        private readonly Event< ITaskManager > onAllTasksCompleted = new Event< ITaskManager >();

        public SimpleTaskManager( ICoroutinesManager coroutineManager ) : base( coroutineManager ) {}

        public override void AddTask( ITask task )
        {
            base.AddTask( task );
            State = ProcessState.Running;
            StartTask( task );
        }

        public override void AddTask( ITask task, int insertIndex )
        {
            base.AddTask( task, insertIndex );

            State = ProcessState.Running;
            StartTask( task );
        }

        protected override void FinishTask( ITask task )
        {
            if( !runningTasks.ContainsKey( task ) ) return;
            runningTasks.Remove( task );
            RemoveTask( task );
            if( runningTasks.Count == 0 ) onAllTasksCompleted.Invoke( this );
        }
    }
}
