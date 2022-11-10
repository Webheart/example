using System;
using System.Collections.Generic;
using UnityCore.CoroutineSystem;
using UnityCore.EventSystem;

namespace UnityCore.TaskSystem
{
    public abstract class AbstractTaskManager : ITaskManager
    {
        protected readonly List< ITask > tasks = new List< ITask >();

        protected readonly ICoroutinesManager coroutineManager;

        public ProcessState State { get; protected set; }

        public float Progress { get; private set; }

        public event Action< IProgressTracker > OnProgressChanged
        {
            add => onProgressChanged.Add( value );
            remove => onProgressChanged.Remove( value );
        }
        private readonly Event< IProgressTracker > onProgressChanged = new Event< IProgressTracker >();

        public abstract event Action< ITaskManager > OnAllTasksCompleted;

        protected AbstractTaskManager( ICoroutinesManager coroutineManager )
        {
            this.coroutineManager = coroutineManager;
        }

        public virtual void AddTask( ITask task )
        {
            tasks.Add( task );
            task.OnProgressChanged += OnTaskProgressChangedHandler;
            task.OnCompleted += OnTaskCompletedHandler;
            UpdateProgress();
        }

        public virtual void AddTask( ITask task, int insertIndex )
        {
            tasks.Insert( insertIndex, task );
            task.OnProgressChanged += OnTaskProgressChangedHandler;
            task.OnCompleted += OnTaskCompletedHandler;
            UpdateProgress();
        }

        public void AddTasks( params ITask[] queue )
        {
            foreach( var task in queue )
            {
                AddTask( task );
            }
        }

        public bool RemoveTask( int taskIndex )
        {
            var task = tasks[ taskIndex ];

            return RemoveTask( task );
        }

        public bool RemoveTask( ITask task )
        {
            if( !tasks.Contains( task ) )
            {
                return false;
            }

            tasks.Remove( task );
            task.OnProgressChanged -= OnTaskProgressChangedHandler;
            task.OnCompleted -= OnTaskCompletedHandler;

            StopTask( task );

            UpdateProgress();

            return true;
        }

        public void RemoveTasks( IEnumerable< ITask > tasks )
        {
            foreach( var task in tasks )
            {
                RemoveTask( task );
            }
        }
        
        public abstract bool PauseTask( ITask task );

        public abstract void Start();

        public abstract void Pause();

        public abstract void Stop();

        protected abstract void StartTask( ITask task );

        protected abstract void StopTask( ITask task );

        protected abstract void FinishTask( ITask task );

        private void OnTaskCompletedHandler( ITask task )
        {
            FinishTask( task );
        }

        private void OnTaskProgressChangedHandler( IProgressTracker task )
        {
            UpdateProgress();
        }

        private void UpdateProgress()
        {
            if( tasks.Count == 0 ) Progress = 0;
            else
            {
                var progressSum = 0f;
                foreach( var task in tasks )
                {
                    progressSum += task.Progress;
                }

                Progress = progressSum / tasks.Count;
            }

            onProgressChanged.Invoke( this );
        }
    }
}
