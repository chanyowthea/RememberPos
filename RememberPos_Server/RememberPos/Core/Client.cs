using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;
using System.Collections.Generic;
using RememberPos.Message;
using RememberPos.Utils; 

namespace RememberPos
{
	public class Client : ClientPeer
	{
		public string _accountName;

		public Client(InitRequest initRequest) : base(initRequest)
		{
			Singleton._log.Info("客户端上线 ip=" + LocalIP + ", connectId=" + ConnectionId);
		}

		//客户端断开连接
		protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
		{
			Singleton._log.Info("客户端下线 ip=" + LocalIP + ", accountName="
				+ _accountName + ", connectId=" + ConnectionId);
			if (string.IsNullOrEmpty(_accountName))
			{
				return;
			}

			// 确定是本Peer才执行删除操作，由于前面栈中，可能在添加之后才移除，导致新数据被移除了，
			// 但是在客户端主动断线时，这里确实应该判断是否移除
			if (Singleton._clients.ContainsKey(_accountName) && Singleton._clients[_accountName].ConnectionId == ConnectionId)
			{
				Singleton._log.Info("connectId = " + ConnectionId + ", 移除 " + _accountName);
				Singleton._clients.Remove(_accountName);
			}

			Singleton._roomManager._Destroy(_accountName);
            Singleton._sceneManager.ExitScene(Singleton._sceneManager.GetSceneID(_accountName), _accountName); 
		}

		//处理客户端的请求
		protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
		{
			switch (operationRequest.OperationCode)
			{
				case 0:
					// 获取数据
					Dictionary<byte, object> data = operationRequest.Parameters;
					object id;
					if (!data.TryGetValue(0, out id))
					{
						Singleton._log.InfoFormat("get id {0} failed! ", id);
						return;
					}
					object tempData;
					if (!data.TryGetValue(1, out tempData))
					{
						Singleton._log.InfoFormat("get data with id {0} failed! ", id);
						return;
					}
					Singleton._log.Info("HandleProtoMessage id=" + ((int)id).ToHex());
					Singleton._serverCallback.HandleProtoMessage(this, (int)id, tempData);
					break;
				default:
					break;
			}
		}
	}
}
