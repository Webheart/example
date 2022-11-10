using System;
using System.Collections.Generic;
using UnityCore.EventSystem;
using UnityCore.TaskSystem;

namespace UnityCore.Graphs
{
    public class State : INode
    {
        public event Action OnStart
        {
            add => onStart.Add( value );
            remove => onStart.Remove( value );
        }

        private readonly Event onStart = new Event();

        public event Action OnTasksCompleted
        {
            add => onTasksCompleted.Add( value );
            remove => onTasksCompleted.Remove( value );
        }

        private readonly Event onTasksCompleted = new Event();

        private readonly ITaskManager taskManager;

        private readonly List< Transition > transitions = new List< Transition >();

        public State( ITaskManager taskManager )
        {
            this.taskManager = taskManager;
        }

        public void Initialize()
        {
            taskManager.OnAllTasksCompleted += OnAllTasksCompletedHandler;
        }

        public void Dispose()
        {
            taskManager.OnAllTasksCompleted -= OnAllTasksCompletedHandler;

            foreach( var transition in transitions ) transition.Dispose();
            transitions.Clear();
        }

        public void AddTasks( params AbstractTask[] tasks )
        {
            taskManager.AddTasks( tasks );
        }

        public void AddTask( AbstractTask task )
        {
            taskManager.AddTask( task );
        }

        public void AddTransition( INode toNode, ICondition[] conditions, bool checkAll = true )
        {
            var transition = new Transition( this, toNode, conditions, checkAll );
            transition.Initialize();
            transitions.Add( transition );
        }

        public void AddTransition( INode toNode, ICondition condition, bool checkAll = true )
        {
            AddTransition( toNode, new[] { condition }, checkAll );
        }

        public void Start()
        {
            onStart.Invoke();

            taskManager.Start();
            foreach( var transition in transitions ) transition.Start();
        }

        public void Stop()
        {
            foreach( var transition in transitions ) transition.Stop();
            taskManager.Stop();
        }

        public void Pause()
        {
            foreach( var transition in transitions ) transition.Pause();
            taskManager.Pause();
        }

        private void OnAllTasksCompletedHandler( ITaskManager taskManager )
        {
            onTasksCompleted.Invoke();
        }
    }
}
