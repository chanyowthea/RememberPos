using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonLibrary : ScriptableObject
{
	[ElementNameAttribute]
	public PersonConf[] _persons;

	public PersonConf GetPerson(int id)
	{
		for (int i = 0, max = _persons.Length; i < max; i++)
		{
			var p = _persons[i];
			if (p._id == id)
			{
				return p;
			}
		}
		return null;
	}
}
