using System;

namespace UnityCore.CoroutineSystem
{
    public interface ITimeProvider
    {
        event Action OnUpdate;
    }
}
