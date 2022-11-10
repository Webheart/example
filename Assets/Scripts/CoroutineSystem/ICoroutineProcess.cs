using System;

namespace UnityCore.CoroutineSystem
{
    public interface ICoroutineProcess
    {
        bool Completed { get; }

        event Action< ICoroutineProcess > OnCompleted;

        void Pause();

        void Start();

        void Stop();
    }
}
