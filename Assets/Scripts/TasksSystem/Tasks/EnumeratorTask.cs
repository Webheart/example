using System;
using System.Collections;
using UnityCore.TaskSystem;

namespace Source.UnityCore.StateMachine
{
    public class EnumeratorTask : AbstractTask
    {
        private readonly Func< IEnumerator > getEnumerator;
        private IEnumerator enumerator;

        public EnumeratorTask( Func< IEnumerator > getEnumerator )
        {
            this.getEnumerator = getEnumerator;
        }

        protected override void Enter()
        {
            enumerator = getEnumerator();
        }

        protected override void Exit()
        {
            enumerator = null;
        }

        protected override IEnumerator Execute()
        {
            return enumerator;
        }
    }
}
