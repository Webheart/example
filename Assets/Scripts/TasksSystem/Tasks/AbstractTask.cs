using System;
using System.Collections;
using UnityCore.CoroutineSystem;
using UnityCore.EventSystem;

namespace UnityCore.TaskSystem
{
    public enum TaskStatus
    {
        Paused,
        Running,
        Completed,
        Stopped,
    }

    public abstract class AbstractTask : ITask
    {
        private float progress;

        public TaskStatus Status { get; private set; }

        public float Progress
        {
            get => progress;
            protected set
            {
                value = value > 1 ? 1 : ( value < 0 ? 0 : value );
                progress = value;
                onProgressChanged.Invoke( this );
            }
        }

        public override string ToString() => GetType().Name;

        public event Action< ITask > OnStarted
        {
            add => onStarted.Add( value );
            remove => onStarted.Remove( value );
        }

        public event Action< ITask > OnCompleted
        {
            add => onCompleted.Add( value );
            remove => onCompleted.Remove( value );
        }

        public event Action< IProgressTracker > OnProgressChanged
        {
            add => onProgressChanged.Add( value );
            remove => onProgressChanged.Remove( value );
        }

        private readonly Event< IProgressTracker > onProgressChanged = new Event< IProgressTracker >();
        private readonly Event< ITask > onStarted = new Event< ITask >();
        private readonly Event< ITask > onCompleted = new Event< ITask >();

        public void Start()
        {
            if( Status == TaskStatus.Completed || Status == TaskStatus.Stopped ) return;
            Status = TaskStatus.Running;
        }

        public void Pause()
        {
            if( Status == TaskStatus.Completed || Status == TaskStatus.Stopped ) return;
            Status = TaskStatus.Paused;
        }

        public void Stop()
        {
            if( Status == TaskStatus.Completed ) return;
            Status = TaskStatus.Stopped;
            Exit();
        }

        public IEnumerator GetCoroutine()
        {
            onStarted.Invoke( this );

            Progress = 0;
            Status = TaskStatus.Running;
            Enter();

            yield return new YieldInstruction( Execute() );

            Exit();
            Status = TaskStatus.Completed;
            Progress = 1;

            onCompleted.Invoke( this );
        }

        protected abstract void Enter();

        protected abstract void Exit();

        protected abstract IEnumerator Execute();
    }
}
