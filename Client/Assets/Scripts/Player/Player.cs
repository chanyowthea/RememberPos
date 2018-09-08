using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using RememberPos;

public class Player : MonoBehaviour
{
	public PlayerData _playerData;
	public PlayerData playerData
	{
		set
		{
			if (value == null || value == _playerData)
			{
				return;
			}
			_playerData = value;
			_accountName = value._accountName;
			_isLocalPlayer = value._accountName == Singleton._accountName;
		}

		get
		{
			return _playerData;
		}
	}

	public string _accountName;
	public bool _isLocalPlayer;

	int _scores;
	public int scores
	{
		set
		{
			_scores = value;
			OnSetScores(value); 
		}
		get
		{
			return _scores; 
		}
	}

	[SerializeField] TextMesh _nameText;
	[SerializeField] GameObject _playerCameraObj;

	public static event Action<int> _onAddPlayerDone;
	public static event Action<int> _onSetServerPlayerIdDone;

	void Start()
	{
		transform.SetParent(Singleton._gameReference._playerParent);
		_nameText.text = playerData._nickName;
		_playerCameraObj.SetActive(_isLocalPlayer);
		bool isMine = Singleton._sceneManager._ownerAccountName == _accountName;
		transform.position = isMine ?
			Singleton._gameReference._playerPos_Mine.transform.position :
			Singleton._gameReference._playerPos_Opposite.transform.position;
		transform.localEulerAngles = isMine ?
			Singleton._gameReference._playerPos_Mine.transform.localEulerAngles :
			Singleton._gameReference._playerPos_Opposite.transform.localEulerAngles;
	}
    
	void Update()
	{
		if (!_isLocalPlayer)
		{
			return;
		}

		if (!Singleton._gameData._permitInput)
		{
			return;
		}
		if (Camera.main == null)
		{
			return;
		}

		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100f, LayerMask.GetMask("Block")))
            {
                var block = hit.transform.GetComponentInParent<Block>();
                if (block != null)
                {
                    Debug.LogError("model=" + block.name);
                    // 如果开始没有显示，点击之后才将要显示，那么判定是否存在相同的
                    if (!block.isShowModel)
                    {
                        Debug.LogError("block._isShowModel=" + block._itemId);
                        Singleton._sceneService.ShowBlock(block._index, null);
                    }
                }
            }
            else if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100f, LayerMask.GetMask("Player")))
            {
                var player = hit.transform.GetComponentInParent<Player>();
                if (player != null)
                {
                    var view = ServerManager.Instance._uiManager.Get<AccountInfoView>(EView.AccountInfo);
                    view.accountName = player._accountName;
                    view._onClickClose = () => ServerManager.Instance._uiManager.Open(EView.HUD);
                    view.Open(); 
                }
            }
		}
	}

	// 准备显示新的Block
	void ServerCountPermit(GameObject go)
	{
		if (go == null)
		{
			Debug.LogError("go is empty! ");
			return;
		}
		var block = go.GetComponent<Block>();
		if (block == null)
		{
			Debug.LogError("block is empty! ");
			return;
		}
		// 执行这个就会导致Block._showedBlocks这个表添加了这个block
		block.isShowModel = true;

		// 删除空Block
		int index = 0;
		while (Block._showedBlocks.Count > index)
		{
			var b = Block._showedBlocks[index];
			if (b == null)
			{
				Block._showedBlocks.RemoveAt(index);
			}
			else
			{
				++index;
			}
		}

		if (Block._showedBlocks.Count > Singleton._gameData._maxShowedBlocksCount)
		{
			var b = Block._showedBlocks[0];
			b.isShowModel = false;
		}
	}

	public void SetScores(int value)
	{
		// 增加得分
		scores = value;
	}
	
	void OnSetScores(int scores)
	{
		var hud = ServerManager.Instance._uiManager.Get<HUDView>(EView.HUD);
		if (_isLocalPlayer)
		{
			hud.myScores = scores;
		}
		else
		{
			hud.otherScores = scores;
		}
	}
}
