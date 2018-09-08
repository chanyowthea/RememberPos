//using System;
//using System.Collections.Generic;
//using RememberPos.Message;

//namespace RememberPos
//{
//	internal interface IProtoHandler
//	{
//		void Execute(object obj);
//	}

//	public class ProtoHandler<T> : IProtoHandler where T : BaseMessage
//	{
//		public Action<T> OnReceive;

//		public void Execute(object obj)
//		{
//			var tempData = (byte[])obj;
//			T v = Singleton._serializer.DeSerialize<T>(tempData);
//			if (v != null && OnReceive != null)
//			{
//#if UNITY_EDITOR
//				OnReceive(v);
//#else
//				try
//				{
//					OnReceive(v);
//				}
//				catch (Exception ex)
//				{
//					UnityEngine.Debug.LogException(ex);
//				}
//#endif
//			}
//		}
//	}

//	public class ServerCallback
//	{
//		Dictionary<int, IProtoHandler> sdict;

//		public ServerCallback()
//		{
//			sdict = new Dictionary<int, IProtoHandler>();
//		}


//		int protoName2ProtoID<T>()
//		{
//			var name = typeof(T).FullName;
//			int idx = name.LastIndexOf('_');
//			string sub = name.Substring(idx + 1);
//			int n = Convert.ToInt32(sub, 16);
//			return n;
//		}

//		public void AddCallback<T>(Action<T> proc) where T : BaseMessage
//		{
//			var n = protoName2ProtoID<T>();
//			IProtoHandler iph;
//			bool b = sdict.TryGetValue(n, out iph);
//			if (b)
//			{
//				ProtoHandler<T> pht = iph as ProtoHandler<T>;
//				pht.OnReceive -= proc;
//				pht.OnReceive += proc;
//			}
//			else
//			{
//				ProtoHandler<T> pht = new ProtoHandler<T>();
//				sdict.Add(n, pht);
//				pht.OnReceive -= proc;
//				pht.OnReceive += proc;
//			}
//		}

//		public void RemoveCallback<T>(Action<T> proc) where T : BaseMessage
//		{
//			var n = protoName2ProtoID<T>();
//			IProtoHandler iph;
//			if (sdict.TryGetValue(n, out iph))
//			{
//				ProtoHandler<T> pht = iph as ProtoHandler<T>;
//				pht.OnReceive -= proc;
//			}
//		}

//		public void HandleProtoMessage(int id, object msg)
//		{
//			IProtoHandler handler;
//			if (sdict.TryGetValue(id, out handler))
//			{
//				handler.Execute(msg);
//			}
//		}
//	}
//}

