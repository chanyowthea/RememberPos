using System;
using System.Collections.Generic;
using System.Linq;

namespace RememberPos
{
    public static class TextUtil
    {
        public static int[] UnpackToDataInt(this string msg)
        {
            string[] ss = msg.Split(';');
            List<int> list = new List<int>();
            for (int i = 0, length = ss.Length; i < length; i++)
            {
                int rs;
                if (int.TryParse(ss[i], out rs))
                {
                    list.Add(rs);
                }
            }
            return list.ToArray(); 
        }

        public static string PackToStringInt(this int[] datas)
        {
            string s = ""; 
            if (datas == null)
            {
                return s; 
            }

            for (int i = 0, length = datas.Length; i < length; i++)
            {
                var d = datas[i]; 
                s += (i == 0 ? "" : ";") + d; 
            }
            return s; 
        }
    }
}
