//using System.Collections;
//using System.Collections.Generic;
//using System;
//using MySql.Data.MySqlClient;
//using UnityEngine;
//using System.Net.Sockets;
//using UnityEngine.Networking;

//public class DataServer
//{
//	bool isStarted; 
//	public void StartClient(string accountName)
//	{
//		//Debug.LogError("accountName=" + accountName);
//		//NetUtility.Instance.SetDelegate((string temp) =>
//		//{
//		//	Debug.Log("[Client] " + temp);
//		//});

//		//Loom.RunAsync(() =>
//		//{
//		//	NetUtility.Instance._onReceive = OnReceive;
//		//	if(!isStarted)
//		//	{
//		//		NetUtility.Instance.ClientConnnect();
//		//	}
//		//	Debug.Log("[Client] SendMsg " + accountName);
//		//	NetUtility.Instance.SendMsg<C_LoginMessage>(new C_LoginMessage { _accountName = accountName }, C_LoginMessage._msgId, NetUtility.Instance.tcpClient);
//		//});
//		// TODO 是否已经使用了这个帐号？？？
//	}

//	void OnReceive(byte[] t, int id, TcpClient client)
//	{
//		Debug.Log("msgId=0x" + Convert.ToString(id, 16));
//		if (id == S_LoginMessage._msgId)
//		{
//			OnClientConnect(t);
//		}
//		else if (id == S_LogoutMessage._msgId)
//		{
//			OnClientDisconnect(t);
//		}
//		else if (id == S_GetRoomListMessage._msgId)
//		{
//			OnGetRoomList(t);
//		}
//	}

//	void OnClientDisconnect(byte[] t)
//	{
//		NetUtility.Instance.ClientDisconnect(); 
//		Debug.LogError("" + LogUtil.GetCurMethodName()); 
//		Loom.QueueOnMainThread(() =>
//		{
//			if (NetworkClient.active)
//			{
//				SingletonLobby._lobbyManager.StopClient();
//			}
//			else
//			{
//				SingletonLobby._lobbyManager.OnLobbyStopClient();
//			}
//			NetUtility.Instance._hasLogin = false;
//			SingletonLobby._uiManager.Open(EView.Login);
//		});
//	}

//	void OnClientConnect(byte[] t)
//	{
//		//isStarted = true; 
//		//S_LoginMessage resultModel = NetUtility.Instance.DeSerialize<S_LoginMessage>(t);
//		//var temp = NetUtility.Instance.DeSerialize<PlayerData>(resultModel._playerData);
//		//Debug.Log("[Client] " + (temp == null ? "null" : temp._nickName));
//		//if (temp == null || temp._accountName == "")
//		//{
//		//	NetUtility.Instance._hasLogin = false;
//		//}
//		//else
//		//{
//		//	Debug.LogError("hasLogin=" + NetUtility.Instance._hasLogin);
//		//	NetUtility.Instance._hasLogin = true;
//		//}

//		//Loom.QueueOnMainThread(() =>
//		//{
//		//	if (temp == null || temp._accountName == "")
//		//	{
//		//		var view = SingletonLobby._uiManager.Open<MessageView>(EView.Message);
//		//		view.message = "登录失败！请检查帐号是否存在。";
//		//		return;
//		//	}
//		//	SingletonLobby._uiManager.Close(EView.Login);
//		//	SingletonLobby._uiManager.Open(EView.Lobby);
//		//	SingletonLobby._localPlayerData = temp;
//		//});
//	}

//	void OnGetRoomList(byte[] t)
//	{
//		//S_GetRoomListMessage msg = NetUtility.Instance.DeSerialize<S_GetRoomListMessage>(t);
//		//Debug.Log("获取房间列表, 数量=" + msg._list.Count);
//		var list = new List<RoomInfo>();
//		//for (int i = 0, length = msg._list.Count; i < length; i++)
//		//{
//		//	var r = msg._list[i];
//		//	list.Add(NetUtility.Instance.DeSerialize<RoomInfo>(r));
//		//}
//		var v = SingletonLobby._uiManager.Get<LobbyView>(EView.Lobby);
//		//v.OnReceive(list.ToArray()); 
//	}

//	// TODO 根据账户名获取其他帐号的数据
//	public void GetPlayerData(string accountName, Action<PlayerData> action)
//	{
//		if (action == null)
//		{
//			return;
//		}

//		// 查看数据库是否有对应数据
//		PlayerData data = new PlayerData(); 
//		SingletonLobby._sql.CreateTable(Player._tableName, data.ToColumnNames(), data.ToClumnTypes());
//		int count = SingletonLobby._sql.GetCount(Player._tableName,
//			new MySqlParameter("?" + SqlUtil.GetVarName(e => data._accountName), accountName));
//		if (count == 0)
//		{
//			Debug.LogError("登录失败！请检查帐号是否存在。");
//			action(null);
//			return;
//		}

//		// 看看表中是否是空数据
//		var list = SingletonLobby._sql.ReaderInfo(Player._tableName, data.ToColumnNames(),
//			new MySqlParameter("?" + SqlUtil.GetVarName(e => data._accountName), accountName));
//		if (list.Count == 0)
//		{
//			Debug.LogError("获取的数据长度为0！");
//			action(null);
//			return;
//		}
//		data.SetAllValues(list[0].ToArray());
//		action(data);
//	}

//	public void CreateNewAccount(PlayerData data, Action<bool> action)
//	{
//		if (action == null)
//		{
//			return;
//		}
//		//NetUtility.Instance.SetDelegate((string msg) =>
//		//{
//		//	Debug.Log(msg + "\r\n");
//		//});
//		////连接服务器
//		//NetUtility.Instance.ClientConnnect();

//		//SingletonLobby._sql.CreateTable(Player._tableName, data.ToColumnNames(), data.ToClumnTypes());
//		//SingletonLobby._sql.UpdateInto(Player._tableName,
//		//	new MySqlParameter("?" + SqlUtil.GetVarName(e => data._accountName), data._accountName),
//		//	data.ToSqlParamsWithoutID());

//		action(true);
//	}

//	public void GetRoomInfos()
//	{
//		//KBEngine.Event.fireIn("reqRooms");
//	}

//	public void CreateRoom(RoomInfo info)
//	{
//		if(info == null)
//		{
//			Debug.LogError("info is empty! ");
//			return; 
//		}
//		//KBEngine.Event.fireIn("addRoom", new object[] { info });
//	}

//	public void DestroyRoom(RoomInfo info)
//	{
//		if (info == null)
//		{
//			Debug.LogError("info is empty! ");
//			return;
//		}
//		//KBEngine.Event.fireIn("removeRoom", new object[] { info });
//	}
//}
