//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//[System.Serializable]
//public class RoomInfo // : BaseMessage
//{
//	public string _ipAddress;
//	public string _roomName;

//	public void Unpack(string s)
//	{
//		string[] ss = s.Split(',');
//		_ipAddress = ss[1].Split('=')[1].TrimStart(' ');
//		_roomName = ss[2].Split('=')[1].TrimStart(' ');
//	}

//	public override bool Equals(object obj)
//	{
//		RoomInfo info = (RoomInfo)obj;
//		if (info == null)
//		{
//			return false;
//		}
//		if (info._ipAddress == _ipAddress && info._roomName == _roomName)
//		{
//			return true;
//		}
//		return false;
//	}
//	public override string ToString()
//	{
//		return string.Format("ip={0}, name={1}", _ipAddress, _roomName);
//	}
//}

