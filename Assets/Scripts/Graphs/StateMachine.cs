using System;
using System.Collections.Generic;
using System.Linq;
using UnityCore.CoroutineSystem;
using UnityCore.EventSystem;

namespace UnityCore.Graphs
{
    public class StateMachine : IGraph
    {
        public event Action OnStart
        {
            add => onStart.Add( value );
            remove => onStart.Remove( value );
        }

        private readonly Event onStart = new Event();

        protected ICoroutinesManager coroutinesManager;

        private readonly List< Transition > transitions = new List< Transition >();

        private IEnumerable< INode > nodes;

        private INode startNode;
        private INode currentNode;

        public StateMachine( ICoroutinesManager coroutinesManager )
        {
            this.coroutinesManager = coroutinesManager;
        }

        public void SetStartNode( INode startNode )
        {
            this.startNode = startNode;
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

        public void Initialize()
        {
            foreach( var node in nodes )
            {
                node.OnStart += () => currentNode = node;
                node.Initialize();
            }

            if( startNode == null && nodes.Any() ) startNode = nodes.First();
        }

        public void Dispose()
        {
            foreach( var node in nodes ) node.Dispose();
            foreach( var transition in transitions ) transition.Dispose();
            transitions.Clear();
        }

        public void Start()
        {
            if( currentNode == null ) currentNode = startNode;

            onStart.Invoke();

            currentNode?.Start();

            foreach( var transition in transitions ) transition.Start();
        }

        public void Stop()
        {
            foreach( var transition in transitions ) transition.Stop();

            currentNode?.Stop();

            currentNode = null;
        }

        public void Pause()
        {
            foreach( var transition in transitions ) transition.Pause();

            currentNode?.Pause();
        }

        public void SetNodes( IEnumerable< INode > nodes )
        {
            this.nodes = nodes;
        }
    }
}
