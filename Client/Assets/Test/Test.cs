//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Test : MonoBehaviour
//{
//	void Start()
//	{

//	}

//	public void OnClickPay()
//	{
//		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"); //获取unity的Java类,只能调用静态方法，获取静态属性  
//		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"); //获取当前的Activity对象,能调用公开方法和公开属性  
//		jo.Call("PayFirst", "name01", "detail01", "12"); //调用add函数 返回计算结果  
//	}
//}
