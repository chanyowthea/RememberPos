using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace RememberPos
{
	public class _Serializer
	{
		public byte[] Serialize<T>(T data)
		{
			try
			{
				//内存流实例
				using (MemoryStream ms = new MemoryStream())
				{
					//ProtoBuf协议序列化数据对象
					ProtoBuf.Serializer.Serialize<T>(ms, data);
					//创建临时结果数组
					byte[] result = new byte[ms.Length];
					//调整游标位置为0
					ms.Position = 0;
					//开始读取，从0到尾
					ms.Read(result, 0, result.Length);
					//返回结果
					return result;
				}
			}
			catch (Exception ex)
			{
#if SERVER
				Singleton._log.Info("Serialize ex=" + ex.Message); 
#endif
				return null;
			}
		}

		public T DeSerialize<T>(byte[] data)
		{
			try
			{
				//内存流实例
				using (MemoryStream ms = new MemoryStream(data))
				{
					//调整游标位置
					ms.Position = 0;
					//ProtoBuf协议反序列化数据
					T mod = ProtoBuf.Serializer.Deserialize<T>(ms);
					//返回数据对象
					return mod;

				}
			}
			catch (Exception ex)
			{
#if SERVER
				Singleton._log.Info("DeSerialize ex=" + ex.Message);
#endif
				return default(T);
			}
		}
	}
}
