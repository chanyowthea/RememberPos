using RememberPos;
using RememberPos.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneService 
{
	Action<int> _onExit; // 错误码
	public void Exit(int sceneId, Action<int> action)
	{
		Debug.Log("[C_Scene_Exit_0x0304]");
		_onExit = action; 
		var data = new C_Scene_Exit_0x0304();
		Singleton._serverCallback.AddCallback<S_Scene_Exit_0x0304>(OnExit);
		Singleton._messageManager.Send(data);
	}

	void OnExit(S_Scene_Exit_0x0304 msg)
	{
		Singleton._serverCallback.RemoveCallback<S_Scene_Exit_0x0304>(OnExit);
		Debug.LogFormat("[S_Scene_Exit_0x0304] returnCode={0}", msg._returnCode);
		if (_onExit != null)
		{
			_onExit(msg._returnCode);
		}
	}

	Action<int> _onSavePlayer; 
	public void SavePlayer(PlayerData playerData, Action<int> action)
	{
        //return;

		if(playerData == null)
		{
			Debug.LogError("playerData is empty! "); 
			return; 
		}

		Debug.Log("[C_Scene_SavePlayer_0x0305]");
		_onSavePlayer = action;
		var data = new C_Scene_SavePlayer_0x0305();
		data._playerData = playerData; 
		Singleton._serverCallback.AddCallback<S_Scene_SavePlayer_0x0305>(OnSavePlayer);
		Singleton._messageManager.Send(data); 
	}

	void OnSavePlayer(S_Scene_SavePlayer_0x0305 msg)
	{
		Singleton._serverCallback.RemoveCallback<S_Scene_SavePlayer_0x0305>(OnSavePlayer);
		Debug.LogFormat("[S_Scene_SavePlayer_0x0305] returnCode={0}", msg._returnCode);
		if (_onSavePlayer != null)
		{
			_onSavePlayer(msg._returnCode);
		}
	}

	Action<BlockData[]> _onGetBlocks; 
	public void GetBlocks(Action<BlockData[]> action)
	{
		Debug.Log("[C_Scene_GetBlocks_0x0306]");
		_onGetBlocks = action;
		var data = new C_Scene_GetBlocks_0x0306();
		Singleton._serverCallback.AddCallback<S_Scene_GetBlocks_0x0306>(OnGetBlocks);
		Singleton._messageManager.Send(data);
	}

	void OnGetBlocks(S_Scene_GetBlocks_0x0306 msg)
	{
		Singleton._serverCallback.RemoveCallback<S_Scene_GetBlocks_0x0306>(OnGetBlocks);
		Debug.LogFormat("[S_Scene_GetBlocks_0x0306] data count={0}", msg._data.Length);
		if (_onGetBlocks != null)
		{
			_onGetBlocks(msg._data);
		}
	}

	Action<int, bool> _onShowBlock;
	public void ShowBlock(int index, Action<int, bool> action)
	{
		Debug.Log("[C_Scene_ShowBlock_0x0307]");
		_onShowBlock = action;
		var data = new C_Scene_ShowBlock_0x0307();
		data._index = index; 
		//Singleton._serverCallback.AddCallback<S_Scene_ShowBlock_0x0307>(OnShowBlock);
		Singleton._messageManager.Send(data);
	}

    //void OnShowBlock(S_Scene_ShowBlock_0x0307 msg)
    //{
    //	Singleton._serverCallback.RemoveCallback<S_Scene_ShowBlock_0x0307>(OnShowBlock);
    //	Debug.LogFormat("[S_Scene_ShowBlock_0x0307] index={0}", msg._index);
    //	if (_onShowBlock != null)
    //	{
    //		_onShowBlock(msg._index, msg._isShow);
    //	}
    //}

    Action<PlayerData> _onGetPlayerData;
    public void GetPlayerData(string accountName, Action<PlayerData> action)
    {
        Debug.Log("[C_Scene_GetPlayerData_0x0309]");
        _onGetPlayerData = action;
        var data = new C_Scene_GetPlayerData_0x0309();
        data._accountName = accountName;
        Singleton._serverCallback.AddCallback<S_Scene_GetPlayerData_0x0309>(OnGetPlayerData);
        Singleton._messageManager.Send(data);
    }

    void OnGetPlayerData(S_Scene_GetPlayerData_0x0309 msg)
    {
        Singleton._serverCallback.RemoveCallback<S_Scene_GetPlayerData_0x0309>(OnGetPlayerData);
        Debug.LogFormat("[S_Scene_GetPlayerData_0x0309] data={0}", msg._playerData);
        Singleton._gameData._playerData = msg._playerData; 
        if (_onGetPlayerData != null)
        {
            _onGetPlayerData(msg._playerData);
        }
    }

    Action<AssetData> _onGetAsset;
    public void GetAsset(string accountName, Action<AssetData> action)
    {
        Debug.Log("[C_Item_GetAsset_0x0402]");
        _onGetAsset = action;
        var data = new C_Item_GetAsset_0x0402();
        data._accountName = accountName;
        Singleton._serverCallback.AddCallback<S_Item_GetAsset_0x0402>(OnGetAsset);
        Singleton._messageManager.Send(data);
    }

    void OnGetAsset(S_Item_GetAsset_0x0402 msg)
    {
        Singleton._serverCallback.RemoveCallback<S_Item_GetAsset_0x0402>(OnGetAsset);
        Debug.LogFormat("[S_Item_GetAsset_0x0402] data={0}", msg._assetData._blocks.PackToStringInt());
        Singleton._gameData._gold = msg._assetData._gold; 
        if (_onGetAsset != null)
        {
            _onGetAsset(msg._assetData);
        }
    }

    Action<int> _onBuyItem;
    public void BuyItem(int itemType, int itemId, int count, Action<int> action)
    {
        Debug.Log("[C_Item_BuyItem_0x0403]");
        _onBuyItem = action;
        var data = new C_Item_BuyItem_0x0403();
        data._itemType = itemType;
        data._itemId = itemId;
        data._count = count;
        Singleton._serverCallback.AddCallback<S_Item_BuyItem_0x0403>(OnBuyItem);
        Singleton._messageManager.Send(data);
    }

    void OnBuyItem(S_Item_BuyItem_0x0403 msg)
    {
        Singleton._serverCallback.RemoveCallback<S_Item_BuyItem_0x0403>(OnBuyItem);
        Debug.LogFormat("[S_Item_BuyItem_0x0403] code={0}", msg._code);
        if (_onBuyItem != null)
        {
            _onBuyItem(msg._code);
        }
    }

    Action<int[], int[]> _onEquipItem;
    public void EquipItem(int itemType, int itemId, bool isDecorate, Action<int[], int[]> action)
    {
        Debug.Log("[C_Item_DecorateItem_0x0405]");
        _onEquipItem = action;
        var data = new C_Item_DecorateItem_0x0405();
        data._itemType = itemType;
        data._itemId = itemId;
        data._isDecorate = isDecorate; 
        Singleton._serverCallback.AddCallback<S_Item_DecorateItem_0x0405>(OnEquipItem);
        Singleton._messageManager.Send(data);
    }

    void OnEquipItem(S_Item_DecorateItem_0x0405 msg) 
    {
        Singleton._serverCallback.RemoveCallback<S_Item_DecorateItem_0x0405>(OnEquipItem);
        Debug.LogFormat("[S_Item_DecorateItem_0x0405] type={0}, inUseIds={1}, ids={2}", msg._itemType, msg._itemIdInUse, msg._itemIds);
        if (_onEquipItem != null)
        {
            _onEquipItem(msg._itemIdInUse, msg._itemIds);
        }
    }
}
