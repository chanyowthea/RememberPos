using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Unity编辑器中的配置
public class ViewConf : ScriptableObject
{
	public EView _viewType;
	public BaseView _prefab;
	public bool _isCloseOther = true;
	public bool _permitReopen;
	public int _sortOrder;
	public bool _isFixed; 
}

// 运行时的配置
[System.Serializable]
public class ViewData
{
	// 唯一ID
	public readonly int _uid; 
	public readonly EView _viewType; 
	public bool _isCloseOther = true;
	public bool _permitReopen;
	public int _sortOrder;
	public bool _isFixed;

	public ViewData(EView viewType)
	{
		_uid = GetHashCode();
		_viewType = viewType;
	}

	public ViewData()
	{

	}

	public void SetData(ViewData data)
	{
		_isCloseOther = data._isCloseOther;
		_permitReopen = data._permitReopen;
	}

	public void SetData(ViewConf conf)
	{
		_isCloseOther = conf._isCloseOther;
		_permitReopen = conf._permitReopen;
		_sortOrder = conf._sortOrder;
		_isFixed = conf._isFixed; 
	}
}
