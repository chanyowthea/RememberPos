//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Networking;
//using UnityEngine.UI; 

//public class LobbyPlayer : NetworkLobbyPlayer
//{
//	[SerializeField] Text _accountNameInput;
//	[SerializeField] GameObject _readyBtn;
//	[SerializeField] GameObject _kickBtn;
//	[SyncVar(hook = "OnSetAccountName")] public string accountName;
//	[SyncVar(hook = "OnReady")] bool isReady;

//	void OnSetAccountName(string value)
//	{
//		accountName = value; 
//		_accountNameInput.text = value;
//	}

//	void Start()
//	{
//		DontDestroyOnLoad(this); 
//		//LogUtil.Write(LogUtil.GetCurMethodName() + ", " + accountName + ", isLocalPlayer=" + isLocalPlayer);
//		//transform.SetParent(ServerManager.Instance._playerParent);
//		transform.localScale = Vector3.one;
//		_readyBtn.SetActive(true);
//		_readyBtn.GetComponent<Button>().interactable = isLocalPlayer; 
//		_kickBtn.SetActive(!isLocalPlayer && isServer);
//		OnSetAccountName(accountName);
//		OnReady(isReady); 
//	}

//	public void OnClickReady()
//    {
//		CmdReady();
//		SendReadyToBeginMessage();
//	}

//	public void _RemovePlayer()
//	{
//		connectionToClient.Disconnect(); 
//	}

//	[Command]
//	void CmdReady()
//	{
//		isReady = true; 
//	}
	
//	void OnReady(bool value)
//	{
//		isReady = value; 
//		_readyBtn.SetActive(!value);
//	}
//}
