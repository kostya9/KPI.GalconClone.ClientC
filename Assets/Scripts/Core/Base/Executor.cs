using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineWorker : MonoBehaviour
{

}

public class Executor : IExecutor
{
    private CoroutineWorker worker;

    public Executor()
    {
        var go = new GameObject("Executor");
        GameObject.DontDestroyOnLoad(go);
        worker = go.AddComponent<CoroutineWorker>();
    }

    public Coroutine Execute(IEnumerator routine)
    {
        return worker.StartCoroutine(routine);
    }

    public void StopExecution(Coroutine coroutine)
    {
        if (coroutine != null)
        {
            worker.StopCoroutine(coroutine);
        }
        else
        {
            Debug.LogError("coroutine is null");
        }
    }
}

public interface IExecutor
{
    Coroutine Execute(IEnumerator routine);
    void StopExecution(Coroutine coroutine);
}
