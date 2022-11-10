using System;
using System.Collections;
using System.Collections.Generic;
using UnityCore.CoroutineSystem;
using UnityCore.TaskSystem;

namespace UnityCore.Aggregators
{
    public interface IProcessor : IDisposable
    {
        ICoroutineProcess StartCoroutine( IEnumerator enumerator );

        bool StopCoroutine( ICoroutineProcess processor );

        ITaskManager GetTaskManager( TaskRunningMode mode );

        void DisposeTaskManager( ITaskManager taskManager );

        void RunTask( AbstractTask task );

        void RunTasks( TaskRunningMode mode, params AbstractTask[] tasks );
    }
}
