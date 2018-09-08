using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 

public class LoginManager
{
	//public string _accountName;
	//public string _password;
	public event Action onConnectSuccess; 

	public LoginManager()
	{
		Singleton._serverManager.onStatusChanged += OnConnected;
	}

	~LoginManager()
	{
		Singleton._serverManager.onStatusChanged -= OnConnected;
	}

	void OnConnected(StatusCode value)
	{
		if (value == StatusCode.Disconnect)
		{
			OnConnectFailed();
			return;
		}
		else if (value == StatusCode.Connect)
		{
			Debug.LogError("connected to server!");
			//if (_accountName == "")
			//{
			//	Debug.LogError("账户名或密码不能为空！");
			//	return;
			//}
			if (onConnectSuccess != null)
			{
				onConnectSuccess(); 
			}
		}
	}


	void OnConnectFailed()
	{
		Debug.LogError("connect failed! ");
	}
}
