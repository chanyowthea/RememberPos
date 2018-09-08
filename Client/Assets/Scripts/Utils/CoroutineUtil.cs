using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System; 

public class CoroutineUtil : MonoBehaviour
{
	#region Main
	static CoroutineUtil _instance; 
	static List<IEnumerator> _routines; 

	void Start()
	{
		_instance = this; 
		_routines = new List<IEnumerator>(); 
	}

	void OnDestroy()
	{
		StopAll();
	}

	public static void Start(IEnumerator routine)
	{
		if (routine == null)
		{
			return; 
		}
		Add(routine); 
	}

	public static void Stop(IEnumerator routine)
	{
		Remove(routine); 
	}

	public static void StopAll()
	{
		if(_routines == null)
		{
			return; 
		}

		for (int i = _routines.Count - 1; i >= 0; --i)
		{
			_instance.StopCoroutine(_routines[i]); 
		}
		_routines.Clear(); 
	}

	public static void WaitSeconds(Action action, float seconds)
	{
		if (action == null)
		{
			return; 
		}
		_instance.StartCoroutine(WaitRoutine(action, seconds)); 
	}
	#endregion

	static IEnumerator WaitRoutine(Action action, float time)
	{
		if (action == null)
		{
			yield break; 
		}
		yield return new WaitForSeconds(time); 
		action(); 
	}

	static void Remove(IEnumerator value)
	{
		if (value == null)
		{
			return; 
		}
		if (_routines != null)
		{
			_routines.Remove(value);
		}
		if (_instance != null)
		{
			_instance.StopCoroutine(value);
		}
	}

	static void Add(IEnumerator value)
	{
		if (value == null)
		{
			return; 
		}
		_routines.Add(value); 
		_instance.StartCoroutine(value); 
	}
}

