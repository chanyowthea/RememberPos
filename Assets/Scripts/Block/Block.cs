using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Block : MonoBehaviour
{
	// 按照时间添加，最早添加的在最前面
	public static List<Block> _showedBlocks = new List<Block>();
	bool _isShowModel;
	public bool isShowModel
	{
		set
		{
			_isShowModel = value;
			ShowModel(value);
		}
		get
		{
			return _isShowModel;
		}
	}

	public int _itemId = 1;
	public int _index;
	// 每个Block的消失时间可能不同
	public int _vanishTime = 5;

	MeshRenderer _mask;
	GameObject _model;

	public void Init()
	{

	}

	public void Clear()
	{
		if (_model != null)
		{
			GameObject.Destroy(_model);
		}
	}

	private void Start()
	{
		transform.SetParent(Singleton._gameReference._blockParent);
		SetModel(_itemId);
		ShowModel(false);
		transform.localScale = Singleton._blockGenerator._blockScale; // TODO 放在这里有问题啊
		transform.localPosition = Singleton._blockGenerator.GetPosByID(_index);
		CreateBlockBoard();
	}

	public void DestroySelf()
	{
		if (_showedBlocks.Contains(this))
		{
			_showedBlocks.Remove(this);
		}

		// 销毁的这个协程内不允许输入
		Singleton._gameData._permitInput = false;

		CoroutineUtil.WaitSeconds(() =>
		{
			Singleton._gameData._permitInput = true;
			// 增加得分 这个要服务器改
			Singleton._blockGenerator.DestroyBlock(this);
		}, 0.5f);
	}

	void ShowModel(bool value)
	{
		//_isShowModel = value;
		_model.SetActive(value);
		if (value)
		{
			if (!_showedBlocks.Contains(this))
			{
				_showedBlocks.Add(this);
			}
		}
		else
		{
			if (_showedBlocks.Contains(this))
			{
				_showedBlocks.Remove(this);
			}
		}
		SetBlockBoard(!value);

		//if (!Singleton._gameData._useCountMode)
		//{
		//	ServerCountDown(this);
		//}
	}

	void ServerCountDown(Block block)
	{
		if (block == null)
		{
			Debug.LogError("block is empty! ");
			return;
		}
		if (!block.isShowModel)
		{
			return;
		}
		CoroutineUtil.WaitSeconds(() =>
		{
			// 这里是由于前面被消除了，因此这里是空的
			Singleton._sceneService.ShowBlock(block._index, (int index, bool isShow) =>
			{
				if (index != block._index)
				{
					Debug.LogErrorFormat("index与服务器不一致 index={0}, block.index={1}", index, block._index);
					return;
				}
				if (block != null)
				{
					// 这里必须是false
					// block.isShowModel = false;
					if (isShow)
					{
						Debug.LogErrorFormat("服务器给了错误的数据 index={0}, isShow={1}", index, isShow);
					}
					block.isShowModel = isShow;
				}
			});
		}, block._vanishTime);
	}

	void SetModel(int id)
	{
		_itemId = id;
		if (_model != null)
		{
			GameObject.Destroy(_model);
		}

		var conf = Singleton._gameReference._blockLib.GetBlock(id);
		_model = GameObject.Instantiate(conf._prefab);
		_model.SetActive(false);
		_model.transform.SetParent(transform);
		_model.transform.localPosition = Vector3.zero;
		_model.transform.localScale = Vector3.one;
		_model.layer = gameObject.layer;
	}

	void CreateBlockBoard()
	{
		bool isLittleIndexSide = _index < Singleton._blockGenerator._heightCount * Singleton._blockGenerator._widthCount / 2;
		for (int i = 0, length = Singleton._players.Count; i < length; i++)
		{
			var p = Singleton._players.ElementAt(i).Value;
			if (p == null)
			{
				Debug.LogError("player is empty! ");
				continue;
			}
			// 这个isLittleIndexSide值得仅仅是在这一边的，而不是一定就是本地Player拥有的Board
			if (isLittleIndexSide)
			{
				if (p._accountName == Singleton._sceneManager._ownerAccountName)
				{
					CreateMask(p._blockBoardIdInUse);
				}
			}
			else
			{
				if (p._accountName != Singleton._sceneManager._ownerAccountName)
				{
					CreateMask(p._blockBoardIdInUse);
				}
			}
		}
	}

	void CreateMask(int id)
	{
		var conf = Singleton._gameReference._blockBoardLib.GetBlockBoard(id);
		if (conf == null)
		{
			return;
		}
		if (_mask != null)
		{
			GameObject.Destroy(_mask.gameObject);
		}
		_mask = GameObject.Instantiate(conf._prefab).GetComponent<MeshRenderer>();
		_mask.transform.SetParent(transform);
		_mask.transform.localPosition = Vector3.zero;
		_mask.transform.localScale = new Vector3(0.5f, 0.1f, 0.5f);
	}

	void SetBlockBoard(bool isShow = true)
	{
		if (_mask == null)
		{
			CreateBlockBoard();
		}
		if (_mask != null)
		{
			_mask.enabled = isShow;
		}
	}
}
