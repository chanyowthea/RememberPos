using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGenerator
{
	public int _widthCount = 8;
	public int _heightCount = 8;
	public float _blockWidth = 0.5f;
	public Vector2 _blockGap = new Vector2(0.05f, 0.05f);
	[SerializeField] Block _block; 
	public List<Block> _blocks = new List<Block>();  
	// 默认间距0.05，边缘0.1，砖块默认大小0.5，砖块默认数量7
	// 2-0.1-0.25, 1.3, -2+0.1+0.25
	// 间距固定，高度固定，桌面大小固定，距离桌面边缘间距固定
	public Vector3 _blockScale = Vector3.one; 

	void OnSetScale(Vector3 value)
	{
		LogUtil.Write("OnSetScale value=" + value + ", blocks count=" + _blocks.Count); 
		for (int i = 0, length = _blocks.Count; i < length; i++)
		{
			var b = Singleton._blockGenerator._blocks[i];
			b.transform.localScale = value; 
		}
	}

	public void Generate(BlockData[] datas)
	{
		if(datas == null)
		{
			return; 
		}

		_blockWidth = (4 - 0.1f * 2) / _heightCount - _blockGap.x; 
		_blockScale = new Vector3(_blockWidth / 0.5f, 1, _blockWidth / 0.5f);
		for (int i = 0, length = datas.Length; i < length; i++)
		{
			var d = datas[i];
			var block = GameObject.Instantiate(Singleton._gameReference._blockPrefab);
			block.name = "Block_" + d._index; 
			//LogUtil.Write("创建Block, name=" + block.name); 
			block._index = d._index;
			block.transform.SetParent(Singleton._gameReference._blockParent);
			block.transform.localScale = _blockScale;
			block._itemId = d._itemId;
			block._vanishTime = d._vanishTime; 
			block.Init();
			block.transform.localPosition = GetPosByID(d._index);
			_blocks.Add(block);
		}
	}

	// 为什么这里会出现已经销毁的物体被重复销毁
	public void DestroyBlock(Block block)
	{
		_blocks.Remove(block);
		GameObject.Destroy(block.gameObject);
		Singleton._gameData._permitInput = true; 
	}

	public Block GetBlock(int id)
	{
		// 由于服务器删除了这个物体，因此表里是空的，因此在这里把空的去掉
		var list = new List<Block>();
		list.AddRange(_blocks); 
		_blocks.Clear(); 
		for (int i = 0, max = list.Count; i < max; i++)
		{
			var b = list[i]; 
			if (b != null)
			{
				_blocks.Add(b);
			}
		}

		for (int i = 0, max = _blocks.Count; i < max; i++)
		{
			var b = _blocks[i]; 
			if (b.isShowModel && id == b._itemId)
			{
				return b;
			}
		}
		return null; 
	}

	public void Clear()
	{
		if (_blocks != null)
		{
			for (int i = 0, max = _blocks.Count; i < max; i++)
			{
				var b = _blocks[i]; 
				if (b != null)
				{
					GameObject.Destroy(b.gameObject); 
				}
			}
			_blocks.Clear(); 
		}
		if (_boards != null)
		{
			for (int i = 0, max = _boards.Count; i < max; i++)
			{
				var b = _boards[i]; 
				if (b != null)
				{
					GameObject.Destroy(b.gameObject); 
				}
			}
			_boards.Clear(); 
		}
	}

	#region Board
	public List<Board> _boards = new List<Board>();  
	public Board GetBoard(string accountName)
	{
		// 由于服务器删除了这个物体，因此表里是空的，因此在这里把空的去掉
		var list = new List<Board>();
		list.AddRange(_boards); 
		_boards.Clear(); 
		for (int i = 0, max = list.Count; i < max; i++)
		{
			var b = list[i]; 
			if (b != null)
			{
				_boards.Add(b);
			}
		}

		for (int i = 0, max = _boards.Count; i < max; i++)
		{
			var b = _boards[i]; 
			if (b._accountName == accountName)
			{
				return b;
			}
		}
		return null; 
	}
	#endregion

	public Vector3 GetPosByID(int index)
	{
		//index = i + j * _heightCount;
		//Debug.LogError("_heightCount=" + _heightCount + ", index=" + index); 
		int i = index % _heightCount; 
		int j = index / _heightCount;
		//1.7, -1.7
		if(index < 2)
		{
			var sys = new System.Diagnostics.StackTrace();
			//LogUtil.Write("StackTrace=" + sys.ToString()); 
			LogUtil.Write(LogUtil.GetCurMethodName() + ", pos=" + (new Vector3(2 - 0.1f - _blockWidth / 2f - (_blockWidth + _blockGap.x) * i, 1.3f, -2 + 0.1f + _blockWidth / 2f + (_blockWidth + _blockGap.y) * j))
				+ ", index=" + index + ", i=" + i + ", j=" + j);
		}
		return new Vector3(2 - 0.1f - _blockWidth / 2f - (_blockWidth + _blockGap.x) * i,
					1.3f, -2 + 0.1f + _blockWidth / 2f + (_blockWidth + _blockGap.y) * j);
	}
}
