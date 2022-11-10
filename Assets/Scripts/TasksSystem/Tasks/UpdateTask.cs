using System;
using System.Collections;

namespace UnityCore.TaskSystem
{
    public class UpdateTask : AbstractTask
    {
        private readonly Action callBack;

        public UpdateTask( Action callback )
        {
            callBack = callback;
        }

        protected override void Enter() {}

        protected override void Exit() {}

        protected override IEnumerator Execute()
        {
            while( true )
            {
                callBack();
                yield return null;
            }
        }
    }
}
