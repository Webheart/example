using System.Collections;
using UnityEngine;

namespace UnityCore.CoroutineSystem
{
    public class WaitForSeconds : YieldInstruction
    {
        public readonly float Seconds;

        private float finishTime;

        public WaitForSeconds( float seconds )
        {
            Seconds = seconds;
            finishTime = Time.time + Seconds;
        }

        public override IEnumerator GetEnumerator()
        {
            while( Time.time < finishTime )
            {
                yield return null;
            }
        }
    }
}
