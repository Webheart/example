using System;
using System.Collections;

namespace UnityCore.TaskSystem
{
    public interface ITask : IProgressTracker
    {
        TaskStatus Status { get; }

        event Action< ITask > OnStarted;

        event Action< ITask > OnCompleted;

        IEnumerator GetCoroutine();

        void Start();

        void Pause();

        void Stop();
    }
}
