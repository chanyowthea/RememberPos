using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogUtil : MonoBehaviour
{
    public const string RED = "<color=red>{0}</color>";
    public const string TEAL = "<color=teal>{0}</color>";
    public const string GREEN = "<color=green>{0}</color>";
    public const string WHITE = "<color=white>{0}</color>";
    public const string BLACK = "{0}";
    public const string NONE = "{0}";
    public static LogView _logView;
    public static MessageView _msgView;

    public static void Init()
    {
        _logView = ServerManager.Instance._uiManager.Get<LogView>(EView.Log);
        _msgView = ServerManager.Instance._uiManager.Get<MessageView>(EView.Message);
    }

    public static void Write(string s, string color = LogUtil.TEAL)
    {
		_logView.Add(string.Format(color, s));
		Debug.Log(string.Format(color, s)); 
    }

    public static void Popup(string s, string color = LogUtil.BLACK)
    {
        _msgView.message = string.Format(color, s);
        _msgView.Open(); 
    }

	public static string GetCurMethodName()
	{
		return new System.Diagnostics.StackFrame(1).GetMethod().Name; 
	}
}
