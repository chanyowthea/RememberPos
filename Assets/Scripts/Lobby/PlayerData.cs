//using System.Collections.Generic;
//using ProtoBuf;

//namespace RememberPos
//{
//	[ProtoContract]
//	public class PlayerData
//	{
//		[ProtoMember(1)]
//		public int _accountId;
//		[ProtoMember(2)]
//		public string _accountName;
//		[ProtoMember(3)]
//		public string _nickName;
//		[ProtoMember(4)]
//		public int _blockIdInUse;
//		[ProtoMember(5)]
//		public int _boardIdInUse;

//		public void SetValues(List<object> list)
//		{
//			if (list.Count != 5)
//			{
//				return;
//			}
//			_accountId = (int)list[0];
//			_accountName = (string)list[1];
//			_nickName = (string)list[2];
//			_blockIdInUse = (int)list[3];
//			_boardIdInUse = (int)list[4];
//		}
//	}
//}
