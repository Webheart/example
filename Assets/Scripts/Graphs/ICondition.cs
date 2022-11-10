using System;
using UnityCore.EventSystem;

namespace UnityCore.Graphs
{
    public interface ICondition
    {
        event Action OnConditionCompleted;

        bool Completed { get; }

        void Start();

        void Pause();

        void Stop();
    }

    public class Condition : ICondition
    {
        public event Action OnConditionCompleted
        {
            add => onCompleted.Add( value );
            remove => onCompleted.Remove( value );
        }

        protected readonly Event onCompleted = new Event();

        public bool Completed { get; protected set; }

        public virtual void Start() {}

        public virtual void Pause() {}

        public virtual void Stop()
        {
            Completed = false;
        }

        public void Complete()
        {
            Completed = true;
            onCompleted.Invoke();
        }
    }

    public class StateTasksCompletedCondition : ICondition
    {
        public event Action OnConditionCompleted
        {
            add => onCompleted.Add( value );
            remove => onCompleted.Remove( value );
        }

        private readonly Event onCompleted = new Event();

        public bool Completed { get; private set; }

        private bool paused;

        public StateTasksCompletedCondition( State state )
        {
            state.OnTasksCompleted += Complete;
        }

        public void Start()
        {
            paused = false;
        }

        public void Pause()
        {
            paused = true;
        }

        public void Stop()
        {
            Completed = false;
        }

        private void Complete()
        {
            Completed = true;
            if( paused ) return;
            onCompleted.Invoke();
        }
    }
}
