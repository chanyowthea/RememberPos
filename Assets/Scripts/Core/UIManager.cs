using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;

// 关闭UI之后无法打开前面一个UI

public class UIManager : MonoBehaviour
{
	List<BaseView> _createdViews = new List<BaseView>();
	// 所有的添加与关闭在这里，出了问题，责任在这里
	List<BaseView> _openedViews = new List<BaseView>();
	List<BaseView> _closedViews = new List<BaseView>();
	Transform _viewParent;
	public void Init()
	{
		_viewParent = new GameObject("ViewParent").transform;
		GameObject go = new GameObject("EventSystem");
		go.AddComponent<EventSystem>();
		go.AddComponent<StandaloneInputModule>();
		go.transform.SetParent(_viewParent); 
	}

	public void _Reset()
	{

	}

	public void Clear()
	{
		CloseAll();
		GameObject.Destroy(_viewParent.gameObject);
	}

	public T Open<T>(EView view, ViewData viewData = null) where T : BaseView
	{
		//Write("Before Open"); 
		var v = Get<T>(view);

		// 这个viewData是外来参数，如果设置了就更新数据，如果没设置就使用旧数据
		if (viewData != null)
		{
			v._viewData.SetData(viewData);
		}

		if (!_openedViews.Contains(v))
		{
			v.Open();
		}
		// 已经打开了
		else
		{
			if (v._viewData._permitReopen)
			{
				v.Close(); 
				v.Open();
			}
			else
			{
				Debug.LogError(string.Format("the view with type {0} is already opened! ", view));
			}
		}
		//Write("After Open");
		object obj = v;
		return (T)v;
	}

	public void Open(EView view, ViewData viewData = null)
	{
		Open<BaseView>(view, viewData);
	}

	void _Open(EView view, int uid)
	{
		var v = Get<BaseView>(view);

		if (v._viewData._isCloseOther)
		{
			CloseAll();
		}

		if (!_openedViews.Contains(v))
		{
			_openedViews.Add(v);
		}
		if (_closedViews.Contains(v))
		{
			_closedViews.Remove(v);
		}
	}

	public void Close(EView view, int uid = 0)
	{
		for (int i = 0, length = _openedViews.Count; i < length; i++)
		{
			var v = _openedViews[i];
			if (v._viewData._viewType == view
				//&& v._viewData._uid == uid
				)
			{
				v.Close();
				break; 
			}
		}
	}

	// UI内部执行Close就执行这里
	void _Close(EView view, int uid)
	{
		for (int i = 0, length = _openedViews.Count; i < length; i++)
		{
			var v = _openedViews[i];
			if (v._viewData._viewType == view
				&& v._viewData._uid == uid
				)
			{
				if (!_closedViews.Contains(v))
				{
					_closedViews.Add(v);
				}
				else
				{
					Debug.LogError(string.Format("the view with type {0} is already closed! ", view));
				}
				if (_openedViews.Contains(v))
				{
					_openedViews.Remove(v);
				}
				break; 
			}
		}
	}

	public T Get<T>(EView view) where T : BaseView
	{
		// 如果对应窗口已经创建，那么使用已经创建了的窗口
		for (int i = 0, length = _createdViews.Count; i < length; i++)
		{
			var v = _createdViews[i];
			if (v._viewData._viewType == view)
			{
				object o = v;
				return (T)o;
			}
		}

		// 如果没有窗口就创建一个
		var conf = ServerManager.Instance._viewLib.GetConf(view);
		if (conf != null)
		{
			var v = GameObject.Instantiate(conf._prefab);
			v.transform.SetParent(_viewParent);
			var viewData = new ViewData(view);
			viewData.SetData(conf);
			v._viewData = viewData;
			v.GetComponent<Canvas>().sortingOrder = v._viewData._sortOrder; 
			v.onClose += _Close;
			v.onOpen += _Open;
			_createdViews.Add(v);
			// 窗口的默认状态是关闭
			_closedViews.Add(v);
			v.Close(); 
			object o = v;
			return (T)o;
		}
		return default(T);
	}

	public void CloseAll()
	{
		// 由于o.Close会执行_openViews的删除操作，因此备份一个list
		var list = new List<BaseView>(_openedViews); 
		for (int i = 0, length = list.Count; i < length; i++)
		{
			var o = list[i];
			// Fixed不执行关闭
			if (o._viewData._isFixed)
			{
				return; 
			}
			o.Close();
		}
		_openedViews.Clear();
	}

	public void CloseOther(EView exceptType, int exceptId)
	{
		// 将需要删除的加入表中
		var viewsNeedToBeRemove = new List<BaseView>();
		for (int i = 0, length = _openedViews.Count; i < length; i++)
		{
			var o = _openedViews[i];
			if (o._viewData._viewType != exceptType && o._viewData._uid == exceptId)
			{
				viewsNeedToBeRemove.Add(o);
			}
		}
		// 遍历表，执行删除操作
		for (int i = 0, length = viewsNeedToBeRemove.Count; i < length; i++)
		{
			var v = viewsNeedToBeRemove[i];
			var o = _openedViews.Find((BaseView temp) => temp == v);
			if (o != null)
			{
				_openedViews.Remove(o);
				o.Close();
			}
		}
	}

	void Write(string title)
	{
		string s = title + " _created: ";
		for (int i = 0, length = _createdViews.Count; i < length; i++)
		{
			s += _createdViews[i]._viewData._viewType + ", ";
		}
		s += "\n_opened: ";
		for (int i = 0, length = _openedViews.Count; i < length; i++)
		{
			s += _openedViews[i]._viewData._viewType + ", ";
		}
		s += "\n_closed: ";
		for (int i = 0, length = _closedViews.Count; i < length; i++)
		{
			s += _closedViews[i]._viewData._viewType + ", uid=" + _closedViews[i]._viewData._uid + "; ";
		}
		//LogUtil.Write(s); 
		Debug.Log(s);
	}
}

public enum EView
{
	None,
	Lobby,
	HUD,
	Message,
	Log,
	Login,
	Register,
	Ready, 
	Fixed, 
    Mall, 
    AccountInfo, 
    Decorate, 
}
