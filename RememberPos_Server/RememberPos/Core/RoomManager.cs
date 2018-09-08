using RememberPos.Message;
using System;
using System.Collections.Generic;

namespace RememberPos
{
	// 仅仅指大厅房间
	public class RoomManager
	{
		public Dictionary<string, RoomInfo> _rooms = new Dictionary<string, RoomInfo>();

		public RoomInfo GetRoomInfo(string accountName)
		{
			if (!_rooms.ContainsKey(accountName))
			{
				return null;
			}
			return _rooms[accountName];
		}

		public RoomInfo[] GetAllRooms()
		{
			List<RoomInfo> infos = new List<RoomInfo>();
			foreach (var item in _rooms)
			{
				infos.Add(item.Value);
			}
			return infos.ToArray();
		}

		public LobbyPlayerData[] GetLobbyPlayerDataByAccountName(string accountName)
		{
			if (!_rooms.ContainsKey(accountName))
			{
				return null;
			}
			return _rooms[accountName]._lobbyPlayerDatas.ToArray();
		}

		void UpdateRooms()
		{
			Singleton._log.Info("UpdateRooms"); 
			// 通知每个客户端更新房间数据
			foreach (var c in Singleton._clients)
			{
				Singleton._messageManager.SendFromServer(c.Value, new S_Login_GetRooms_0x0102
				{
					_rooms = GetAllRooms()
				});
			}
		}

		public bool Create(Client c, string roomName, int mode)
		{
			if (c == null)
			{
				Singleton._log.Info("创建房间失败！client为空！");
				return false;
			}

			if (string.IsNullOrEmpty(roomName))
			{
				Singleton._log.Info("创建房间失败！roomName为空！");
				return false;
			}

			if (_rooms.ContainsKey(c._accountName))
			{
				Singleton._log.Info("创建房间失败！房间已存在！_ownerAccountName=" + c._accountName);
				return false;
			}

			var info = new RoomInfo();
			info._roomName = roomName;
			info._ip = c.RemoteIP;
			info._ownerAccountName = c._accountName;
            info._mode = mode; 
			_rooms[info._ownerAccountName] = info;

			Singleton._log.InfoFormat("创建房间成功！roomName={0}, _ownerAccountName={1}, mode={2}", roomName, c._accountName, mode);
			// 将房主信息加入RoomInfo
			info._lobbyPlayerDatas.Add(Singleton._sqlServer.GetLobbyPlayerData(c._accountName));
			UpdateRooms();
			return true;
		}

		public bool _Destroy(string ownerAccountName)
		{
			if (string.IsNullOrEmpty(ownerAccountName))
			{
				Singleton._log.Info("account name is empty！");
				return false;
			}

			var info = GetRoomInfo(ownerAccountName);
			if (info == null)
			{
				Singleton._log.Info("获取房间信息失败！ ownerAccountName=" + ownerAccountName);
				return false;
			}

			if (_rooms.ContainsKey(ownerAccountName))
			{
				Singleton._log.Info("销毁房间成功！ ownerAccountName=" + ownerAccountName);
				_rooms.Remove(ownerAccountName);
			}
			UpdateRooms();
			return true;
		}

		public bool Join(string accountName, string ownerAccountName)
		{
			if (string.IsNullOrEmpty(accountName) || string.IsNullOrEmpty(ownerAccountName))
			{
				Singleton._log.Info("account name is empty！");
				return false;
			}
			var info = GetRoomInfo(ownerAccountName);
			if (info == null)
			{
				Singleton._log.Info("获取房间信息失败！ ownerAccountName=" + ownerAccountName);
				return false;
			}

			Singleton._log.InfoFormat("加入房间成功！ accountName={0}, ownerAccountName={1}", accountName, ownerAccountName);
			info._lobbyPlayerDatas.Add(Singleton._sqlServer.GetLobbyPlayerData(accountName));
			UpdateRooms();
			return true;
		}

		public bool Leave(string accountName, string ownerAccountName)
		{
			if (string.IsNullOrEmpty(accountName) || string.IsNullOrEmpty(ownerAccountName))
			{
				Singleton._log.Info("account name is empty！");
				return false;
			}
			var info = GetRoomInfo(ownerAccountName);
			if (info == null)
			{
				Singleton._log.Info("获取房间信息失败！ ownerAccountName=" + ownerAccountName);
				return false;
			}

			var data = info._lobbyPlayerDatas.Find((LobbyPlayerData temp) => temp._accountName == accountName);
			if (data != null)
			{
				Singleton._log.Info("离开房间成功！ ownerAccountName=" + ownerAccountName);
				info._lobbyPlayerDatas.Remove(data);
			}
			UpdateRooms();
			return true;
		}

		public int Ready(string accountName, string ownerAccountName)
		{
			if (string.IsNullOrEmpty(accountName) || string.IsNullOrEmpty(ownerAccountName))
			{
				Singleton._log.Info("account name is empty！");
				// 帐号名为空
				return 1;
			}
			var info = GetRoomInfo(ownerAccountName);
			if (info == null)
			{
				Singleton._log.Info("获取房间信息失败！ ownerAccountName=" + ownerAccountName);
				// 获取房间信息失败
				return 2;
			}

			if(info._lobbyPlayerDatas.Count < 1) 
			{
				Singleton._log.InfoFormat("人数不足，等待其他玩家加入！ accountName={0}, ownerAccountName={1}"
					, accountName, ownerAccountName);
				// 人数不足
				return 3;
			}

			Singleton._log.InfoFormat("准备成功！ accountName={0}, ownerAccountName={1}", accountName, ownerAccountName);
			var data = info._lobbyPlayerDatas.Find((LobbyPlayerData temp) => temp._accountName == accountName);
			data._isReady = true; 
			UpdateRooms();

			bool isAllReady = true;
			for (int i = 0, length = info._lobbyPlayerDatas.Count; i < length; i++)
			{
				if(!info._lobbyPlayerDatas[i]._isReady)
				{
					isAllReady = false;
					break; 
				}
			}
			if(isAllReady)
			{
                int id;
                List<PlayerData> ps = new List<PlayerData>();
                for (int i = 0, length = info._lobbyPlayerDatas.Count; i < length; i++)
                {
                    var newInfo = info._lobbyPlayerDatas[i];
                    ps.Add(Singleton._sqlServer.GetPlayerData(newInfo._accountName));
                }
                int code = Singleton._sceneManager.StartScene(out id, info._ownerAccountName, ps.ToArray(), null, info._mode); 
                
				// 进入场景，清空大厅相关角色
				foreach(var c in Singleton._clients)
				{
					var rs = info._lobbyPlayerDatas.Find((LobbyPlayerData temp) => temp._accountName == c.Value._accountName);
					if(rs == null)
					{
						continue; 
					}
					Singleton._messageManager.SendFromServer(c.Value, new S_Scene_Start_0x0301
					{
						_ownerAccountName = ownerAccountName, 
						_players = ps.ToArray(),
					    _visitors = null, 
                        _sceneId = id, 
                        _mode = info._mode
                    });
				}
				_Destroy(ownerAccountName); 
			}
			// 成功
			return 0;
		}
	}
}
