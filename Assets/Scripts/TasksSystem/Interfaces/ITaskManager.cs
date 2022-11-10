using System;
using System.Collections.Generic;
using UnityCore.CoroutineSystem;

namespace UnityCore.TaskSystem
{
    public interface ITaskManager : IProgressTracker
    {
        ProcessState State { get; }

        event Action< ITaskManager > OnAllTasksCompleted;

        void AddTask( ITask task );

        void AddTask( ITask task, int insertIndex );

        void AddTasks( params ITask[] queue );

        bool PauseTask( ITask task );

        bool RemoveTask( int taskIndex );

        bool RemoveTask( ITask task );

        void RemoveTasks( IEnumerable< ITask > tasks );

        void Start();

        void Pause();

        void Stop();
    }
}
