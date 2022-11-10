using System;
using System.Collections;

namespace UnityCore.TaskSystem
{
    public class CallbackTask : AbstractTask
    {
        private readonly Action callBack;

        public CallbackTask( Action callback )
        {
            callBack = callback;
        }

        protected override void Enter()
        {
            callBack();
        }

        protected override void Exit() {}

        protected override IEnumerator Execute()
        {
            yield return null;
        }
    }
}
