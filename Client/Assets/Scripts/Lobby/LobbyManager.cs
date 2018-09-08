//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Networking;

//public class LobbyManager : NetworkLobbyManager
//{
//	public static LobbyManager _lobbyManager;
//	[SerializeField] public LobbyReference _lobbyReference;
//	Dictionary<NetworkConnection, string> _lobbyAccounts = new Dictionary<NetworkConnection, string>();

//	#region Client
//	private void Start()
//	{
//		//Debug.LogError(1 << 3);

//		_lobbyManager = this;
//		SingletonLobby.Init();
//		SingletonLobby._uiManager.Init();
//		LogUtil.Init();
//		SingletonLobby._uiManager.Open(EView.Login);
//		SingletonLobby._uiManager.Open(EView.Fixed);
//	}

//	public override void OnClientConnect(NetworkConnection conn)
//	{
//		NetworkServer.RegisterHandler(C_EnterLobbyMessage._msgId, OnServerEnterLobby);
//		//LogUtil.Write(LogUtil.GetCurMethodName() + " C_EnterLobbyMessage conn=" + conn);
//		conn.Send(C_EnterLobbyMessage._msgId,
//			new C_EnterLobbyMessage { _accountName = SingletonLobby._localPlayerData._accountName });
//		base.OnClientConnect(conn);
//	}

//	public override void OnClientDisconnect(NetworkConnection conn)
//	{
//		base.OnClientDisconnect(conn);
//	}

//	public override void OnLobbyStopClient()
//	{
//		SingletonLobby._uiManager.Open(EView.Lobby);
//		SingletonLobby._uiManager.Open(EView.Fixed);
//		base.OnLobbyStopClient();
//	}

//	// 所有玩家准备好，开始游戏，切换场景
//	public override void OnClientSceneChanged(NetworkConnection conn)
//	{
//		base.OnClientSceneChanged(conn);
//		SingletonLobby._uiManager.Close(EView.Fixed);

//		// 如果结束游戏，那么服务器会自动销毁该帐号对应的房间
//		//SingletonLobby._dataServer.DestroyRoom(new RoomInfo
//		//{
//		//	_roomName = SingletonLobby._localPlayerData._accountName,
//		//	_ipAddress = SqlUtil._GetLocalIP()
//		//});
//	}

//	public override void OnStopClient()
//	{
//		//SingletonLobby._discovery.StopClient();
//		//SingletonLobby._discovery.showGUI = true;
//		base.OnStopClient();
//	}

//	#endregion

//	#region Server
//	public override void OnStartHost()
//	{
//		// 初始化并发出广播，让其他玩家可以搜索到这个游戏
//		//SingletonLobby._discovery.Initialize();
//		base.OnStartHost();
//		//SingletonLobby._discovery.StartServer();
//	}

//	public override void OnLobbyServerPlayersReady()
//	{
//		bool allready = true;
//		for (int i = 0; i < lobbySlots.Length; ++i)
//		{
//			if (lobbySlots[i] != null)
//				allready &= lobbySlots[i].readyToBegin;
//		}

//		LogUtil.Write("OnLobbyServerPlayersReady");
//		if (allready)
//		{
//			Debug.LogError("all ready! " + playScene);
//			LogUtil.Write("all ready! " + playScene); 
//			SingletonLobby._uiManager.Close(EView.Ready);
//			ServerChangeScene(playScene);
//		}
//	}

//	public override void OnStartServer()
//	{
//		base.OnStartServer();
//	}

//	public override void OnStopServer()
//	{
//		//SingletonLobby._discovery.StopServer();
//		_lobbyAccounts.Clear();
//		base.OnStopServer();
//	}

//	public override GameObject OnLobbyServerCreateLobbyPlayer(NetworkConnection conn, short playerControllerId)
//	{
//		//if(_lobbyAccounts.Count >= 2)
//		//{
//		//	// 只有前面两个加入的才能进入对战，其他的是观战
//		//	return null; 
//		//}
//		var player = GameObject.Instantiate(SingletonLobby._lobbyReference._lobbyPlayerPrefab);
//		player.accountName = _lobbyAccounts[conn];
//		return player.gameObject;
//	}

//	public override void OnServerDisconnect(NetworkConnection conn)
//	{
//		_lobbyAccounts.Remove(conn);
//		base.OnServerDisconnect(conn);
//	}

//	public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer)
//	{
//		var lobby = lobbyPlayer.GetComponent<LobbyPlayer>();
//		Player player = gamePlayer.GetComponent<Player>();
//		player._accountName = lobby.accountName;
//		return true;
//	}
//	#endregion

//	#region Server Callback
//	void OnServerEnterLobby(NetworkMessage msg)
//	{
//		LogUtil.Write(LogUtil.GetCurMethodName() + " C_EnterLobbyMessage conn=" + msg.conn);
//		var temp = msg.ReadMessage<C_EnterLobbyMessage>();
//		_lobbyAccounts[msg.conn] = temp._accountName;
//	}
//	#endregion



//	#region Client Callback



//	#endregion

//	bool _isOpenLog;
//	void Update()
//	{
//		if (Input.GetKeyDown(KeyCode.L))
//		{
//			if (!_isOpenLog)
//			{
//				SingletonLobby._uiManager.Open(EView.Log);
//			}
//			else
//			{
//				SingletonLobby._uiManager.Close(EView.Log);
//			}
//			_isOpenLog = !_isOpenLog;
//		}
//	}

//	private void OnApplicationQuit()
//	{
//		//Debug.LogError("" + LogUtil.GetCurMethodName() + " hasLogin=" + NetUtility.Instance._hasLogin); 
//		//if (NetUtility.Instance._hasLogin)
//		//{
//		//	NetUtility.Instance._hasLogin = false; 
//		//	NetUtility.Instance.SendMsg<C_LogoutMessage>(new C_LogoutMessage { _accountName = SingletonLobby._localPlayerData._accountName }, C_LogoutMessage._msgId, NetUtility.Instance.tcpClient);
//		//}
//	}
//}
