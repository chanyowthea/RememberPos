// 登录流程 Login
// 0x0101

// 玩家数据 Player
// 0x0201

// 场景数据 Scene
// 0x0301

// 物品系统 Item
// 0x0401

// 角色加入场景 角色离开
using System.Collections;
using System.Collections.Generic;
using ProtoBuf;

namespace RememberPos.Message
{
	public enum ELoginResult
	{
		Failed_Unknown,
		Failed_AccountNotExist,
		Success,
	}

	[ProtoContract]
	public class C_Login_Login_0x0101 : BaseMessage
	{
		[ProtoMember(1)]
		public string _accountName;
		[ProtoMember(2)]
		public string _password;
	}

	[ProtoContract]
	public class S_Login_Login_0x0101 : BaseMessage
	{
		[ProtoMember(1)]
		public ELoginResult _rs;
	}

	[ProtoContract]
	public class C_Login_GetRooms_0x0102 : BaseMessage
	{

	}

	[ProtoContract]
	public class S_Login_GetRooms_0x0102 : BaseMessage
	{
		[ProtoMember(1)]
		public RoomInfo[] _rooms;
	}

	[ProtoContract]
	public class C_Login_CreateRoom_0x0103 : BaseMessage
	{
		[ProtoMember(1)]
		public string _roomName;
		[ProtoMember(2)]
		public int _mode;
	}

	[ProtoContract]
	public class S_Login_CreateRoom_0x0103 : BaseMessage
	{
		[ProtoMember(1)]
		public bool _rs;
		[ProtoMember(2)]
		public LobbyPlayerData[] _lobbyPlayerDatas;
	}

	[ProtoContract]
	public class C_Login_JoinRoom_0x0104 : BaseMessage
	{
		[ProtoMember(1)]
		public string _ownerAccountName;
	}

	[ProtoContract]
	public class S_Login_JoinRoom_0x0104 : BaseMessage
	{
		// 0 代表成功
		[ProtoMember(1)]
		public int _returnCode;
		[ProtoMember(2)]
		public LobbyPlayerData[] _lobbyPlayerDatas;
	}

	[ProtoContract]
	public class C_Login_Ready_0x0105 : BaseMessage
	{
		[ProtoMember(1)]
		public string _ownerAccountName;
	}

	[ProtoContract]
	public class S_Login_Ready_0x0105 : BaseMessage
	{
		// 0 代表成功
		[ProtoMember(1)]
		public int _returnCode;
	}

	[ProtoContract]
	public class C_Login_Register_0x0106 : BaseMessage
	{
		[ProtoMember(1)]
		public string _accountName;
		[ProtoMember(2)]
		public string _password;
		[ProtoMember(3)]
		public string _nickName;
	}

	[ProtoContract]
	public class S_Login_Register_0x0106 : BaseMessage
	{
		[ProtoMember(1)]
		public int _returnCode;
	}

	[ProtoContract]
	public class C_Login_Logout_0x0107 : BaseMessage
	{
		[ProtoMember(1)]
		public string _accountName;
	}

	[ProtoContract]
	public class S_Login_Logout_0x0107 : BaseMessage
	{
		[ProtoMember(1)]
		public int _returnCode;
	}

    //[ProtoContract]
    //public class C_Scene_Start_0x0301 : BaseMessage
    //{
    //	[ProtoMember(1)]
    //	public string _ownerAccountName;
    //}

    [ProtoContract]
	public class S_Scene_Start_0x0301 : BaseMessage
	{
		[ProtoMember(1)]
		public string _ownerAccountName;
		[ProtoMember(2)]
		public PlayerData[] _players;
		[ProtoMember(3)]
		public VisitorData[] _visitors;
		[ProtoMember(4)]
		public int _sceneId;
		[ProtoMember(5)]
		public int _mode; // 游戏模式，0计时，1计数
	}

	//[ProtoContract]
	//public class C_Scene_End_0x0302 : BaseMessage
	//{
	//	[ProtoMember(1)]
	//	public int _sceneId;
	//}

	[ProtoContract]
	public class S_Scene_End_0x0302 : BaseMessage
	{
		[ProtoMember(1)]
		public int _sceneId; 
	}

	[ProtoContract]
	public class S_Scene_Update_0x0303 : BaseMessage
	{
		[ProtoMember(1)]
		public string _ownerAccountName;
		[ProtoMember(2)]
		public PlayerData[] _players;
		[ProtoMember(3)]
		public VisitorData[] _visitors;
		[ProtoMember(4)]
		public int _sceneId;
		//[ProtoMember(5)]
		//public Dictionary<string, int> _scores = new Dictionary<string, int>();
	}

	[ProtoContract]
	public class C_Scene_Exit_0x0304 : BaseMessage
	{

	}

	[ProtoContract]
	public class S_Scene_Exit_0x0304 : BaseMessage
	{
		[ProtoMember(1)]
		public int _returnCode;
	}

	[ProtoContract]
	public class C_Scene_SavePlayer_0x0305 : BaseMessage
	{
		[ProtoMember(1)]
		public PlayerData _playerData; 
	}

	[ProtoContract]
	public class S_Scene_SavePlayer_0x0305 : BaseMessage
	{
		[ProtoMember(1)]
		public int _returnCode;
	}
    
	[ProtoContract]
	public class C_Scene_GetBlocks_0x0306 : BaseMessage
	{

	}

	[ProtoContract]
	public class S_Scene_GetBlocks_0x0306 : BaseMessage
	{
		[ProtoMember(1)]
		public BlockData[] _data;
		[ProtoMember(2)]
		public int _width;
		[ProtoMember(3)]
		public int _height;
	}

	[ProtoContract]
	public class C_Scene_ShowBlock_0x0307 : BaseMessage
	{
		[ProtoMember(1)]
		public int _index;
	}

	[ProtoContract]
	public class S_Scene_ShowBlock_0x0307 : BaseMessage
	{
		[ProtoMember(1)]
		public int _index;
		[ProtoMember(2)]
		public bool _isShow;
	}

	// 销毁的逻辑应该在服务器
	[ProtoContract]
	public class C_Scene_DestroyBlock_0x0308 : BaseMessage
	{
		[ProtoMember(1)]
		public int[] _indexs;
	}

	[ProtoContract]
	public class S_Scene_DestroyBlock_0x0308 : BaseMessage
	{
		[ProtoMember(1)]
		public int[] _indexs;
		[ProtoMember(2)]
		public Dictionary<string, int> _scores = new Dictionary<string, int>();
	}

    [ProtoContract]
    public class C_Scene_GetPlayerData_0x0309 : BaseMessage
    {
        [ProtoMember(1)]
        public string _accountName;
    }

    [ProtoContract]
    public class S_Scene_GetPlayerData_0x0309 : BaseMessage
    {
        [ProtoMember(1)]
        public PlayerData _playerData; 
    }

    [ProtoContract]
	public class S_Item_Gift_0x0401 : BaseMessage
	{
		[ProtoMember(1)]
		public int _itemId;
		[ProtoMember(2)]
		public int _count;
	}

    [ProtoContract]
    public class C_Item_GetAsset_0x0402 : BaseMessage
    {
        [ProtoMember(1)]
        public string _accountName;
    }

    [ProtoContract]
    public class S_Item_GetAsset_0x0402 : BaseMessage
    {
        [ProtoMember(1)]
        public AssetData _assetData; 
    }

    // 购买
    [ProtoContract]
    public class C_Item_BuyItem_0x0403 : BaseMessage
    {
        [ProtoMember(1)]
        public int _itemType; // 0 砖块Model， 1 遮盖板Block， 2 棋盘Board, 3 人物Person
        [ProtoMember(2)]
        public int _itemId; 
        [ProtoMember(3)]
        public int _count;
    }

    [ProtoContract]
    public class S_Item_BuyItem_0x0403 : BaseMessage
    {
        [ProtoMember(1)]
        public int _itemType;
        [ProtoMember(2)]
        public int _itemId; 
        [ProtoMember(3)]
        public int _code;
    }

    [ProtoContract]
    public class C_Item_GetGold_0x0404 : BaseMessage
    {

    }

    [ProtoContract]
    public class S_Item_GetGold_0x0404 : BaseMessage
    {
        [ProtoMember(1)]
        public int _num;
    }

    // 装扮
    [ProtoContract]
    public class C_Item_DecorateItem_0x0405 : BaseMessage
    {
        [ProtoMember(1)]
        public int _itemType; // 0 砖块Model， 1 遮盖板Block， 2 棋盘Board, 3 人物Person
        [ProtoMember(2)]
        public int _itemId;
        [ProtoMember(3)]
        public bool _isDecorate;
    }

    [ProtoContract]
    public class S_Item_DecorateItem_0x0405 : BaseMessage
    {
        [ProtoMember(1)]
        public int _itemType;
        [ProtoMember(2)]
        public int[] _itemIds;
        [ProtoMember(3)]
        public int[] _itemIdInUse; // 装备或卸下
    }
}


