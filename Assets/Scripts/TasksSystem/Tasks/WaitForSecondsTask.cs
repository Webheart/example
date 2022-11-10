using System.Collections;
using UnityCore.TaskSystem;
using UnityEngine;

namespace Source.UnityCore.StateMachine
{
    public class WaitForSecondsTask : AbstractTask
    {
        private readonly float time;

        public WaitForSecondsTask( float time )
        {
            this.time = time;
        }

        protected override void Enter() {}

        protected override void Exit() {}

        protected override IEnumerator Execute()
        {
            yield return new WaitForSeconds( time );
        }
    }
}
