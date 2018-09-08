using RememberPos.Message;
using System.Collections;
using System.Collections.Generic;

namespace RememberPos
{
	public class MessageManager
	{
		public void Send<T>(T t) where T : BaseMessage
		{
			// parameter 0对应id，1对应数据
			var parameter = new Dictionary<byte, object>();
			parameter.Add(0, t.GetMessageID());
			var temp = Singleton._serializer.Serialize<T>(t);
			parameter.Add(1, temp);
			Singleton._serverManager.peer.OpCustom(0, parameter, true);
		}
	}
}
