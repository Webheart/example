using System;
using System.Collections.Generic;

namespace UnityCore.Graphs
{
    public class Transition : IInitializable, IDisposable
    {
        public readonly INode FromNode;
        public readonly INode ToNode;

        private readonly ICondition[] conditions;
        private readonly bool checkAll;

        private State state;

        public Transition( INode fromNode, INode toNode, ICondition[] conditions, bool checkAll )
        {
            FromNode = fromNode;
            ToNode = toNode;
            this.conditions = conditions;
            this.checkAll = checkAll;
        }

        public void Start()
        {
            state = State.Running;
            for( var index = 0; index < conditions.Length; index++ )
            {
                conditions[ index ].Start();
            }

            CheckConditions();
        }

        public void Stop()
        {
            for( var index = 0; index < conditions.Length; index++ )
            {
                conditions[ index ].Stop();
            }

            state = State.Stopped;
        }

        public void Pause()
        {
            if( state != State.Running ) return;

            for( var index = 0; index < conditions.Length; index++ )
            {
                conditions[ index ].Pause();
            }

            state = State.Paused;
        }

        public void Initialize()
        {
            for( var index = 0; index < conditions.Length; index++ )
            {
                conditions[ index ].OnConditionCompleted += CheckConditions;
            }
        }

        public void Dispose()
        {
            for( var index = 0; index < conditions.Length; index++ )
            {
                conditions[ index ].OnConditionCompleted -= CheckConditions;
            }
        }

        private void CheckConditions()
        {
            if( state != State.Running ) return;

            if( checkAll )
            {
                for( var index = 0; index < conditions.Length; index++ )
                {
                    if( !conditions[ index ].Completed ) return;
                }

                SwitchNodes();
            }
            else
            {
                for( var index = 0; index < conditions.Length; index++ )
                {
                    if( !conditions[ index ].Completed ) continue;
                    SwitchNodes();
                    return;
                }
            }
        }

        private void SwitchNodes()
        {
            FromNode.Stop();
            ToNode.Start();
        }

        private enum State : byte
        {
            NotStarted,
            Running,
            Stopped,
            Paused,
        }
    }
}
