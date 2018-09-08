using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RegisterView : BaseView
{
	[SerializeField] InputField _accountNameInput;
	[SerializeField] InputField _nickNameInput;

	public override void Open()
	{
		base.Open();
	}

	public override void Close()
	{
		base.Close();
	}

	// 注册并登录
	public override void OnClick0()
	{
		if (string.IsNullOrEmpty(_accountNameInput.text))
		{
			var view = ServerManager.Instance._uiManager.Open<MessageView>(EView.Message);
			view.message = "账户名不能为空！";
			return;
		}

		if (string.IsNullOrEmpty(_nickNameInput.text))
		{
			var view = ServerManager.Instance._uiManager.Open<MessageView>(EView.Message);
			view.message = "昵称不能为空！";
			return;
		}

		base.OnClick0();

		Singleton._loginManager.onConnectSuccess += Register;
		Singleton._serverManager.Connect();
	}

	void Register()
	{
		Singleton._loginManager.onConnectSuccess -= Register;
		Singleton._loginService.Register(_accountNameInput.text, _accountNameInput.text == "2" ? "2" : "1", 
			_nickNameInput.text, OnRegister);
	}

	void OnRegister(int code)
	{
		if (code != 0)
		{
			Debug.LogError("注册失败！code=" + code);
		}
		else
		{
			Singleton._accountName = _accountNameInput.text;
			Singleton._uiManager.Open(EView.Lobby);
		}
	}

	public override void OnClick1()
	{
		base.OnClick1();
	}
}
