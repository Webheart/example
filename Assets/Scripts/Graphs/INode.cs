using System;
using System.Collections.Generic;

namespace UnityCore.Graphs
{
    public interface INode : IInitializable, IDisposable
    {
        event Action OnStart;
        
        void AddTransition( INode toNode, ICondition[] conditions, bool checkAll = true );

        void AddTransition( INode toNode, ICondition condition, bool checkAll = true );

        void Start();

        void Pause();

        void Stop();
    }
}
