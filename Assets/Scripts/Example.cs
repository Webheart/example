using System.Collections.Generic;
using UnityCore.CoroutineSystem;
using UnityEngine;

public class Example : MonoBehaviour
{
    private ExampleClass exampleClass;

    // Start is called before the first frame update
    void Start()
    {
        var coroutinesManager = new CoroutinesManager( TimeProvider.Instance );
        exampleClass = new ExampleClass( coroutinesManager );
        exampleClass.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        if( Input.GetKeyDown( KeyCode.Alpha1 ) ) exampleClass.StartParallelTasks();
        if( Input.GetKeyDown( KeyCode.Alpha2 ) ) exampleClass.StartSequencedTask();
        if( Input.GetKeyDown( KeyCode.Alpha3 ) ) exampleClass.StartCoroutine();
    }
}