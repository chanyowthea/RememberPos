//using RememberPos;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class RoomItem : MonoBehaviour
//{
//	public RoomInfo _info;
//	LoginService _service = new LoginService();
//	public void OnClickJoin()
//	{
//		if (_info == null)
//		{
//			Debug.LogError("_info is empty! ");
//			return;
//		}
//		_service.JoinRoom(_info._ownerAccountName, OnJoinRoom);
//	}

//	void OnJoinRoom(LobbyPlayerData[] datas)
//	{
//		Singleton._uiManager._readyView.Close();
//		Singleton._uiManager._readyView._ownerAccountName = _info._ownerAccountName;
//		Singleton._uiManager._readyView._datas.AddRange(datas);
//		Singleton._uiManager._readyView.Open(); 
//		Debug.LogError("OnJoinRoom datas.Length=" + datas.Length + ", name=" + datas[0]._nickName);
//	}
//}
