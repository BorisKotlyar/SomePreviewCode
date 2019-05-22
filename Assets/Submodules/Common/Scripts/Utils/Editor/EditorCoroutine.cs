using UnityEngine;
using UnityEditor;
using System.Collections;

public class EditorCoroutine
{
	private readonly IEnumerator routine;

	private EditorCoroutine(IEnumerator routine)
	{
		this.routine = routine;
	}

	public static EditorCoroutine Start(IEnumerator routine)
	{
		EditorCoroutine coroutine = new EditorCoroutine(routine);
		coroutine.Start();
		return coroutine;
	}

	private void Start()
	{
		EditorApplication.update += Update;
	}

	public void Stop()
	{
		EditorApplication.update -= Update;
	}

	private void Update()
	{
		if(!routine.MoveNext())
		{
			Stop();
		}
	}
}
