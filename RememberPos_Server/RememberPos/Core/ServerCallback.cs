using System;
using System.Collections.Generic;
using RememberPos.Message;
using Photon.SocketServer;

namespace RememberPos
{
	internal interface IProtoHandler
	{
		void Execute(Client peer, object obj);
	}

	public class ProtoHandler<T> : IProtoHandler where T : BaseMessage
	{
		public Action<Client, T> OnReceive;

		public void Execute(Client peer, object obj)
		{
			var tempData = (byte[])obj;
			T v = Singleton._serializer.DeSerialize<T>(tempData);
			if (v != null && OnReceive != null)
			{
#if UNITY_EDITOR
				OnReceive(v);
#else
				try
				{
					OnReceive(peer, v);
				}
				catch (Exception ex)
				{
					Singleton._log.Info("Execute " + ex);
				}
#endif
			}
		}
	}

	public class ServerCallback
	{
		Dictionary<int, IProtoHandler> sdict;

		public ServerCallback()
		{
			sdict = new Dictionary<int, IProtoHandler>();
		}


		int protoName2ProtoID<T>()
		{
			var name = typeof(T).FullName;
			int idx = name.LastIndexOf('_');
			string sub = name.Substring(idx + 1);
			int n = Convert.ToInt32(sub, 16);
			return n;
		}

		public void AddCallback<T>(Action<Client, T> proc) where T : BaseMessage
		{
			var n = protoName2ProtoID<T>();
			IProtoHandler iph;
			bool b = sdict.TryGetValue(n, out iph);
			if (b)
			{
				ProtoHandler<T> pht = iph as ProtoHandler<T>;
				pht.OnReceive -= proc;
				pht.OnReceive += proc;
			}
			else
			{
				ProtoHandler<T> pht = new ProtoHandler<T>();
				sdict.Add(n, pht);
				pht.OnReceive -= proc;
				pht.OnReceive += proc;
			}
		}

		public void RemoveCallback<T>(Action<Client, T> proc) where T : BaseMessage
		{
			var n = protoName2ProtoID<T>();
			IProtoHandler iph;
			if (sdict.TryGetValue(n, out iph))
			{
				ProtoHandler<T> pht = iph as ProtoHandler<T>;
				pht.OnReceive -= proc;
			}
		}

		public void HandleProtoMessage(Client peer, int id, object msg)
		{
			IProtoHandler handler;
			if (sdict.TryGetValue(id, out handler))
			{
				handler.Execute(peer, msg);
			}
		}
	}
}

