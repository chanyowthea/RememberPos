using RememberPos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mall; 

public class MallView : BaseView
{
    [SerializeField] Toggle _blockToggle;
    [SerializeField] Toggle _blockBoardToggle;
    [SerializeField] Toggle _boardToggle;
    [SerializeField] Toggle _personToggle;
    [SerializeField] BlockRect _blockRect;
    [SerializeField] BlockBoardRect _blockBoardRect;
    [SerializeField] BoardRect _boardRect;
    [SerializeField] PersonRect _personRect;
    [SerializeField] Text _goldText; 

    public override void Open()
    {
        _blockToggle.isOn = true;
        _blockBoardToggle.isOn = false;
        _boardToggle.isOn = false;
        _personToggle.isOn = false;
        _blockRect.Close();
        _blockBoardRect.Close();
        _boardRect.Close();
        _personRect.Close();
        OnSetBlock(true);
        _blockToggle.onValueChanged.AddListener(OnSetBlock);
        _blockBoardToggle.onValueChanged.AddListener(OnSetBlockBoard);
        _boardToggle.onValueChanged.AddListener(OnSetBoard);
        _personToggle.onValueChanged.AddListener(OnSetPerson);
        Singleton._globalEvent._onGetGold += OnGetGold; 
        base.Open();
    }

    public override void Close()
    {
        Singleton._globalEvent._onGetGold -= OnGetGold;
        _blockToggle.onValueChanged.RemoveListener(OnSetBlock);
        _blockBoardToggle.onValueChanged.RemoveListener(OnSetBlockBoard);
        _boardToggle.onValueChanged.RemoveListener(OnSetBoard); 
        _personToggle.onValueChanged.RemoveListener(OnSetPerson); 
        _blockToggle.isOn = true;
        _blockBoardToggle.isOn = false;
        _boardToggle.isOn = false;
        _personToggle.isOn = false;
        _blockRect.Close();
        _blockBoardRect.Close();
        _boardRect.Close();
        _personRect.Close();
        base.Close();
    }

    public override void OnClick1()
    {
        Singleton._uiManager.Open(EView.Lobby); 
        base.OnClick1();
    }

    void OnSetBlock(bool value)
    {
        if (!value)
        {
            _blockRect.Close(); 
            return;
        }
        Singleton._sceneService.GetAsset(Singleton._accountName, (AssetData data) =>
        {
            _blockRect.Open();
            _goldText.text = data._gold.ToString(); 
            for (int i = 0, length = data._blocks.Length; i < length; i++)
            {
                var b = data._blocks[i]; 
                var v = _blockRect._vos.Find((BlockVO vo) => vo._id == b);
                if (v != null)
                {
                    v.count = 1; 
                }
            }
        }); 
    }

    void OnSetBlockBoard(bool value)
    {
        if (!value)
        {
            _blockBoardRect.Close();
            return;
        }
        Singleton._sceneService.GetAsset(Singleton._accountName, (AssetData data) =>
        {
            _blockBoardRect.Open();
            _goldText.text = data._gold.ToString();
            for (int i = 0, length = data._blockBoards.Length; i < length; i++)
            {
                var b = data._blockBoards[i];
                var v = _blockBoardRect._vos.Find((BlockBoardVO vo) => vo._id == b);
                if (v != null)
                {
                    v.count = 1;
                }
            }
        });
    }

    void OnSetBoard(bool value)
    {
        if (!value)
        {
            _boardRect.Close();
            return;
        }
        Singleton._sceneService.GetAsset(Singleton._accountName, (AssetData data) =>
        {
            _boardRect.Open();
            _goldText.text = data._gold.ToString();
            for (int i = 0, length = data._boards.Length; i < length; i++)
            {
                var b = data._boards[i];
                var v = _boardRect._vos.Find((BoardVO vo) => vo._id == b);
                if (v != null)
                {
                    v.count = 1;
                }
            }
        });
    }

    void OnSetPerson(bool value)
    {
        if (!value)
        {
            _personRect.Close();
            return;
        }
        Singleton._sceneService.GetAsset(Singleton._accountName, (AssetData data) =>
        {
            _personRect.Open();
            _goldText.text = data._gold.ToString();
            for (int i = 0, length = data._persons.Length; i < length; i++)
            {
                var b = data._persons[i];
                var v = _personRect._vos.Find((PersonVO vo) => vo._id == b);
                if (v != null)
                {
                    v.count = 1;
                }
            }
        });
    }

    void OnGetGold(int value)
    {
        _goldText.text = value.ToString(); 
    }
}
