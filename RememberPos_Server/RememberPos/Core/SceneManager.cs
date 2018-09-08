using RememberPos.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// 角色拥有的Block种类，Board种类，Person种类
// 添加好友
// 角色信息：金币数，拥有的各类型砖块等，
// 赠送，
// 在商城中可以销售自己的砖块（按照时间价格排序，按照帐号名搜索）


namespace RememberPos
{
    public class SceneManager
    {
        public Dictionary<int, Scene> _scenes = new Dictionary<int, Scene>();
        public int StartScene(out int sceneId, string ownerAccountName, PlayerData[] players, VisitorData[] visitors, int mode)
        {
#if !TEST
            if (players == null || players.Length < 2)
            {
                sceneId = 0;
                Singleton._log.Info("人数过少无法开始！");
                return 1;
            }
#endif

            Singleton._log.Info("StartScene players.Length=" + players.Length);
            var s = new Scene();
            sceneId = s._id;
            s._ownerAccountName = ownerAccountName;
            s._mode = mode;
            for (int i = 0, length = players == null ? 0 : players.Length; i < length; i++)
            {
                var p = players[i];
                if (s._players.ContainsKey(p._accountName))
                {
                    continue;
                }
                Singleton._log.Info("StartScene p._accountName=" + p._accountName);
                s._players.Add(p._accountName, p);
            }
            for (int i = 0, length = visitors == null ? 0 : players.Length; i < length; i++)
            {
                var v = visitors[i];
                s._visitors.Add(v._accountName, v);
            }

            // 创建砖块
            if (players != null && players.Length >= 2)
            {
                int[] b0 = players[0]._blockIdsInUse;
                int[] b1 = players[1]._blockIdsInUse;
                var bs = GenerateBlocks(s._id, s._width, s._height, b0, b1);
                for (int i = 0, length = bs.Length; i < length; i++)
                {
                    var b = bs[i];
                    s._blocks.Add(b._index, b);
                }
            }
            _scenes.Add(s._id, s);
            Singleton._log.Info("开始场景！场景ID=" + sceneId);
            return 0;
        }

        public int ExitScene(int sceneId, string accountName)
        {
            Singleton._log.InfoFormat("离开场景！场景ID={0}, accountName={1}", sceneId, accountName);
            if (!_scenes.ContainsKey(sceneId))
            {
                return 1;
            }
            var s = _scenes[sceneId];
            // 删除玩家
            if (s._players.ContainsKey(accountName))
            {
                s._players.Remove(accountName);
            }
            else
            {
                // 删除游客
                if (s._visitors.ContainsKey(accountName))
                {
                    s._visitors.Remove(accountName);
                }
                else
                {
                    Singleton._log.Info("离开场景！场景ID=" + sceneId);
                    return 2;
                }
            }

            if (s._players.Count == 0)
            {
                EndScene(sceneId);
            }
            else
            {
                UpdateScene(s._id);
            }
            return 0;
        }

        int EndScene(int sceneId)
        {
            Singleton._log.Info("结束场景！sceneId=" + sceneId);
            if (!_scenes.ContainsKey(sceneId))
            {
                return 1;
            }
            _scenes.Remove(sceneId);
            foreach (var c in Singleton._clients)
            {
                Singleton._messageManager.SendFromServer(c.Value, new S_Scene_End_0x0302
                {
                    _sceneId = sceneId
                });
            }
            return 0;
        }

        int UpdateScene(int sceneId)
        {
            Singleton._log.Info("更新场景！场景ID=" + sceneId);
            if (!_scenes.ContainsKey(sceneId))
            {
                Singleton._log.Info("场景不存在！场景ID=" + sceneId);
                return 1;
            }

            var s = _scenes[sceneId];
            foreach (var c in Singleton._clients)
            {
                Singleton._messageManager.SendFromServer(c.Value, new S_Scene_Update_0x0303
                {
                    _ownerAccountName = s._ownerAccountName,
                    _players = new List<PlayerData>(s._players.Values).ToArray(),
                    _sceneId = s._id,
                    _visitors = new List<VisitorData>(s._visitors.Values).ToArray(),
                });
            }

            return 0;
        }

        public Scene GetScene(int id)
        {
            if (_scenes.ContainsKey(id))
            {
                return _scenes[id];
            }

            Singleton._log.Info("找不到场景！场景ID=" + id);
            return null;
        }

        public int HideBlock(int sceneId, int index)
        {
            if (!_scenes.ContainsKey(sceneId))
            {
                // 不存在对应场景
                Singleton._log.Info("不存在对应场景！sceneId=" + sceneId);
                return 1;
            }
            var s = _scenes[sceneId];
            if (!s._blocks.ContainsKey(index))
            {
                // 不存在对应砖块
                Singleton._log.Info("不存在对应砖块！index=" + index);
                return 2;
            }

            var b = s._blocks[index];
            var showedBlock = s._showedBlocks.Find((BlockData temp) => temp._index == b._index);
            if (showedBlock == null)
            {
                // 该砖块已经隐藏
                Singleton._log.Info("该砖块已经隐藏！index=" + index);
                return 3;
            }
            Singleton._log.InfoFormat("隐藏砖块！sceneId={0}, index={1}", sceneId, index);
            b.isShow = false;
            s._showedBlocks.Remove(showedBlock);
            UpdateBlock(b._index, b._isShow);
            return 0;
        }

        public int ShowBlock(Client executive, int sceneId, int index)
        {
            if (executive == null)
            {
                // 客户端为空
                Singleton._log.Info("客户端为空！");
                return 1;
            }
            if (!Singleton._clients.ContainsKey(executive._accountName))
            {
                // 不存在对应客户端
                Singleton._log.Info("不存在对应客户端！accountName=" + executive._accountName);
                return 2;
            }
            if (!_scenes.ContainsKey(sceneId))
            {
                // 不存在对应场景
                Singleton._log.Info("不存在对应场景！sceneId=" + sceneId);
                return 3;
            }
            var s = _scenes[sceneId];
            var time = Singleton._timeUtil.GetTimeGap(DateTime.Now, s._lastShowedBlockTime);
            if (time < s._gapTime)
            {
                // 冷却时间内
                Singleton._log.Info("冷却时间内！time=" + time);
                return 4;
            }
            s._lastShowedBlockTime = DateTime.Now;
            if (!s._blocks.ContainsKey(index))
            {
                // 不存在对应砖块
                Singleton._log.Info("不存在对应砖块！index=" + index);
                return 5;
            }
            var b = s._blocks[index];
            if (s._showedBlocks.Find((BlockData temp) => temp._index == b._index) != null)
            {
                // 该砖块已经显示
                Singleton._log.Info("该砖块已经显示！index=" + index);
                return 6;
            }

            Singleton._log.InfoFormat("显示砖块！executive={0}, sceneId={1}, index={2}", executive._accountName, sceneId, index);
            b.isShow = true;
            s._showedBlocks.Add(b);
            UpdateBlock(b._index, b._isShow);

            // 超过显示数量，把前面的隐藏
            if (s._showedBlocks.Count > s._maxCount)
            {
                var p = s._showedBlocks[0];
                p.isShow = false;
                s._showedBlocks.RemoveAt(0);
                UpdateBlock(p._index, p._isShow);
                Singleton._log.Info("超过显示数量，把前面的隐藏！index=" + p._index);
            }

            foreach (var temp in s._blocks)
            {
                if (temp.Value._itemId == b._itemId && temp.Value._isShow && temp.Key != b._index)
                {
                    // 销毁砖块
                    DestroyBlock(executive, s._id, b._index, temp.Key);
                    return 7;
                }
            }
            return 0;
        }

        public int DestroyBlock(Client client, int sceneId, params int[] indexs)
        {
            if (!_scenes.ContainsKey(sceneId))
            {
                // 不存在对应场景
                Singleton._log.Info("不存在对应场景！sceneId=" + sceneId);
                return 1;
            }

            if (indexs == null || indexs.Length == 0)
            {
                Singleton._log.Info("DestroyBlock indexs数量为0！");
                return 2;
            }

            if (client == null || !Singleton._clients.ContainsKey(client._accountName))
            {
                Singleton._log.Info("DestroyBlock client is empty！");
                return 3;
            }

            var s = _scenes[sceneId];
            for (int i = 0, length = indexs.Length; i < length; i++)
            {
                var index = indexs[i];
                if (!s._blocks.ContainsKey(index))
                {
                    // 不存在对应砖块
                    Singleton._log.Info("不存在对应砖块！index=" + index);
                    return 3;
                }
                var b = s._blocks[index];
                b.isShow = false;
                var showedBlock = s._showedBlocks.Find((BlockData temp) => temp._index == b._index);
                if (showedBlock != null)
                {
                    s._showedBlocks.Remove(showedBlock);
                }
                s._blocks.Remove(b._index);
            }

            // 为销毁本砖块的客户端加分
            if (!s._scores.ContainsKey(client._accountName))
            {
                s._scores.Add(client._accountName, 0);
            }
            s._scores[client._accountName] += 1;

            foreach (var c in Singleton._clients)
            {
                Singleton._messageManager.SendFromServer(c.Value, new S_Scene_DestroyBlock_0x0308
                {
                    _indexs = indexs,
                    _scores = s._scores
                });
            }
            return 0;
        }

        BlockData[] GenerateBlocks(int sceneId, int width, int height, int[] ownedBlocks0, int[] ownedBlocks1)
        {
            // 合并两者数据
            List<int> bs = new List<int>();
            bs.AddRange(ownedBlocks0);
            for (int i = 0, length = ownedBlocks1.Length; i < length; i++)
            {
                var b = ownedBlocks1[i];
                if (!bs.Contains(b))
                {
                    bs.Add(b);
                }
            }

            // 总是成对出现的
            List<int> allType = new List<int>();
            for (int i = 0; i < height * width / 2; i++)
            {
                // 随机取出一个元素，并删除这个元素，下次不再随机到该元素
                var index = Singleton._randomUtil.GetNext(bs.Count, 0);
                allType.Add(bs[index]); // TODO 这里要根据玩家数据来设置，两个玩家的并集
                //bs.RemoveAt(index);
            }
            List<int> allIndex = new List<int>();
            allIndex.AddRange(allType);
            for (int i = 0; i < height * width / 2; i++)
            {
                // 随机选择其中一个，并且从allType里面删除这个index，下次再随机取出其他
                var pickIndex = Singleton._randomUtil.GetNext(allType.Count);
                allIndex.Add(allType[pickIndex]);
                allType.RemoveAt(pickIndex);
            }

            List<BlockData> blocks = new List<BlockData>();
            for (int i = 0, length = allIndex.Count; i < length; i++)
            {
                var b = new BlockData();
                b._itemId = allIndex[i];
                b._index = i;
                b._sceneId = sceneId;
                blocks.Add(b);
            }
            return blocks.ToArray();
        }

        public BlockData[] GetBlocks(int sceneId)
        {
            if (!_scenes.ContainsKey(sceneId))
            {
                // 不存在对应场景
                Singleton._log.Info("不存在对应场景！sceneId=" + sceneId);
                return null;
            }
            var s = _scenes[sceneId];
            Singleton._log.Info("GetBlocks！sceneId=" + sceneId);
            return new List<BlockData>(s._blocks.Values).ToArray();
        }

        int UpdateBlock(int index, bool isShow)
        {
            foreach (var c in Singleton._clients)
            {
                Singleton._messageManager.SendFromServer(c.Value, new S_Scene_ShowBlock_0x0307
                {
                    _index = index,
                    _isShow = isShow
                });
            }
            return 0;
        }

        public int GetSceneID(string accountName)
        {
            foreach (var s in _scenes)
            {
                // TODO 游客怎么办？
                if (s.Value._players.ContainsKey(accountName))
                {
                    return s.Key;
                }
            }
            Singleton._log.Info("找不到场景 accountName=" + accountName);
            return -1;
        }
    }
}
