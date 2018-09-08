using RememberPos;
using RememberPos.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginService
{
	public LoginService()
	{
	}

	~LoginService()
	{
	}

	Action<bool> _onLogin;
	public void Login(string accountName, string password, Action<bool> action)
	{
		Debug.Log("[C_Login_Login_0x0101]");
		_onLogin = action;
		var data = new C_Login_Login_0x0101 { _accountName = accountName, _password = password };
		Singleton._serverCallback.AddCallback<S_Login_Login_0x0101>(OnLogin);
		Singleton._messageManager.Send(data);
	}

	void OnLogin(S_Login_Login_0x0101 msg)
	{
		Singleton._serverCallback.RemoveCallback<S_Login_Login_0x0101>(OnLogin);
		Debug.Log("[S_Login_Login_0x0101]" + "rs=" + msg._rs);
		if (_onLogin != null)
		{
			_onLogin(msg._rs == ELoginResult.Success);
		}
	}

	Action<RoomInfo[]> _onGetRooms;
	public void GetRooms(Action<RoomInfo[]> action)
	{
		Debug.Log("[C_Login_GetRooms_0x0102]");
		_onGetRooms = action;
		Singleton._serverCallback.AddCallback<S_Login_GetRooms_0x0102>(OnGetRooms);
		var data = new C_Login_GetRooms_0x0102();
		Singleton._messageManager.Send(data);
	}

	void OnGetRooms(S_Login_GetRooms_0x0102 msg)
	{
		// TODO 需要添加一个更新协议
		Singleton._serverCallback.RemoveCallback<S_Login_GetRooms_0x0102>(OnGetRooms);
		Debug.Log("[S_Login_GetRooms_0x0102]" + "rooms count=" + (msg._rooms == null ? 0 : msg._rooms.Length));
		if (_onGetRooms != null)
		{
			_onGetRooms(msg._rooms); 
		}
	}

	Action<LobbyPlayerData[]> _onCreateRoom;
	public void CreateRoom(string roomName, int mode, Action<LobbyPlayerData[]> action)
	{
		Debug.Log("[C_Login_CreateRoom_0x0103]");
		_onCreateRoom = action;
		var data = new C_Login_CreateRoom_0x0103 { _roomName = roomName, _mode = mode};
		Singleton._serverCallback.AddCallback<S_Login_CreateRoom_0x0103>(OnCreateRoom);
		Singleton._messageManager.Send(data);
	}

	void OnCreateRoom(S_Login_CreateRoom_0x0103 msg)
	{
		Singleton._serverCallback.RemoveCallback<S_Login_CreateRoom_0x0103>(OnCreateRoom);
		Debug.LogFormat("[S_Login_CreateRoom_0x0103] rs={0}, lobbyPlayers count={1}", msg._rs,
			(msg._lobbyPlayerDatas == null ? 0 : msg._lobbyPlayerDatas.Length));
		if (msg._rs && _onCreateRoom != null)
		{
			_onCreateRoom(msg._lobbyPlayerDatas);
		}
	}

	// 人数过多
	// 房间不存在
	// 条件限制
	Action<LobbyPlayerData[]> _onJoinRoom; // 错误码
	public void JoinRoom(string ownerAccountName, Action<LobbyPlayerData[]> action)
	{
		Debug.Log("[C_Login_JoinRoom_0x0104]");
		_onJoinRoom = action;
		var data = new C_Login_JoinRoom_0x0104 { _ownerAccountName = ownerAccountName };
		Singleton._serverCallback.AddCallback<S_Login_JoinRoom_0x0104>(OnJoinRoom);
		Singleton._messageManager.Send(data);
	}

	void OnJoinRoom(S_Login_JoinRoom_0x0104 msg)
	{
		Singleton._serverCallback.RemoveCallback<S_Login_JoinRoom_0x0104>(OnJoinRoom);
		Debug.LogFormat("[S_Login_JoinRoom_0x0104] returnCode={0}, lobbyPlayers count={1}", msg._returnCode,
			(msg._lobbyPlayerDatas == null ? 0 : msg._lobbyPlayerDatas.Length));
		if (msg._returnCode == 0 && _onJoinRoom != null)
		{
			_onJoinRoom(msg._lobbyPlayerDatas);
		}
	}

	Action<int> _onReady; // 错误码
	public void Ready(string ownerAccountName, Action<int> action)
	{
		Debug.Log("[C_Login_Ready_0x0105]");
		_onReady = action;
		var data = new C_Login_Ready_0x0105 { _ownerAccountName = ownerAccountName };
		Singleton._serverCallback.AddCallback<S_Login_Ready_0x0105>(OnReady);
		Singleton._messageManager.Send(data);
	}

	void OnReady(S_Login_Ready_0x0105 msg)
	{
		Singleton._serverCallback.RemoveCallback<S_Login_Ready_0x0105>(OnReady);
		Debug.LogFormat("[S_Login_Ready_0x0105] returnCode={0}", msg._returnCode);
		if (_onReady != null)
		{
			_onReady(msg._returnCode);
		}
	}

	Action<int> _onRegister; // 错误码
	public void Register(string accountName, string password, string nickName, Action<int> action)
	{
		Debug.Log("[C_Login_Register_0x0106]");
		_onRegister = action; 
		var data = new C_Login_Register_0x0106 { _accountName = accountName, _nickName = nickName, _password = password};
		Singleton._serverCallback.AddCallback<S_Login_Register_0x0106>(OnRegister);
		Singleton._messageManager.Send(data);
	}

	void OnRegister(S_Login_Register_0x0106 msg)
	{
		Singleton._serverCallback.RemoveCallback<S_Login_Register_0x0106>(OnRegister);
		Debug.LogFormat("[S_Login_Register_0x0106] returnCode={0}", msg._returnCode);
		if (_onRegister != null)
		{
			_onRegister(msg._returnCode);
		}
	}

	Action<int> _onLogout; // 错误码
	public void Logout(string accountName, Action<int> action)
	{
		Debug.Log("[C_Login_Logout_0x0107]");
		_onLogout = action;
		var data = new C_Login_Logout_0x0107 { _accountName = accountName};
		Singleton._serverCallback.AddCallback<S_Login_Logout_0x0107>(OnLogout);
		Singleton._messageManager.Send(data);
	}

	void OnLogout(S_Login_Logout_0x0107 msg)
	{
		Singleton._serverCallback.RemoveCallback<S_Login_Logout_0x0107>(OnLogout);
		Debug.LogFormat("[S_Login_Logout_0x0107] returnCode={0}", msg._returnCode);
		if (_onLogout != null)
		{
			_onLogout(msg._returnCode);
		}
	}
}
