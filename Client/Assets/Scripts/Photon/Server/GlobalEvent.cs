using RememberPos;
using RememberPos.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalEvent
{
	public void Init()
	{
		Singleton._serverCallback.AddCallback<S_Scene_Start_0x0301>(OnEnterScene);
		Singleton._serverCallback.AddCallback<S_Login_GetRooms_0x0102>(OnGetRooms); // TODO 只有在lobbyView才能注册这个事件
		Singleton._serverCallback.AddCallback<S_Scene_End_0x0302>(OnEndScene);
        //Singleton._serverCallback.AddCallback<S_Item_Gift_0x0401>(TestGift);
        Singleton._serverCallback.AddCallback<S_Item_GetGold_0x0404>(OnGetGold);
    }

    public void Clear()
	{
		Singleton._serverCallback.RemoveCallback<S_Scene_Start_0x0301>(OnEnterScene);
		Singleton._serverCallback.RemoveCallback<S_Login_GetRooms_0x0102>(OnGetRooms);
		Singleton._serverCallback.RemoveCallback<S_Scene_End_0x0302>(OnEndScene);
        Singleton._serverCallback.RemoveCallback<S_Item_GetGold_0x0404>(OnGetGold);
    }

	//void TestGift(S_Item_Gift_0x0401 msg)
	//{
	//	Debug.LogFormat("[{0}] 获得物品：id={1}, count={2}", msg.GetMessageID().ToHex(), msg._itemId, msg._count); 
	//}

	void OnEnterScene(S_Scene_Start_0x0301 msg)
	{
		Debug.LogError("msg count=" + msg._players.Length + "[0]=" + msg._players[0]._nickName);
		Singleton._sceneManager._ownerAccountName = msg._ownerAccountName;
		Singleton._sceneManager._sceneId = msg._sceneId;
		for (int i = 0, length = msg._players.Length; i < length; i++)
		{
			var p = msg._players[i];
			if (Singleton._players.ContainsKey(p._accountName))
			{
				continue;
			}
			Singleton._players[p._accountName] = p;
		}
		Singleton._gameManager._Start();
		Singleton._gameData._useCountMode = msg._mode == 0;
	}

	void OnEndScene(S_Scene_End_0x0302 msg)
	{
		Debug.Log("[S_Scene_End_0x0302] sceneId=" + msg._sceneId);
		Singleton._gameManager._OnDestroy();
		Singleton._uiManager.Open(EView.Lobby);
		Singleton._sceneManager.EndScene(msg._sceneId);
	}

	public event Action<RoomInfo[]> _onGetRooms;
	void OnGetRooms(S_Login_GetRooms_0x0102 msg)
	{
		Debug.Log("[S_Login_GetRooms_0x0102]" + "rooms count=" + (msg._rooms == null ? 0 : msg._rooms.Length));
		if (_onGetRooms != null)
		{
			_onGetRooms(msg._rooms);
		}
	}

    public event Action<int> _onGetGold;
    void OnGetGold(S_Item_GetGold_0x0404 msg)
    {
        Debug.Log("[S_Item_GetGold_0x0404]" + "gold=" + msg._num);
        Singleton._gameData._gold = msg._num; 
        if (_onGetGold != null)
        {
            _onGetGold(msg._num);
        }
    }
}
