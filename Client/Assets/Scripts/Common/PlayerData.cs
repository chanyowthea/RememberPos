﻿//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
////using MySql.Data.MySqlClient;
////using ProtoBuf;
//using System;

//[System.Serializable]
////[ProtoContract]
//public class PlayerData // : BaseMessage
//{
//	public static int _msgId = 0x0301;
//	// 这里写的所有数据都会储存
//	//[ProtoMember(1)]
//	public int _playerId;
//	//[ProtoMember(2)]
//	public int _boardIdInUse;
//	//[ProtoMember(3)]
//	public string _accountName;
//	//[ProtoMember(4)]
//	public string _nickName;
//	//[ProtoMember(5)]
//	public int _blockIdInUse; // block board
//	//	public int _playerModelId; 
//	//System.Reflection.FieldInfo[] _fieldInfos;

//	public PlayerData()
//	{
//		//List<System.Reflection.FieldInfo> list = new List<System.Reflection.FieldInfo>();
//		//foreach(var temp in GetType().GetFields())
//		//{
//		//	var _as = Attribute.GetCustomAttributes(temp);
//		//	if(_as != null && _as.Length > 0)
//		//	{
//		//		// 获得被ProtoMemeber修饰的成员
//		//		if((_as[0] as ProtoMemberAttribute) != null)
//		//		{
//		//			list.Add(temp); 
//		//		}
//		//	}
//		//}
//		//_fieldInfos = list.ToArray(); 
//	}

//	public override string ToString()
//	{
//		return string.Format("[PlayerData] _playerId={0}, _boardIdInUse={1}", _playerId, _boardIdInUse);
//	}

//	//public void SetAllValues(object[] objs)
//	//{
//	//	var fs = _fieldInfos; 
//	//	if (fs.Length != objs.Length)
//	//	{
//	//		Debug.LogError(string.Format("数据长度错误！变量个数：{0}, 数据个数：{1}", fs.Length, objs.Length));
//	//		return;
//	//	}
//	//	for (int i = 0, max = objs.Length; i < max; i++)
//	//	{
//	//		fs[i].SetValue(this, objs[i]);
//	//	}
//	//}

//	//public string[] ToTest()
//	//{
//	//	List<string> ss = new List<string>();
//	//	var fs = GetSortedFieldInfos();
//	//	for (int i = 0, length = fs.Length; i < length; i++)
//	//	{
//	//		var a = fs[i]; 
//	//		Console.WriteLine(a.Name);
//	//		//ss.Add(a.Name); 
//	//	}
//	//	Console.WriteLine("---");
//	//	foreach (var f in GetType().GetFields())
//	//	{
//	//		//	var a = Attribute.GetCustomAttributes(f)[0];
//	//		//	Console.WriteLine(((ProtoMemberAttribute)a).Tag + ", name=" + f.Name);
//	//		Console.WriteLine(f.Name); 
//	//		//	ss.Add(f.Name);
//	//	}

//	//	Console.WriteLine("---");
//	//	fs = GetType().GetFields();
//	//	for (int i = 0, length = fs.Length; i < length; i++)
//	//	{
//	//		var a = fs[i];
//	//		Console.WriteLine(a.Name);
//	//	}
//	//	return ss.ToArray();
//	//}

//	//System.Reflection.FieldInfo[] GetSortedFieldInfos()
//	//{
//	//	List<System.Reflection.FieldInfo> ss = new List<System.Reflection.FieldInfo>();
//	//	ss.AddRange(GetType().GetFields()); 
//	//	ss.Sort((System.Reflection.FieldInfo x, System.Reflection.FieldInfo y) =>
//	//		{
//	//			return (Attribute.GetCustomAttributes(x)[0] as ProtoMemberAttribute).Tag
//	//			- (Attribute.GetCustomAttributes(y)[0] as ProtoMemberAttribute).Tag;
//	//		});
//	//	return ss.ToArray();
//	//}

//	//public string[] ToColumnNames()
//	//{
//	//	List<string> ss = new List<string>();
//	//	foreach (var f in _fieldInfos)
//	//	{
//	//		ss.Add(f.Name);
//	//	}
//	//	return ss.ToArray();
//	//}

//	//public string[] ToClumnTypes()
//	//{
//	//	List<string> ss = new List<string>();
//	//	foreach (var f in _fieldInfos)
//	//	{
//	//		ss.Add(f.FieldType.ToSqlType());
//	//	}
//	//	return ss.ToArray();
//	//}

//	//public MySqlParameter[] ToSqlParams()
//	//{
//	//	List<MySqlParameter> ps = new List<MySqlParameter>();
//	//	foreach (var f in _fieldInfos)
//	//	{
//	//		ps.Add(new MySqlParameter("?" + f.Name, f.GetValue(this)));
//	//	}
//	//	return ps.ToArray();
//	//}

//	//public MySqlParameter[] ToSqlParamsWithoutID()
//	//{
//	//	List<MySqlParameter> ps = new List<MySqlParameter>();
//	//	ps.AddRange(ToSqlParams());
//	//	ps.RemoveAt(0);
//	//	return ps.ToArray();
//	//}
//}
