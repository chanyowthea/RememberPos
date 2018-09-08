using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RememberPos;
using UnityEngine.UI;

public class ReadyItem : BaseItem
{
	public string _ownerAccountName;
	[SerializeField] GameObject _readyBtnObj;
	LobbyPlayerData _data;
	public LobbyPlayerData data
	{
		set
		{
			if (value == null)
			{
				return;
			}	
			_nameText0.text = value._accountName;
			_data = value;

			// 本地和当前LobbyPlayer的名字是否一样
			Debug.LogError("data isReady=" + data._isReady); 
			_readyBtnObj.SetActive(Singleton._accountName == _data._accountName && !data._isReady);
		}
		get
		{
			return _data;
		}
	}
	
	public override void OnClick0()
	{
		base.OnClick0();
		Singleton._loginService.Ready(_ownerAccountName, (int value) =>
		{
			if(value == 0)
			{
				// TODO 这里要想个办法解决
				if(_readyBtnObj == null)
				{
					return; 
				}
				_readyBtnObj.SetActive(false); 
			}
			Debug.Log("Ready returnCode=" + value + ", accountName=" + data._accountName);
		});
	}
}
