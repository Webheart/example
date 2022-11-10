using System.Collections;
using UnityCore.TaskSystem;
using UnityEngine;

public class ExampleTask : AbstractTask
{
    private string name;
    private float duration;

    public ExampleTask( string name, float duration )
    {
        this.name = name;
        this.duration = duration;
    }

    protected override void Enter()
    {
        Debug.Log( $"Start {name}" );
    }

    protected override void Exit()
    {
        Debug.Log( $"Finish {name}" );
    }

    protected override IEnumerator Execute()
    {
        yield return new UnityCore.CoroutineSystem.WaitForSeconds( duration );
    }
}
