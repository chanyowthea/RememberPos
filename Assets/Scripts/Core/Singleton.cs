using RememberPos;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 既可以是单例，也可以是引用
public static class Singleton
{
	// Lobby
	public static _Serializer _serializer;
	public static ServerCallback _serverCallback;
	public static ServerManager _serverManager;
	public static MessageManager _messageManager;
	public static UIManager _uiManager;
	public static GlobalEvent _globalEvent;
	public static ViewLibrary _viewLib;
	public static LoginService _loginService;
	public static string _accountName;
	public static LoginManager _loginManager;

	// Scene
	public static BlockGenerator _blockGenerator;
	public static GameData _gameData;
	public static RandomUtil _randomUtil;
	public static Dictionary<string, PlayerData> _players = new Dictionary<string, PlayerData>();
	public static SceneManager _sceneManager;
	public static GameManager _gameManager;
	public static GameReference _gameReference;
	public static SceneService _sceneService;
    public static ExcelUtil _excelUtil;


    public static void Init()
	{
		_serializer = new _Serializer();
		_serverCallback = new ServerCallback();
		_serverManager = ServerManager.Instance;
		_messageManager = new MessageManager();
		_globalEvent = new GlobalEvent();
		_globalEvent.Init();
		_viewLib = _serverManager._viewLib;
		_uiManager = _serverManager._uiManager;
		_loginService = new LoginService();
		_sceneManager = new SceneManager();
		_sceneService = new SceneService();
		_loginManager = new LoginManager(); 

		_gameManager = GameManager._instance;
		_gameReference = _gameManager._gameReference;
		_blockGenerator = new BlockGenerator();
		_gameData = new GameData();
		_randomUtil = new RandomUtil();
        _excelUtil = new ExcelUtil(); 
	}

	public static void Clear()
	{
		_globalEvent.Clear();
		_globalEvent = null;
		_randomUtil = null;
		_gameData = null;
		_blockGenerator = null;
        _excelUtil = null; 
	}
}
