using UnityEngine;
using System.Collections;
using System;

public class BaseView : MonoBehaviour
{
	public event Action<EView, int> onOpen;
	public event Action<EView, int> onClose;

	public Action onClick0;
	public Action onClick1;
	public Action onClick2;
	public ViewData _viewData; 

	public virtual void Open()
	{
		if (onOpen != null)
		{
			onOpen(_viewData._viewType, _viewData._uid); 
		}
		gameObject.SetActive(true); 
	}

	public virtual void Close()
	{
		if (onClose != null)
		{
			onClose(_viewData._viewType, _viewData._uid); 
		}
		gameObject.SetActive(false); 
	}

	public virtual void Show()
	{
		gameObject.SetActive(false); 
	}

	public virtual void Hide()
	{
		gameObject.SetActive(false); 
	}

	public virtual void OnClickClose()
	{
		Close(); 
	}

	public virtual void OnClick0()
	{
		if (onClick0 != null)
		{
			onClick0(); 
		}
	}

	public virtual void OnClick1()
	{
		if (onClick1 != null)
		{
			onClick1(); 
		}
	}

	public virtual void OnClick2()
	{
		if (onClick2 != null)
		{
			onClick2(); 
		}
	}
}
