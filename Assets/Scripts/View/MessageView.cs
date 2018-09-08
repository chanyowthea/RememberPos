using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class MessageView : BaseView
{
	[SerializeField] Text _msgText; 

	public string message
	{
		set
		{ 
			_msgText.text = value; 
		}
		get
		{ 
			return _msgText.text; 
		}
	}

	public override void Open()
	{
		base.Open();
		Invoke("CloseEx", 2);
	}

	public override void Close()
	{
		// 重新打开的时候，执行时间可能不对，因此在这里取消
		CancelInvoke("CloseEx");
		base.Close();
	}

	void CloseEx()
	{
		UnityEngine.Debug.Log("MessageView " + LogUtil.GetCurMethodName());
		Close(); 
	}
}
