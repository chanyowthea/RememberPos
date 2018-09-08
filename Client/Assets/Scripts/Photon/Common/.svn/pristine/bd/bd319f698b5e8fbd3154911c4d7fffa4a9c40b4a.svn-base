using System.Collections;
using System.Collections.Generic;
using ProtoBuf;

namespace RememberPos
{
	[ProtoContract]
	public class RoomInfo
	{
#if SERVER
		public static int s_id; 
		public RoomInfo()
		{
			++s_id;
			_roomId = s_id; 
		}
#endif

		[ProtoMember(1)]
		public string _roomName;
		[ProtoMember(2)]
		public string _ip;
		[ProtoMember(3)]
		public string _ownerAccountName;

		[ProtoMember(4)]
#if SERVER
		public int _roomId { private set; get; }
#else
		public int _roomId;
#endif
		[ProtoMember(5)]
		public List<LobbyPlayerData> _lobbyPlayerDatas = new List<LobbyPlayerData>();
		[ProtoMember(6)]
		public int _mode; 
	}
}
