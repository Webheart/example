using System;
using System.Collections;

namespace UnityCore.CoroutineSystem
{
    public class YieldInstruction : IEnumerable
    {
        private readonly IEnumerator enumerator;

        public YieldInstruction( IEnumerator enumerator )
        {
            this.enumerator = enumerator;
        }
        
        protected YieldInstruction(){}

        public virtual IEnumerator GetEnumerator() => enumerator;
        
        IEnumerator IEnumerable.GetEnumerator() => enumerator;
    }
}
