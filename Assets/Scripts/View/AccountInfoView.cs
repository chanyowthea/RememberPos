using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;
using RememberPos;
using System;

public class AccountInfoView : BaseView
{
    [SerializeField] Text _accountNameText;
    [SerializeField] Text _nickNameText;
    [SerializeField] Text _goldText;
    [SerializeField] Text _blocksText;
    [SerializeField] Text _boardsText;
    [SerializeField] Text _personsText;

    public Action _onClickClose;

    public string accountName
    {
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }
            _accountNameText.text = value;
            PlayerData data;
            if (Singleton._players.TryGetValue(value, out data))
            {
                nickName = data._nickName;
            }
            else
            {
                Singleton._sceneService.GetPlayerData(value, (PlayerData temp) => nickName = temp._nickName);
            }

            Singleton._sceneManager.GetAssetData(value, OnGetAssetData); 
        }
        get
        {
            return _accountNameText.text;
        }
    }

    public string nickName
    {
        set
        {
            _nickNameText.text = value;
        }
        get
        {
            return _nickNameText.text;
        }
    }

    public int gold
    {
        set
        {
            _goldText.text = value.ToString();
        }
        get
        {
            int rs;
            if (int.TryParse(_goldText.text, out rs))
            {
                return rs;
            }
            return 0;
        }
    }

    public int[] blocks
    {
        set
        {
            _blocksText.text = value.PackToStringInt();
        }
        get
        {
            return _blocksText.text.UnpackToDataInt();
        }
    }

    public int[] boards
    {
        set
        {
            _boardsText.text = value.PackToStringInt();
        }
        get
        {
            return _boardsText.text.UnpackToDataInt();
        }
    }

    public int[] persons
    {
        set
        {
            _personsText.text = value.PackToStringInt();
        }
        get
        {
            return _personsText.text.UnpackToDataInt();
        }
    }

    public override void Open()
    {
        base.Open();
    }

    public override void Close()
    {
        base.Close();
    }

    void OnGetAssetData(AssetData data)
    {
        gold = data._gold;
        blocks = data._blocks;
        boards = data._boards;
        persons = data._persons;
    }

    public override void OnClick0()
    {
        if (_onClickClose != null)
        {
            _onClickClose();
        }
        base.OnClick0();
    }
}
