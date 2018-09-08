using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System; 
using UnityEngine.Networking; 

public class LogView : BaseView
{
	[SerializeField] Text _logText; 
	string _log; 

	public override void Close()
	{
		base.Close();
	}

	public void Add(string s)
	{
		_log += s + "\n"; 
		_logText.text = _log; 
	}

	public void Clear()
	{
		_logText.text = null; 
		_log = null; 
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Backspace))
		{
			Clear(); 
		}
	}
}
