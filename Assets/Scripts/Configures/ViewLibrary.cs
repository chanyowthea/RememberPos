using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewLibrary : ScriptableObject
{
	[SerializeField, ViewConf] ViewConf[] _confs;

	public ViewConf GetConf(EView id)
	{
		foreach (ViewConf c in _confs)
		{
			if (c._viewType == id)
			{
				return c;
			}
		}
		return null;
	}
}
