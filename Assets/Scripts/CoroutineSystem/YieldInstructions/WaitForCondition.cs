using System;
using System.Collections;

namespace UnityCore.CoroutineSystem
{
    // public class WaitForCondition : YieldInstruction
    // {
    //     private readonly Func< bool > check;
    //
    //     public WaitForCondition( Func< bool > check )
    //     {
    //         this.check = check;
    //     }
    //
    //     public override IEnumerator GetEnumerator()
    //     {
    //         if( check != null )
    //         {
    //             while( !check() )
    //             {
    //                 yield return null;
    //             }
    //         }
    //     }
    // }
}
