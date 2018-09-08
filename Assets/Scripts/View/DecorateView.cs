using RememberPos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Decorate; 

public class DecorateView : BaseView
{
    [SerializeField] Toggle _blockToggle;
    [SerializeField] Toggle _blockBoardToggle;
    [SerializeField] Toggle _boardToggle;
    [SerializeField] Toggle _personToggle;
    [SerializeField] BlockRect _blockRect;
    [SerializeField] BlockBoardRect _blockBoardRect;
    [SerializeField] BoardRect _boardRect;
    [SerializeField] PersonRect _personRect;

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
        base.Open();
    }

    public override void Close()
    {
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

    public override void OnClick0()
    {
        Singleton._uiManager.Open(EView.Lobby); 
        base.OnClick0();
    }

    void OnSetBlock(bool value)
    {
        if (!value)
        {
            _blockRect.Close(); 
            return;
        }
        _blockRect.Open();
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
}
