using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BaseVO
{
    public event Action<string> _onNameChanged0;
    public event Action<string> _onNameChanged1;
    public event Action<string> _onNameChanged2;

    string _name0;
    public string name0
    {
        set
        {
            _name0 = value;
            if (_onNameChanged0 != null)
            {
                _onNameChanged0(value);
            }
        }
        get
        {
            return _name0;
        }
    }

    string _name1;
    public string name1
    {
        set
        {
            _name1 = value;
            if (_onNameChanged1 != null)
            {
                _onNameChanged1(value);
            }
        }
        get
        {
            return _name1;
        }
    }

    string _name2;
    public string name2
    {
        set
        {
            _name2 = value;
            if (_onNameChanged2 != null)
            {
                _onNameChanged2(value);
            }
        }
        get
        {
            return _name2; 
        }
    }

    public Action onClick0;
    public Action onClick1;
    public Action onClick2;
}

public class BaseItem : MonoBehaviour
{
    [SerializeField] protected Text _nameText0;
    [SerializeField] protected Text _nameText1;
    [SerializeField] protected Text _nameText2;
    public BaseVO _vo;

    protected string name0
    {
        get
        {
            if (_nameText0 == null)
            {
                return ""; 
            }
            return _nameText0.text;
        }
        set
        {
            if (_nameText0 == null)
            {
                Debug.LogError("_nameText0 is empty! "); 
                return;
            }
            Debug.LogError("_nameText0 value=" + value);
            _nameText0.text = value;
        }
    }

    protected string name1
    {
        get
        {
            if (_nameText1== null)
            {
                return "";
            }
            return _nameText1.text;
        }
        set
        {
            if (_nameText1 == null)
            {
                return;
            }
            _nameText1.text = value;
        }
    }

    protected string name2
    {
        get
        {
            if (_nameText2 == null)
            {
                return "";
            }
            return _nameText2.text;
        }
        set
        {
            if (_nameText2 == null)
            {
                return;
            }
            _nameText2.text = value;
        }
    }

    protected Action onClick0;
    protected Action onClick1;
    protected Action onClick2;

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

    public virtual void Show()
    {
        if (_vo == null)
        {
            return;
        }
        _vo._onNameChanged0 += OnNameChanged0;
        _vo._onNameChanged1 += OnNameChanged1;
        _vo._onNameChanged2 += OnNameChanged2;
        name0 = _vo.name0;
        name1 = _vo.name1;
        name2 = _vo.name2;
        onClick0 = _vo.onClick0;
        onClick1 = _vo.onClick1;
        onClick2 = _vo.onClick2;
    }

    public virtual void Hide()
    {
        if (_vo == null)
        {
            return;
        }
        _vo._onNameChanged0 -= OnNameChanged0;
        _vo._onNameChanged1 -= OnNameChanged1;
        _vo._onNameChanged2 -= OnNameChanged2;
        name0 = null;
        name1 = null;
        name2 = null;
        onClick0 = null;
        onClick1 = null;
        onClick2 = null;
    }

    protected void OnNameChanged0(string s)
    {
        name0 = s;
    }

    protected void OnNameChanged1(string s)
    {
        name1 = s;
    }

    protected void OnNameChanged2(string s)
    {
        name2 = s;
    }
}
