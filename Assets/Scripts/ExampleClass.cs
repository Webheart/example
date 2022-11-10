using System.Collections;
using UnityCore.Aggregators;
using UnityCore.CoroutineSystem;
using UnityCore.TaskSystem;
using UnityEngine;

public class ExampleClass
{
    private ParallelTaskManager parallelTaskManager;
    private SequencedTaskManager sequencedTaskManager;
    private Processor processor;

    public ExampleClass( ICoroutinesManager coroutinesManager )
    {
        parallelTaskManager = new ParallelTaskManager( coroutinesManager );
        sequencedTaskManager = new SequencedTaskManager( coroutinesManager );
        processor = new Processor( coroutinesManager );
    }

    public void Initialize()
    {
        var task1 = new ExampleTask( "first task", 1 );
        var task2 = new ExampleTask( "second task", 1 );
        parallelTaskManager.AddTasks( task1, task2 );

        var task3 = new ExampleTask( "first task", 1 );
        var task4 = new ExampleTask( "second task", 1 );
        sequencedTaskManager.AddTasks( task3, task4 );
    }

    public void StartParallelTasks()
    {
        parallelTaskManager.Start();
    }

    public void StartSequencedTask()
    {
        sequencedTaskManager.Start();
    }

    public void StartCoroutine()
    {
        processor.StartCoroutine( ExampleCoroutine() );
    }

    IEnumerator ExampleCoroutine()
    {
        Debug.Log( "Start coroutine" );
        yield return null;
        Debug.Log( "Finish coroutine" );
    }
}
