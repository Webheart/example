using System.Collections;
using System.Collections.Generic;
using UnityCore.CoroutineSystem;
using UnityCore.TaskSystem;

namespace UnityCore.Aggregators
{
    public class Processor : IProcessor
    {
        private readonly List< ICoroutineProcess > coroutines = new List< ICoroutineProcess >();

        private readonly List< ITaskManager > runningTaskManagers = new List< ITaskManager >();

        private readonly SimpleTaskManager simpleTaskManager;

        private readonly ICoroutinesManager coroutineManager;

        public Processor( ICoroutinesManager coroutineManager )
        {
            this.coroutineManager = coroutineManager;
            simpleTaskManager = new SimpleTaskManager( coroutineManager );
            simpleTaskManager.Start();
        }

        public void Dispose()
        {
            foreach( var coroutineProcessor in coroutines )
            {
                coroutineProcessor.OnCompleted -= OnCoroutineCompletedHandler;
                coroutineProcessor.Stop();
            }

            foreach( var taskManager in runningTaskManagers )
                taskManager.OnAllTasksCompleted -= OnAllTasksInManagerCompleted;
            runningTaskManagers.Clear();
            coroutines.Clear();
            simpleTaskManager.Stop();
        }

        public void RunTasks( TaskRunningMode mode, params AbstractTask[] tasks )
        {
            var taskManager = CreateTaskManager( coroutineManager, mode );
            runningTaskManagers.Add( taskManager );
            taskManager.OnAllTasksCompleted += OnAllTasksInManagerCompleted;

            taskManager.AddTasks( tasks );
            taskManager.Start();
        }

        public void RunTask( AbstractTask task )
        {
            simpleTaskManager.AddTask( task );
        }

        public void PauseTask( AbstractTask task ) => simpleTaskManager.PauseTask( task );
        
        public void StopTask( AbstractTask task ) => simpleTaskManager.RemoveTask( task );

        public ICoroutineProcess StartCoroutine( IEnumerator enumerator )
        {
            var process = coroutineManager.StartCoroutine( enumerator );
            process.OnCompleted += OnCoroutineCompletedHandler;
            coroutines.Add( process );
            return process;
        }

        public bool StopCoroutine( ICoroutineProcess coroutine )
        {
            coroutine.Stop();
            return coroutines.Remove( coroutine );
        }

        public ITaskManager GetTaskManager( TaskRunningMode mode )
        {
            var taskManager = CreateTaskManager( coroutineManager, mode );
            return taskManager;
        }

        public void DisposeTaskManager( ITaskManager taskManager )
        {
            taskManager.Stop();
        }

        private void OnAllTasksInManagerCompleted( ITaskManager taskManager )
        {
            taskManager.OnAllTasksCompleted -= OnAllTasksInManagerCompleted;
            runningTaskManagers.Remove( taskManager );
            taskManager.Stop();
        }

        private void OnCoroutineCompletedHandler( ICoroutineProcess coroutine )
        {
            coroutine.OnCompleted -= OnCoroutineCompletedHandler;
            StopCoroutine( coroutine );
        }
        
        ITaskManager CreateTaskManager( ICoroutinesManager coroutineManager, TaskRunningMode mode )
        {
            switch( mode )
            {
                case TaskRunningMode.Parallel:
                    return new ParallelTaskManager( coroutineManager );
                case TaskRunningMode.Sequenced:
                    return new SequencedTaskManager( coroutineManager );
            }

            return null;
        }
    }
}
