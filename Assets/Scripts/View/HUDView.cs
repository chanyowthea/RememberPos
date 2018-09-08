using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System; 
using UnityEngine.Networking; 

public class HUDView : BaseView
{
	[SerializeField] Text _myScoresText; 
	int _myScores; 
	public int myScores
	{
		get
		{
			return _myScores;
		}

		set
		{
			_myScores = value;
			_myScoresText.text = value.ToString(); 
		}
	}

	[SerializeField] Text _otherScoresText;
	int _otherScores;
	public int otherScores
	{
		get
		{
			return _otherScores;
		}

		set
		{
			_otherScores = value;
			_otherScoresText.text = value.ToString();
		}
	}

	public override void Open()
	{
		base.Open();
		myScores = 0;
		otherScores = 0; 
	}

	public override void OnClick0()
	{
		base.OnClick0();
		Singleton._sceneService.Exit(Singleton._sceneManager._sceneId, (int value) =>
		{
            if (value != 0)
            {
                Debug.LogError("exit scene returnCode=" + value); 
                return; 
            }
            // 本地客户端离开场景，在本地客户端是结束场景
            Singleton._sceneManager.EndScene(Singleton._sceneManager._sceneId); 
			Singleton._gameManager._OnDestroy();
		});
	}

	// 平台显示Log
	bool _isOpenLog;
	public override void OnClick1()
	{
		base.OnClick1();
		if (!_isOpenLog)
		{
			Singleton._uiManager.Open(EView.Log);
		}
		else
		{
			Singleton._uiManager.Close(EView.Log);
		}
		_isOpenLog = !_isOpenLog;
	}
}
