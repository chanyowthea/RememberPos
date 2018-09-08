using System;
using System.Collections;
using System.Collections.Generic;

namespace RememberPos.Message
{
	public class BaseMessage
	{
		public int GetMessageID()
		{
			string s = GetType().FullName;
			int idx = s.LastIndexOf('_');
			string sub = s.Substring(idx + 1);
			return Convert.ToInt32(sub, 16);
		}
	}
}
