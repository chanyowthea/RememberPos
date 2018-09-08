using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
	public string _accountName; 

	void Start()
	{
		bool isMine = Singleton._sceneManager._ownerAccountName == _accountName;
		transform.SetParent(isMine ?
			Singleton._gameReference._boardPivot_Mine :
			Singleton._gameReference._boardPivot_Opposite);
		transform.localPosition = Vector3.zero;
	}
}
