using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RememberPos.Utils
{
	public static class MessageUtil
	{
		public static string ToHex(this int num)
		{
			return "0x" + System.Convert.ToString(num, 16);
		}
	}
}
