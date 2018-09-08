using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardLibrary : ScriptableObject
{
	[ElementNameAttribute]
	public BoardConf[] _boards; 

	public BoardConf GetBoard(int id)
	{
		for (int i = 0, max = _boards.Length; i < max; i++)
		{
			var b = _boards[i];
			if (b._id == id)
			{
				return b; 
			}
		}
		return null; 
	}
}
