//using ProtoBuf;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace RememberPos
//{
//	[ProtoContract]
//	public class VisitorData
//	{
//		[ProtoMember(1)]
//		public int _accountId;
//		[ProtoMember(2)]
//		public string _accountName;
//		[ProtoMember(3)]
//		public string _nickName;

//		public void SetValues(List<object> list)
//		{
//			if (list.Count != 3)
//			{
//				return;
//			}
//			_accountId = (int)list[0];
//			_accountName = (string)list[1];
//			_nickName = (string)list[2];
//		}
//	}
//}
