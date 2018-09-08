//// 登录流程 Login
//// 0x0101

//// 玩家数据 Player
//// 0x0201

//// 场景数据 Scene
//// 0x0301

//// 物品系统 Item
//// 0x0401

//using System.Collections;
//using System.Collections.Generic;
//using ProtoBuf;

//namespace RememberPos.Message
//{
//	public enum ELoginResult
//	{
//		Failed_Unknown,
//		Failed_AccountNotExist,
//		Success,
//	}

//	[ProtoContract]
//	public class C_Login_Login_0x0101 : BaseMessage
//	{
//		[ProtoMember(1)]
//		public string _accountName;
//		[ProtoMember(2)]
//		public string _password;
//	}

//	[ProtoContract]
//	public class S_Login_Login_0x0101 : BaseMessage
//	{
//		[ProtoMember(1)]
//		public ELoginResult _rs;
//	}

//	[ProtoContract]
//	public class C_Login_GetRooms_0x0102 : BaseMessage
//	{

//	}

//	[ProtoContract]
//	public class S_Login_GetRooms_0x0102 : BaseMessage
//	{
//		[ProtoMember(1)]
//		public RoomInfo[] _rooms;
//	}

//	[ProtoContract]
//	public class C_Login_CreateRoom_0x0103 : BaseMessage
//	{
//		[ProtoMember(1)]
//		public string _roomName;
//	}

//	[ProtoContract]
//	public class S_Login_CreateRoom_0x0103 : BaseMessage
//	{
//		[ProtoMember(1)]
//		public bool _rs;
//		[ProtoMember(2)]
//		public LobbyPlayerData[] _lobbyPlayerDatas;
//	}

//	[ProtoContract]
//	public class C_Login_JoinRoom_0x0104 : BaseMessage
//	{
//		[ProtoMember(1)]
//		public string _ownerAccountName;
//	}

//	[ProtoContract]
//	public class S_Login_JoinRoom_0x0104 : BaseMessage
//	{
//		// 0 代表成功
//		[ProtoMember(1)]
//		public int _returnCode;
//		[ProtoMember(2)]
//		public LobbyPlayerData[] _lobbyPlayerDatas;
//	}

//	[ProtoContract]
//	public class C_Login_Ready_0x0105 : BaseMessage
//	{
//		[ProtoMember(1)]
//		public string _ownerAccountName;
//	}

//	[ProtoContract]
//	public class S_Login_Ready_0x0105 : BaseMessage
//	{
//		// 0 代表成功
//		[ProtoMember(1)]
//		public int _returnCode;
//	}

//	//[ProtoContract]
//	//public class C_Scene_Enter_0x0301 : BaseMessage
//	//{
//	//	[ProtoMember(1)]
//	//	public string _ownerAccountName;
//	//}

//	[ProtoContract]
//	public class S_Scene_Enter_0x0301 : BaseMessage
//	{
//		[ProtoMember(1)]
//		public string _ownerAccountName;
//		[ProtoMember(2)]
//		public PlayerData[] _players;
//		[ProtoMember(3)]
//		public VisitorData[] _visitors;
//	}

//	[ProtoContract]
//	public class S_Item_Gift_0x0401 : BaseMessage
//	{
//		[ProtoMember(1)]
//		public int _itemId;
//		[ProtoMember(2)]
//		public int _count;
//	}
//}

