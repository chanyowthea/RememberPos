using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedView : BaseView
{
	[SerializeField] Transform _playerParent;

	public override void Open()
	{
		//ServerManager.Instance._playerParent = _playerParent;
		base.Open();
	}
}
