using Photon.SocketServer;
using RememberPos.Message;
using System.Collections;
using System.Collections.Generic;
using RememberPos.Utils;
using System.Linq;

namespace RememberPos
{
    public class MessageManager
    {
        public MessageManager()
        {
            Init();
        }

        ~MessageManager()
        {
            Clear();
        }

        public void Init()
        {
            Singleton._serverCallback.AddCallback<C_Login_Login_0x0101>(OnLogin);
            Singleton._serverCallback.AddCallback<C_Login_GetRooms_0x0102>(OnGetRooms);
            Singleton._serverCallback.AddCallback<C_Login_CreateRoom_0x0103>(OnCreateRoom);
            Singleton._serverCallback.AddCallback<C_Login_JoinRoom_0x0104>(OnJoinRoom);
            Singleton._serverCallback.AddCallback<C_Login_Ready_0x0105>(OnReady);
            Singleton._serverCallback.AddCallback<C_Login_Logout_0x0107>(OnLogout);
            Singleton._serverCallback.AddCallback<C_Login_Register_0x0106>(OnRegister);

            Singleton._serverCallback.AddCallback<C_Scene_Exit_0x0304>(OnExitScene);
            Singleton._serverCallback.AddCallback<C_Scene_SavePlayer_0x0305>(OnSavePlayer);
            Singleton._serverCallback.AddCallback<C_Scene_GetBlocks_0x0306>(OnGetBlocks);
            Singleton._serverCallback.AddCallback<C_Scene_ShowBlock_0x0307>(OnShowBlock);
            Singleton._serverCallback.AddCallback<C_Scene_GetPlayerData_0x0309>(OnGetPlayerData);

            Singleton._serverCallback.AddCallback<C_Item_GetAsset_0x0402>(OnGetAsset);
            Singleton._serverCallback.AddCallback<C_Item_BuyItem_0x0403>(OnBuyItem);
            Singleton._serverCallback.AddCallback<C_Item_GetGold_0x0404>(OnGetGold);
            Singleton._serverCallback.AddCallback<C_Item_DecorateItem_0x0405>(OnDecorateItem);
        }

        public void Clear()
        {
            Singleton._serverCallback.RemoveCallback<C_Login_Login_0x0101>(OnLogin);
            Singleton._serverCallback.RemoveCallback<C_Login_GetRooms_0x0102>(OnGetRooms);
            Singleton._serverCallback.RemoveCallback<C_Login_CreateRoom_0x0103>(OnCreateRoom);
            Singleton._serverCallback.RemoveCallback<C_Login_JoinRoom_0x0104>(OnJoinRoom);
            Singleton._serverCallback.RemoveCallback<C_Login_Ready_0x0105>(OnReady);
            Singleton._serverCallback.RemoveCallback<C_Login_Logout_0x0107>(OnLogout);
            Singleton._serverCallback.RemoveCallback<C_Login_Register_0x0106>(OnRegister);

            Singleton._serverCallback.RemoveCallback<C_Scene_Exit_0x0304>(OnExitScene);
            Singleton._serverCallback.RemoveCallback<C_Scene_SavePlayer_0x0305>(OnSavePlayer);
            Singleton._serverCallback.RemoveCallback<C_Scene_GetBlocks_0x0306>(OnGetBlocks);
            Singleton._serverCallback.RemoveCallback<C_Scene_ShowBlock_0x0307>(OnShowBlock);
            Singleton._serverCallback.RemoveCallback<C_Scene_GetPlayerData_0x0309>(OnGetPlayerData);

            Singleton._serverCallback.RemoveCallback<C_Item_GetAsset_0x0402>(OnGetAsset);
            Singleton._serverCallback.RemoveCallback<C_Item_BuyItem_0x0403>(OnBuyItem);
            Singleton._serverCallback.RemoveCallback<C_Item_GetGold_0x0404>(OnGetGold);
            Singleton._serverCallback.RemoveCallback<C_Item_DecorateItem_0x0405>(OnDecorateItem);
        }

        public void Send<T>(Client peer, T t) where T : BaseMessage
        {
            OperationResponse opResponse = new OperationResponse(0);
            var parameter = new Dictionary<byte, object>();
            parameter.Add(0, t.GetMessageID());
            var temp = Singleton._serializer.Serialize<T>(t);
            parameter.Add(1, temp);
            opResponse.SetParameters(parameter);
            peer.SendOperationResponse(opResponse, new SendParameters());
        }

        public void SendFromServer<T>(Client peer, T t) where T : BaseMessage
        {
            // 客户端没有发送请求给服务器，服务器直接发送数据给客户端
            EventData ed = new EventData(0); // 客户端有opCode，那么服务器之间发送就需要eventCode，这我们也设置eventcode为1
            var parameter = new Dictionary<byte, object>();
            parameter.Add(0, t.GetMessageID());
            var temp = Singleton._serializer.Serialize<T>(t);
            parameter.Add(1, temp);
            ed.Parameters = parameter;
            peer.SendEvent(ed, new SendParameters());
        }

        void OnLogin(Client peer, C_Login_Login_0x0101 msg)
        {
            Singleton._log.InfoFormat("[{0}] 帐号 {1} 登录！", msg.GetMessageID().ToHex()
                , (msg == null ? "null" : msg._accountName));
            var rs = Singleton._sqlServer.CheckAccount(msg._accountName, msg._password);
            ELoginResult newRs = ELoginResult.Failed_AccountNotExist;
            switch (rs)
            {
                case Sql.EAccountResult.AccountNotExist:
                    newRs = ELoginResult.Failed_AccountNotExist;
                    break;
                case Sql.EAccountResult.PasswordNotCorrect:
                    newRs = ELoginResult.Failed_Unknown;
                    break;
                case Sql.EAccountResult.Success:
                    newRs = ELoginResult.Success;
                    break;
            }
            if (newRs == ELoginResult.Success)
            {
                if (Singleton._clients.ContainsKey(msg._accountName))
                {
                    // 如果同名则踢出前面登录客户端
                    var client = Singleton._clients[msg._accountName];
                    // 在Disconnect中会移除这个名字
                    Singleton._log.Info("connectId = " + peer.ConnectionId + ", 移除 " + msg._accountName);
                    Singleton._clients.Remove(msg._accountName);
                    client.Disconnect();
                }

                Singleton._log.Info("connectId = " + peer.ConnectionId + ", 添加 " + msg._accountName);
                Singleton._clients[msg._accountName] = peer;
                peer._accountName = msg._accountName;

                // 登录赠送金币1， 每天只限一次
                var p = Singleton._sqlServer.GetPlayerData(msg._accountName);
                // 设置登录时间
                p._lastLoginTime = Singleton._timeUtil.DateTimeToStamp(System.DateTime.Now);
                var time = Singleton._timeUtil.StampToDateTime(p._lastObtainGoldTime);
                var now = System.DateTime.Now;
                Singleton._log.Info("now=" + now + ", last=" + time);
                if (Singleton._timeUtil.GetTimeGap(now, time) >= 24 * 3600 // 如果时间间隔超过24小时
                    || (time.Day - now.Day > 0)) // 或者在24小时以内但不在同一天登录的
                {
                    p._lastObtainGoldTime = Singleton._timeUtil.DateTimeToStamp(System.DateTime.Now);
                    Singleton._log.Info("增加金币数1！");
                    var a = Singleton._sqlServer.GetAssetData(msg._accountName);
                    a._gold += 1;
                    Singleton._sqlServer.ChangeAsset(a);
                }
                Singleton._sqlServer.SavePlayer(p);
            }

            // 返回数据
            Send(peer, new S_Login_Login_0x0101 { _rs = newRs });

            // 测试
            // System.Threading.Thread.Sleep(3000);
            // peer.Disconnect(); 
            // SendFromServer(peer, new S_Item_Gift_0x0401{ _itemId = 1, _count = 10});  
        }

        void OnGetRooms(Client peer, C_Login_GetRooms_0x0102 msg)
        {
            Singleton._log.Info("C_Login_GetRooms_0x0102" + ", client=" + peer._accountName);
            Send(peer, new S_Login_GetRooms_0x0102
            {
                _rooms = Singleton._roomManager.GetAllRooms()
            });
        }

        void OnCreateRoom(Client peer, C_Login_CreateRoom_0x0103 msg)
        {
            if (!Singleton._roomManager.Create(peer, msg._roomName, msg._mode))
            {
                return;
            }

            Singleton._log.Info("C_Login_CreateRoom_0x0103" + ", client=" + peer._accountName);
            Send(peer, new S_Login_CreateRoom_0x0103
            {
                _rs = true,
                _lobbyPlayerDatas = Singleton._roomManager.GetLobbyPlayerDataByAccountName(peer._accountName)
            });
        }

        void OnJoinRoom(Client peer, C_Login_JoinRoom_0x0104 msg)
        {
            if (!Singleton._roomManager.Join(peer._accountName, msg._ownerAccountName))
            {
                return;
            }

            Singleton._log.Info("C_Login_JoinRoom_0x0104" + ", client=" + peer._accountName);
            Singleton._log.Info("S_Login_JoinRoom_0x0104" + ", client=" + peer._accountName);
            Send(peer, new S_Login_JoinRoom_0x0104
            {
                _returnCode = 0,
                _lobbyPlayerDatas = Singleton._roomManager.GetLobbyPlayerDataByAccountName(msg._ownerAccountName)
            });
        }

        void OnReady(Client peer, C_Login_Ready_0x0105 msg)
        {
            var rs = Singleton._roomManager.Ready(peer._accountName, msg._ownerAccountName);
            Singleton._log.Info("C_Login_Ready_0x0105" + ", client=" + peer._accountName);
            Singleton._log.Info("S_Login_Ready_0x0105" + ", client=" + peer._accountName);
            Send(peer, new S_Login_Ready_0x0105
            {
                _returnCode = rs,
            });
        }

        void OnLogout(Client peer, C_Login_Logout_0x0107 msg)
        {
            int rs = 0;
            if (peer == null)
            {
                rs = 1;
            }
            peer.Disconnect();
            Singleton._log.Info("C_Login_Logout_0x0107" + ", client=" + peer._accountName);
            Singleton._log.Info("S_Login_Logout_0x0107" + ", client=" + peer._accountName);
            Send(peer, new S_Login_Logout_0x0107
            {
                _returnCode = rs,
            });
        }

        void OnRegister(Client peer, C_Login_Register_0x0106 msg)
        {
            Singleton._log.Info("C_Login_Register_0x0106" + ", client=" + peer._accountName);
            int rs = 0;
            if (peer == null)
            {
                rs = 1;
            }
            if (string.IsNullOrEmpty(msg._accountName))
            {
                rs = 2;
            }

            Singleton._log.InfoFormat("[{0}] 帐号 {1} 注册！", msg.GetMessageID().ToHex()
                , (msg == null ? "null" : msg._accountName));
            var newRs = Singleton._sqlServer.CheckAccount(msg._accountName, msg._password);
            if (newRs != Sql.EAccountResult.AccountNotExist)
            {
                // 帐号已存在
                rs = 3;
            }
            var playerData = new PlayerData();
            playerData._accountName = msg._accountName;
            playerData._nickName = msg._nickName;
            Singleton._sqlServer.Register(playerData, msg._password);
            Singleton._log.Info("connectId = " + peer.ConnectionId + ", 添加 " + msg._accountName);
            Singleton._clients[msg._accountName] = peer;
            peer._accountName = msg._accountName;
            Singleton._log.Info("S_Login_Register_0x0106" + ", client=" + peer._accountName);
            Send(peer, new S_Login_Register_0x0106
            {
                _returnCode = rs,
            });
        }

        void OnGetBlocks(Client peer, C_Scene_GetBlocks_0x0306 msg)
        {
            Singleton._log.Info("C_Scene_GetBlocks_0x0306" + ", client=" + peer._accountName);
            int id = Singleton._sceneManager.GetSceneID(peer._accountName);
            if (id < 0)
            {
                return;
            }
            var s = Singleton._sceneManager.GetScene(id);
            if (s == null)
            {
                return;
            }

            var rs = Singleton._sceneManager.GetBlocks(id);
            if (rs == null)
            {
                return;
            }

            Singleton._log.Info("S_Scene_GetBlocks_0x0306" + ", client=" + peer._accountName);
            Send(peer, new S_Scene_GetBlocks_0x0306
            {
                _data = rs,
                _width = s._width,
                _height = s._height
            });
        }

        void OnSavePlayer(Client peer, C_Scene_SavePlayer_0x0305 msg)
        {
            Singleton._log.Info("C_Scene_SavePlayer_0x0305" + ", client=" + peer._accountName);
            var rs = Singleton._sqlServer.SavePlayer(msg._playerData);
            Singleton._log.Info("S_Scene_SavePlayer_0x0305" + ", client=" + peer._accountName);
            Send(peer, new S_Scene_SavePlayer_0x0305
            {
                _returnCode = rs
            });
        }

        void OnShowBlock(Client peer, C_Scene_ShowBlock_0x0307 msg)
        {
            Singleton._log.Info("C_Scene_ShowBlock_0x0307" + ", client=" + peer._accountName);
            var rs = Singleton._sceneManager.ShowBlock(peer, Singleton._sceneManager.GetSceneID(peer._accountName), msg._index);
            //Singleton._log.Info("S_Scene_ShowBlock_0x0307" + ", client=" + peer._accountName);
            //Send(peer, new S_Scene_ShowBlock_0x0307
            //{
            //    _index = rs, _isShow = rs == 0
            //});
        }

        void OnExitScene(Client peer, C_Scene_Exit_0x0304 msg)
        {
            Singleton._log.Info("C_Scene_Exit_0x0304" + ", client=" + peer._accountName);
            var rs = Singleton._sceneManager.ExitScene(Singleton._sceneManager.GetSceneID(peer._accountName), peer._accountName);
            Singleton._log.Info("S_Scene_Exit_0x0304" + ", client=" + peer._accountName);
            Send(peer, new S_Scene_Exit_0x0304
            {
                _returnCode = rs
            });
        }

        void OnGetAsset(Client peer, C_Item_GetAsset_0x0402 msg)
        {
            Singleton._log.Info("C_Item_GetAsset_0x0402" + ", client=" + peer._accountName);
            var data = Singleton._sqlServer.GetAssetData(msg._accountName);
            Singleton._log.Info("S_Item_GetAsset_0x0402" + ", client=" + peer._accountName);
            Send(peer, new S_Item_GetAsset_0x0402
            {
                _assetData = data
            });
        }

        void OnGetPlayerData(Client peer, C_Scene_GetPlayerData_0x0309 msg)
        {
            Singleton._log.Info("C_Scene_GetPlayerData_0x0309" + ", client=" + peer._accountName);
            var data = Singleton._sqlServer.GetPlayerData(msg._accountName);
            Singleton._log.Info("S_Scene_GetPlayerData_0x0309" + ", client=" + peer._accountName);
            Send(peer, new S_Scene_GetPlayerData_0x0309
            {
                _playerData = data
            });
        }

        void OnDecorateItem(Client peer, C_Item_DecorateItem_0x0405 msg)
        {
            Singleton._log.InfoFormat("C_Item_DecorateItem_0x0405" + ", client={0}, type={1}, id={2}", 
                peer._accountName, msg._itemType, msg._itemId);
            var rs = Singleton._sqlServer.DecorateItem(peer._accountName, msg._itemType, msg._isDecorate, msg._itemId);
            if (rs != 0)
            {
                Singleton._log.Info("OnDecorateItem" + ", rs=" + rs);
                return; 
            }

            var p = Singleton._sqlServer.GetPlayerData(peer._accountName);
            var a = Singleton._sqlServer.GetAssetData(peer._accountName);
            if (a == null)
            {
                Singleton._log.Info("OnDecorateItem p == null");
                return; 
            }
            Singleton._log.Info("S_Item_DecorateItem_0x0405" + ", client=" + peer._accountName);
            int[] inUseIds = null;
            int[] ownedIds = null; 
            switch (msg._itemType)
            {
                case 0:
                    inUseIds = p._blockIdsInUse; 
                    ownedIds = a._blocks;
                    break;
                case 1:
                    inUseIds = new int[1] { p._blockBoardIdInUse };
                    ownedIds = a._blockBoards; 
                    break;
                case 2:
                    inUseIds = new int[1] { p._boardIdInUse };
                    ownedIds = a._boards;
                    break;
                case 3:
                    inUseIds = new int[1] { p._personIdInUse };
                    ownedIds = a._persons;
                    break;
                default:
                    Singleton._log.InfoFormat("DecorateItem 找不到对应类型 type={0}! ", msg._itemType);
                    break; 
            }
            Send(peer, new S_Item_DecorateItem_0x0405
            {
                _itemType = msg._itemType, _itemIds = ownedIds, _itemIdInUse = inUseIds
            });
        }



        // 价格表
        // 根据价格处理
        // 金币要减少
        void OnBuyItem(Client peer, C_Item_BuyItem_0x0403 msg)
        {
            Singleton._log.Info("C_Item_BuyItem_0x0403" + ", client=" + peer._accountName);
            int code = 0; 
            var data = Singleton._sqlServer.GetAssetData(peer._accountName);
            if (data == null)
            {
                code = 11;
            }
            else
            {
                List<int> list = new List<int>();
                switch (msg._itemType)
                {
                    // 购买砖块模型
                    case 0:
                        list.Clear();
                        list.AddRange(data._blocks);
                        var rs = list.Any((int temp) => temp == msg._itemId);
                        if (rs)
                        {
                            // 已经购买该物品
                            code = 10;
                            break;
                        }

                        var b0 = Singleton._excelUtil.GetBlock(msg._itemId);
                        if (b0 == null)
                        {
                            // 表中找不到该物品
                            code = 13;
                            break;
                        }
                        var finalCost = b0._gold * msg._count;
                        if (data._gold < finalCost)
                        {
                            // 余额不足
                            code = 12;
                            break;
                        }
                        data._gold -= finalCost;
                        Singleton._log.InfoFormat("账户{0}扣除金币{1}, 剩余金币{2}", peer._accountName, finalCost, data._gold);
                        list.Add(msg._itemId);
                        data._blocks = list.ToArray();
                        break;
                    // 购买砖块遮盖板
                    case 1:
                        list.Clear();
                        list.AddRange(data._blockBoards);
                        rs = list.Any((int temp) => temp == msg._itemId);
                        // 如果已经拥有就无法购买
                        // 此处是指只能拥有一个的物品
                        if (rs)
                        {
                            // 已经购买该物品
                            code = 10;
                            break;
                        }

                        var b1 = Singleton._excelUtil.GetBlockBoard(msg._itemId);
                        if (b1 == null)
                        {
                            // 表中找不到该物品
                            code = 13;
                            break;
                        }
                        finalCost = b1._gold * msg._count;
                        if (data._gold < finalCost) 
                        {
                            // 余额不足
                            code = 12;
                            break; 
                        }
                        data._gold -= finalCost;
                        Singleton._log.InfoFormat("账户{0}扣除金币{1}, 剩余金币{2}", peer._accountName, finalCost, data._gold); 
                        list.Add(msg._itemId);
                        data._blockBoards = list.ToArray();
                        break;
                    // 购买棋盘
                    case 2:
                        list.Clear();
                        list.AddRange(data._boards);
                        rs = list.Any((int temp) => temp == msg._itemId);
                        // 如果已经拥有就无法购买
                        // 此处是指只能拥有一个的物品
                        if (rs)
                        {
                            // 已经购买该物品
                            code = 10;
                            break;
                        }

                        var b2 = Singleton._excelUtil.GetBoard(msg._itemId);
                        if (b2 == null)
                        {
                            // 表中找不到该物品
                            code = 13;
                            break;
                        }
                        finalCost = b2._gold * msg._count;
                        if (data._gold < finalCost)
                        {
                            // 余额不足
                            code = 12;
                            break;
                        }
                        data._gold -= finalCost;
                        Singleton._log.InfoFormat("账户{0}扣除金币{1}, 剩余金币{2}", peer._accountName, finalCost, data._gold);
                        list.Add(msg._itemId);
                        data._boards = list.ToArray();
                        break;
                    // 购买人物
                    case 3:
                        list.Clear();
                        list.AddRange(data._persons);
                        rs = list.Any((int temp) => temp == msg._itemId);
                        // 如果已经拥有就无法购买
                        // 此处是指只能拥有一个的物品
                        if (rs)
                        {
                            // 已经购买该物品
                            code = 10;
                            break;
                        }

                        var b3 = Singleton._excelUtil.GetPerson(msg._itemId);
                        if (b3 == null)
                        {
                            // 表中找不到该物品
                            code = 13;
                            break;
                        }
                        finalCost = b3._gold * msg._count;
                        if (data._gold < finalCost)
                        {
                            // 余额不足
                            code = 12;
                            break;
                        }
                        data._gold -= finalCost;
                        Singleton._log.InfoFormat("账户{0}扣除金币{1}, 剩余金币{2}", peer._accountName, finalCost, data._gold);
                        list.Add(msg._itemId);
                        data._persons = list.ToArray();
                        break;
                }
            }
            if (code == 0)
            {
                code = Singleton._sqlServer.ChangeAsset(data);
            }

            // 更新客户端金币
            Singleton._log.Info("S_Item_GetGold_0x0404" + ", client=" + peer._accountName);
            int num = 0;
            var a = Singleton._sqlServer.GetAssetData(peer._accountName);
            if (a != null)
            {
                num = a._gold;
            }

            Send(peer, new S_Item_GetGold_0x0404
            {
                _num = num
            });

            Singleton._log.Info("S_Item_BuyItem_0x0403" + ", client=" + peer._accountName);
            Send(peer, new S_Item_BuyItem_0x0403
            {
                _itemType = msg._itemType,
                _itemId = msg._itemId,
                _code = code
            });
        }

        void OnGetGold(Client peer, C_Item_GetGold_0x0404 msg)
        {
            Singleton._log.Info("C_Item_GetGold_0x0404" + ", client=" + peer._accountName);
            Singleton._log.Info("S_Item_GetGold_0x0404" + ", client=" + peer._accountName);
            int num = 0;
            var a = Singleton._sqlServer.GetAssetData(peer._accountName);
            if (a != null)
            {
                num = a._gold; 
            }
            Send(peer, new S_Item_GetGold_0x0404
            {
                _num = num
            });
        }
    }
}
