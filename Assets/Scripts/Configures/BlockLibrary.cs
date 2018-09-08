using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockLibrary : ScriptableObject
{
	[ElementNameAttribute]
	public BlockConf[] _blocks;

	public BlockConf GetBlock(int id)
	{
		for (int i = 0, max = _blocks.Length; i < max; i++)
		{
			var b = _blocks[i];
			if (b._id == id)
			{
				return b; 
			}
		}
		return null; 
	}
}
