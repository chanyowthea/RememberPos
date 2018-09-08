using RememberPos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyView : BaseView
{
	//public List<LobbyPlayerData> _datas = new List<LobbyPlayerData>();
	public string _ownerAccountName;
	[SerializeField] ReadyItem _itemPrefab;
	[SerializeField] Transform _itemContent;
	List<ReadyItem> _rooms = new List<ReadyItem>();

	private void Start()
	{
		_itemPrefab.gameObject.SetActive(false); 
	}

	public override void Open()
	{
		base.Open();
		Singleton._loginService.GetRooms(UpdateView); 
		Singleton._globalEvent._onGetRooms += UpdateView; 
	}

	public override void Close()
	{
		Singleton._globalEvent._onGetRooms -= UpdateView;
		for (int i = 0, length = _rooms.Count; i < length; i++)
		{
			Destroy(_rooms[i].gameObject);
		}
		_rooms.Clear();
		//_datas.Clear();
		base.Close();
	}

	void UpdateView(RoomInfo[] infos)
	{
		if(infos == null)
		{
			return; 
		}
		Debug.LogError("UpdateView infos.Length=" + infos.Length); 

		LobbyPlayerData[] datas = new LobbyPlayerData[0]; 
		for (int i = 0, length = infos.Length; i < length; i++)
		{
			var info = infos[i]; 
			if(info._ownerAccountName == _ownerAccountName)
			{
				datas = info._lobbyPlayerDatas.ToArray();
				break; 
			}
		}

		for (int i = 0, length = _rooms.Count; i < length; i++)
		{
			Destroy(_rooms[i].gameObject);
		}
		_rooms.Clear();

		for (int i = 0, length = datas.Length; i < length; i++)
		{
			var go = Instantiate(_itemPrefab);
			go.gameObject.SetActive(true);
			go.transform.SetParent(_itemContent);
			go.transform.localScale = Vector3.one;
			go.data = datas[i];
			go._ownerAccountName = _ownerAccountName;
			_rooms.Add(go);
		}

	}
}
