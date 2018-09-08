using RememberPos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomVO : BaseVO
{
    
}

public class RoomItem : BaseItem
{
	public RoomInfo _info;
	public override void OnClick0()
	{
		base.OnClick0();
		if (_info == null)
		{
			Debug.LogError("_info is empty! ");
			return;
		}
		Singleton._loginService.JoinRoom(_info._ownerAccountName, OnJoinRoom);
	}

	void OnJoinRoom(LobbyPlayerData[] datas)
	{
		var ui = Singleton._uiManager.Get<ReadyView>(EView.Ready); 
		ui.Close();
		ui._ownerAccountName = _info._ownerAccountName;
		//ui._datas.AddRange(datas);
		ui.Open();
		Debug.LogError("OnJoinRoom datas.Length=" + datas.Length + ", name=" + datas[0]._nickName);
	}
}
