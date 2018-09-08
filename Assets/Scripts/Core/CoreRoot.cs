using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreRoot : MonoBehaviour
{
	void Awake()
	{
		DontDestroyOnLoad(this); 
	}
}
