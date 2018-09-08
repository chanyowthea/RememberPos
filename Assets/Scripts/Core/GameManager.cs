using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

// 帐号信息
// 金币
// 商城

public class GameManager : MonoBehaviour
{
	public static GameManager _instance;
	public GameReference _gameReference;
	public float _saveInterval = 6f; // in seconds
	public short _playerControllerId;

	private void Awake()
	{
		_instance = this;
	}

	public void _Start()
	{
		Singleton._gameReference._mainCam.SetActive(false);
		Singleton._gameReference._sceneRoot.SetActive(true);
		LogUtil.Write("GameManager._Start");
		Singleton._sceneManager.StartScene();
		ServerManager.Instance._uiManager.Open(EView.HUD);
		InvokeRepeating("SavePlayers", _saveInterval, _saveInterval);
		for (int i = 0, length = Singleton._players.Count; i < length; i++)
		{
			var p = Singleton._players.ElementAt(i).Value;
			CreateBoard(p._accountName, p._boardIdInUse);
		}

		// 创建砖块
		Singleton._sceneService.GetBlocks(Singleton._blockGenerator.Generate); 
	}

	public void _OnDestroy()
	{
		Singleton._uiManager.Open(EView.Lobby);
		Singleton._sceneManager.EndScene(Singleton._sceneManager._sceneId);
		Singleton._gameReference._mainCam.SetActive(true);
		Singleton._gameReference._sceneRoot.SetActive(false);
		CancelInvoke("SavePlayers");
		Singleton._blockGenerator.Clear();
	}

	public void CreateBoard(string accountName, int boardId)
	{
		var boardConf = Singleton._gameReference._boardLib.GetBoard(boardId);
		if (boardConf != null)
		{
			var go = GameObject.Instantiate(boardConf._prefab);
			var b = go.GetComponent<Board>();
			if (b != null)
			{
				b._accountName = accountName;
				Singleton._blockGenerator._boards.Add(b);
			}
			else
			{
				Debug.LogError("board is empty! ");
			}
		}
	}

	void SavePlayers()
	{
		for (int i = 0, length = Singleton._players.Count; i < length; i++)
		{
			var p = Singleton._players.ElementAt(i).Value;
			// 只有本地Player才有权限存储数据
			if (p._accountName == Singleton._accountName)
			{
				Singleton._sceneService.SavePlayer(p, null);
				break; 
			}
		}
	}


	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.U))
		{
			Singleton._gameData._useCountMode = !Singleton._gameData._useCountMode;
		}
	}
}
