using System.Collections;
using UnityEngine.SceneManagement;

namespace UnityCore.TaskSystem
{
    public class LoadSceneTask : AbstractTask
    {
        private readonly string name;
        private readonly LoadSceneMode mode;

        public LoadSceneTask( string name, LoadSceneMode mode )
        {
            this.name = name;
            this.mode = mode;
        }

        protected override void Enter() {}

        protected override void Exit() {}

        protected override IEnumerator Execute()
        {
            var o = SceneManager.LoadSceneAsync( name, mode );
            o.allowSceneActivation = true;
            while( !o.isDone )
            {
                Progress = o.progress;
                yield return null;
            }
        }
    }

    public class UnloadSceneTask : AbstractTask
    {
        private readonly string name;

        public UnloadSceneTask( string name )
        {
            this.name = name;
        }

        protected override void Enter() {}

        protected override void Exit() {}

        protected override IEnumerator Execute()
        {
            var o = SceneManager.UnloadSceneAsync( name );
            while( !o.isDone )
            {
                Progress = o.progress;
                yield return null;
            }
        }
    }
}
