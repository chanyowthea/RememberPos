using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using ExitGames.Client.Photon;

public class LoginView : BaseView
{
	[SerializeField] InputField _accountNameInput;
	[SerializeField] Dropdown _dropDown;
	string[] _servers;
	public string[] servers
	{
		get
		{
			return _servers;
		}
		set
		{
			if (value == null)
			{
				return;
			}
			_dropDown.ClearOptions();
			_servers = value;
			List<Dropdown.OptionData> list = new List<Dropdown.OptionData>();
			for (int i = 0, length = value.Length; i < length; i++)
			{
				list.Add(new Dropdown.OptionData { text = value[i] });
			}
			_dropDown.AddOptions(list);
		}
	}

	public override void Open()
	{
		servers = new string[] { "39.106.31.37（外网）" };
		base.Open();
	}

	public override void Close()
	{
		base.Close();
	}

	void Login()
	{
		Singleton._loginManager.onConnectSuccess -= Login;
		Singleton._loginService.Login(_accountNameInput.text, "1", OnLogin);
	}

	void OnLogin(bool value)
	{
		if (!value)
		{
			Debug.LogError("登录失败！");
		}
		else
		{
			Singleton._accountName = _accountNameInput.text;
			Singleton._uiManager.Open(EView.Lobby);
		}
	}

	// 登录
	public override void OnClick0()
	{
		if (string.IsNullOrEmpty(_accountNameInput.text))
		{
			var view = ServerManager.Instance._uiManager.Open<MessageView>(EView.Message);
			view.message = "账户名不能为空！";
			return;
		}
		base.OnClick0();
		Singleton._loginManager.onConnectSuccess += Login;
		Singleton._serverManager.Connect();
	}

	public override void OnClick1()
	{
		base.OnClick1();
		Singleton._uiManager.Open(EView.Register);
	}
}
