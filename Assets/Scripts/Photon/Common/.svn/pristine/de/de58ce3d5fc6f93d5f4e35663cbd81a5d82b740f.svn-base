using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf; 

namespace RememberPos
{
	[Serializable]
	[ProtoContract]
	public class PlayerData
	{
		[ProtoMember(1)]
		public int _accountId;
		[ProtoMember(2)]
		public string _accountName;
		[ProtoMember(3)]
		public string _nickName;
		[ProtoMember(4)]
		public int[] _blockIdsInUse; // 默认装扮全部使用0
        [ProtoMember(5)]
        public int _blockBoardIdInUse; // 默认装扮全部使用0
        [ProtoMember(6)]
		public int _boardIdInUse;
		[ProtoMember(7)]
		public int _personIdInUse;
        [ProtoMember(8)]
        public long _lastLoginTime;
        [ProtoMember(9)]
        public long _lastObtainGoldTime;

        public void SetValues(List<object> list)
		{
			if (list.Count != 9)
			{
#if SERVER
                Singleton._log.Info("list.Count != 7     list.Count=" + list.Count); 
#endif
				return;
			}
			_accountId = (int)list[0];
			_accountName = (string)list[1];
			_nickName = (string)list[2];
            _blockIdsInUse = ((string)list[3]).UnpackToDataInt();
            _blockBoardIdInUse = (int)list[4];
			_boardIdInUse = (int)list[5];
			_personIdInUse = (int)list[6];
            _lastLoginTime = (long)list[7];
            _lastObtainGoldTime = (long)list[8];
        }
	}
}
