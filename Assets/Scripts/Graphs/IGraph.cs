using System.Collections.Generic;

namespace UnityCore.Graphs
{
    public interface IGraph : INode
    {
        void SetNodes( IEnumerable< INode > nodes );
    }
}
