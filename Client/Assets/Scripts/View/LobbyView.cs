using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using RememberPos;

public class LobbyView : BaseView
{
	[SerializeField] InputField _roomNameInput;
	[SerializeField] Text _localIPText;
	[SerializeField] RectTransform _content;
	[SerializeField] RoomItem _itemPrefab;
	
	List<RoomItem> _items = new List<RoomItem>();
    List<RoomVO> _itemVOs = new List<RoomVO>();

    public override void Open()
	{
		base.Open();

		_localIPText.text = Network.player.ipAddress;
		_itemPrefab.gameObject.SetActive(false);

		Singleton._loginService.GetRooms(OnGetRooms);
		Singleton._globalEvent._onGetRooms += OnGetRooms; 
	}

	public override void Close()
	{
		Singleton._globalEvent._onGetRooms -= OnGetRooms;
		ClearItems();
		base.Close();
	}

	// 创建房间
	public override void OnClick0()
	{
		if (string.IsNullOrEmpty(_roomNameInput.text))
		{
			var ui0 = ServerManager.Instance._uiManager.Open<MessageView>(EView.Message);
			ui0.message = "房间名不能为空！";
			return;
		}

        base.OnClick0();
        //var view = ServerManager.Instance._uiManager.Get<LogView>(EView.Log);
        //Close();

        Singleton._loginService.CreateRoom(_roomNameInput.text, _mode, (LobbyPlayerData[] temps) =>
		{
			var ui = ServerManager.Instance._uiManager.Get<ReadyView>(EView.Ready);
			//Debug.LogError("OnGetRooms accountName=" + temps[0]._accountName + ", count=" + temps.Count);
			ui._ownerAccountName = Singleton._accountName; 
			//ui._datas.AddRange(temps);
			ui.Open(); 
		}); 
	}

	// 登出
	public override void OnClick1()
	{
		base.OnClick1();
		Singleton._loginService.Logout(Singleton._accountName, (int value) =>
		{
			if(value == 0)
			{
				Singleton._uiManager.Open(EView.Login);
			}
		}); 
	}

    // 商城
    public override void OnClick2()
    {
        base.OnClick2();
        ServerManager.Instance._uiManager.Open(EView.Mall);
    }

    public void OnOpenDecorate()
    {
        ServerManager.Instance._uiManager.Open(EView.Decorate);
    }

    void OnSelectRoom()
	{
		Close();
		ServerManager.Instance._uiManager.Open(EView.Ready);
	}

	void ClearItems()
	{
		for (int i = 0, length = _items.Count; i < length; i++)
		{
			var item = _items[i];
            item.Hide(); 
			GameObject.Destroy(item.gameObject);
		}
		_items.Clear();
	}

	public void OnGetRooms(RoomInfo[] infos)
	{
		if (infos == null)
		{
			return;
		}
		ClearItems();
        _itemVOs.Clear();
        
		for (int i = 0, length = infos.Length; i < length; i++)
		{
			var c = infos[i];
            RoomVO vo = new RoomVO();
            vo.name0 = string.Format("{0}({1}, {2})", c._roomName, c._ip, c._mode == 0 ? "计数" : "计时");
            Debug.Log("OnGetRooms name=" + vo.name0); 
            vo.onClick0 = OnSelectRoom; 
            _itemVOs.Add(vo);

            var item = GameObject.Instantiate(_itemPrefab);
			item.transform.SetParent(_content);
			item.gameObject.SetActive(true);
			item.transform.localScale = Vector3.one;
			item._info = c;
            item._vo = vo; 
            item.Show(); 
			_items.Add(item);
		}
	}

	int _mode = 0;
	[SerializeField] Toggle _countModeToggle; 
	[SerializeField] Toggle _timeModeToggle; 
	public void OnSetCountMode()
	{
		if(_countModeToggle.isOn)
		{
			_mode = 0;
		}
	}

	public void OnSetTimeMode()
	{
		if (_timeModeToggle.isOn)
		{
			_mode = 1;
		}
	}

    public void OnClickAccount()
    {
        var view = ServerManager.Instance._uiManager.Get<AccountInfoView>(EView.AccountInfo);
        view.accountName = Singleton._accountName;
        view._onClickClose = () => ServerManager.Instance._uiManager.Open(EView.Lobby);
        view.Open();
    }
}
