using System;
using UnityEngine;
using Event = UnityCore.EventSystem.Event;

namespace UnityCore.CoroutineSystem
{
    public class TimeProvider: MonoBehaviour, ITimeProvider
    {
        public static TimeProvider Instance => instance ? instance : instance = new GameObject( "TimeProvider" ).AddComponent< TimeProvider >();

        private static TimeProvider instance;
        
        public event Action OnUpdate
        {
            add => onUpdate.Add( value );
            remove => onUpdate.Remove( value );
        }

        private readonly Event onUpdate = new Event();

        private void Awake() => DontDestroyOnLoad( this );

        private void Update() => onUpdate.Invoke();
    }
}
