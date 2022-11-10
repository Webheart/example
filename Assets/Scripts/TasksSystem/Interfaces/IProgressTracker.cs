using System;

namespace UnityCore.TaskSystem
{
    public interface IProgressTracker
    {
        float Progress { get; }

        event Action< IProgressTracker > OnProgressChanged;
    }
}
