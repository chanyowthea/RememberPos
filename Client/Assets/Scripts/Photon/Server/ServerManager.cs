using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using System;
using RememberPos.Message;
using RememberPos;
using RememberPos.Utils; 

public class ServerManager : MonoBehaviour, IPhotonPeerListener
{
	#region Lobby
	public ViewLibrary _viewLib;
	public UIManager _uiManager;

	#endregion


	private static ServerManager _instance = null;
	public static ServerManager Instance { get { return _instance; } }

	public PhotonPeer peer { private set; get; }
	private ConnectionProtocol protocol = ConnectionProtocol.Tcp; // 默认使用udp协议
	public string serverAddress = "39.106.31.37:5055"; // 连接本机ip，端口5055 // "127.0.0.1:5055"
	public string applicationName = "RememberPos"; // 连接名称

	public event Action<StatusCode> onStatusChanged;
	public bool connected; 

	void Awake()
	{
		_instance = this;
	}

	void Start()
	{
		Debug.LogError("localip=" + Network.player.ipAddress); 
		Singleton.Init();
		_uiManager.Init();
		LogUtil.Init();
		_uiManager.Open(EView.Login);
        Screen.autorotateToPortrait = false; 
		//Connect(); 
	}

	//停止客户端时，与服务器断开连接
	bool _isDestroyed; 
	void OnDestroy()
	{
		_isDestroyed = true; 
		onStatusChanged = null;
		Singleton.Clear(); 
		if (peer != null)
		{
			peer.Disconnect();
		}
	}

	public void Connect()
	{
		peer = new PhotonPeer(this, protocol);
		peer.Connect(serverAddress, applicationName); 
	}

	void Update()
	{
		//if (!connected) //如果与服务器断开了，就需要再连接一下
		//	peer.Connect(serverAddress, applicationName);
		if (peer == null)
		{
			return;
		}
		peer.Service();//获取服务器的响应，需要每时每刻都获取，保持连接状态
		if (Input.GetKeyDown(KeyCode.Space))
		{
			Debug.LogError("send!!!"); 
			// parameter 0对应id，1对应数据 
			var parameter = new Dictionary<byte, object>();
			var data = new C_Login_Login_0x0101 { _accountName = "1", _password = "1" };
			parameter.Add(0, data.GetMessageID());
			var temp = Singleton._serializer.Serialize<C_Login_Login_0x0101>(data);
			parameter.Add(1, temp);
			peer.OpCustom(0, parameter, true);
		}
	}

	public void DebugReturn(DebugLevel level, string message)
	{
		//Debug.LogError("DebugReturn " + message); 
	}

	// 直接服务器下发消息
	public void OnEvent(EventData eventData)
	{
		switch (eventData.Code)
		{
			case 0:
				Dictionary<byte, object> data = eventData.Parameters;
				object id;
				if (!data.TryGetValue(0, out id))
				{
					break;
				}
				Debug.Log("OnEvent id=" + ((int)id).ToHex());
				object temp;
				if (data.TryGetValue(1, out temp))
				{
					Singleton._serverCallback.HandleProtoMessage((int)id, temp);
				}
				break;
			default:
				break;
		}
	}

	// 服务器给客户端的响应
	public void OnOperationResponse(OperationResponse operationResponse)
	{
		switch (operationResponse.OperationCode)
		{
			case 0:
				Dictionary<byte, object> data = operationResponse.Parameters;
				object id;
				if (!data.TryGetValue(0, out id))
				{
					break;
				}
				object temp;
				if (data.TryGetValue(1, out temp))
				{
					Singleton._serverCallback.HandleProtoMessage((int)id, temp);
				}
				break;
			default:
				break;
		}
	}

	//状态改变时调用
	public void OnStatusChanged(StatusCode statusCode)
	{
		if(_isDestroyed)
		{
			return; 
		}
		if(onStatusChanged != null)
		{
			onStatusChanged(statusCode); 
		}
		Debug.Log(statusCode.ToString());
		if(statusCode == StatusCode.ExceptionOnReceive || statusCode == StatusCode.ExceptionOnConnect)
		{
            LogUtil.Popup("连接服务器失败，请检查网络连接！"); 
			//Debug.LogError("连接服务器失败，请检查网络连接！"); 
		}

		switch (statusCode)
		{
			case StatusCode.Connect:
				connected = true;
				break;
			case StatusCode.Disconnect:
				Debug.LogError("您已掉线！");
				Singleton._uiManager.Open(EView.Login); 
				connected = false;
				break;
		}
	}
}

