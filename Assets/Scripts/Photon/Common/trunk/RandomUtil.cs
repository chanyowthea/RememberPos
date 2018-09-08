using System.Collections;
using System.Collections.Generic;
using System;

public class RandomUtil
{
	public System.Random _rand = new System.Random();
	public int GetNext(int max = int.MaxValue, int min = 0)
	{
		return _rand.Next(min, max);
	}
}