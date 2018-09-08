using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemConf : ScriptableObject
{
	public int _id;
	public GameObject _prefab; 
}

public class BlockConf : ItemConf
{
    public Sprite _sprite;
}