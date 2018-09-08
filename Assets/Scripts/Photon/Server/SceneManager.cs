using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using RememberPos.Message; 

namespace RememberPos
{
	public class SceneManager
	{
		public string _ownerAccountName; 
		public int _sceneId; 
		public List<Player> _players = new List<Player>(); 
        //public Dictionary<string, AssetData> _assetDatas = new Dictionary<string, AssetData>(); 

        // 一个客户端同时只能存在一个Scene
        public void StartScene()
		{
			Singleton._serverCallback.AddCallback<S_Scene_Update_0x0303>(OnUpdateScenes);
            Singleton._serverCallback.AddCallback<S_Scene_ShowBlock_0x0307>(OnShowBlock); 
            Singleton._serverCallback.AddCallback<S_Scene_DestroyBlock_0x0308>(OnDestroyBlock); 
            Debug.LogError("start scene players.Count=" + Singleton._players.Count); 
			foreach (var p in Singleton._players)
			{
				var conf = Singleton._gameReference._personLib.GetPerson(p.Value._personIdInUse);
                Debug.LogError("start scene p.Value._personId=" + p.Value._personIdInUse);
                if (conf == null)
				{
					continue; 
				}
				var go = GameObject.Instantiate(conf._prefab).GetComponent<Player>();
				go.playerData = p.Value;
				go.name = p.Value._accountName;
				_players.Add(go); 
			}
		}

		public void EndScene(int sceneId)
		{
            if (_sceneId != sceneId)
            {
                return; 
            }

			for (int i = 0, length = _players.Count; i < length; i++)
			{
				var p = _players[i]; 
				GameObject.Destroy(p.gameObject); 
			}
			_players.Clear();
			Singleton._players.Clear();
			_ownerAccountName = null;
			Singleton._serverCallback.RemoveCallback<S_Scene_Update_0x0303>(OnUpdateScenes);
            Singleton._serverCallback.RemoveCallback<S_Scene_ShowBlock_0x0307>(OnShowBlock);
            Singleton._serverCallback.RemoveCallback<S_Scene_DestroyBlock_0x0308>(OnDestroyBlock);
        }

		public void EnterScene(PlayerData arg)
		{

		}

		public void ExitScene(string accountName)
		{

		}

		void OnUpdateScenes(S_Scene_Update_0x0303 msg)
		{
			if(msg._sceneId != _sceneId)
			{
				Debug.LogErrorFormat("sceneId不匹配, msg._sceneId={0}, this._sceneId={1}", msg._sceneId, _sceneId);
				return; 
			}

            if (msg._players == null)
            {
                return; 
            }

			int max = Mathf.Max(msg._players.Length, _players.Count);
			var deleteList = new List<Player>();
			var addList = new List<PlayerData>();

			// 筛选添加进入的角色
			for (int i = 0, length = msg._players.Length; i < length; i++)
			{
				var p = msg._players[i]; 
				bool isExist = false; 
				for (int j = 0, max1 = _players.Count; j < max1; j++)
				{
					var p1 = _players[j];
					if(p1._accountName == p._accountName)
					{
						isExist = true;
						break; 
					}
				}
				if(!isExist)
				{
					addList.Add(p); 
				}
			}

			// 筛选删除的角色
			for (int i = 0, length = _players.Count; i < length; i++)
			{
				var p1 = _players[i];
				bool isExist = false;
				for (int j = 0, max1 = msg._players.Length; j < max1; j++)
				{
					var p = msg._players[j]; 
					if (p1._accountName == p._accountName)
					{
						isExist = true;
						break;
					}
				}
				if (!isExist)
				{
					deleteList.Add(p1);
				}
			}

			// 创建添加的角色
			for (int i = 0, length = addList.Count; i < length; i++)
			{
				var a = addList[i];
				var conf = Singleton._gameReference._personLib.GetPerson(a._personIdInUse);
				if (conf == null)
				{
					continue;
				}
				var go = GameObject.Instantiate(conf._prefab).GetComponent<Player>();
				go.playerData = a;
				go.name = a._accountName; 
				_players.Add(go);
			}

			// 创建删除的角色
			for (int i = 0, length = deleteList.Count; i < length; i++)
			{
				var d = deleteList[i];
				_players.Remove(d); 
				GameObject.Destroy(d.gameObject); 
			}
		}

        void OnShowBlock(S_Scene_ShowBlock_0x0307 msg)
        {
            Debug.Log("[S_Scene_ShowBlock_0x0307]" + "index=" + msg._index + ", isShow=" + msg._isShow);
            var b = Singleton._blockGenerator._blocks.Find((Block temp) => temp._index == msg._index);
            if (b != null)
            {
                b.isShowModel = msg._isShow;
            }
        }

        void OnDestroyBlock(S_Scene_DestroyBlock_0x0308 msg)
        {
            Debug.Log("[S_Scene_DestroyBlock_0x0308]" + "index count=" + msg._indexs.Length);
            for (int i = 0, length = msg._indexs.Length; i < length; i++)
            {
                var index = msg._indexs[i];
                var b = Singleton._blockGenerator._blocks.Find((Block temp) => temp._index == index);
                Debug.LogError("Destroy b=" + (b == null));
                if (b != null)
                {
                    Debug.LogError("Destroy index=" + b._index);
                    b.DestroySelf();
                }
            }
            foreach (var item in msg._scores)
            {
                var p = Singleton._sceneManager._players.Find((Player temp) => temp._accountName == item.Key);
                if (p == null)
                {
                    continue;
                }
                p.SetScores(item.Value);
            }
        }

        public void GetAssetData(string accountName, Action<AssetData> action)
        {
            //if (Singleton._sceneManager._assetDatas.ContainsKey(accountName))
            //{
            //    var a = Singleton._sceneManager._assetDatas[accountName];
            //    if (action != null)
            //    {
            //        action(a); 
            //    }
            //}
            //else
            //{
                Singleton._sceneService.GetAsset(accountName, (AssetData temp) =>
                {
                    if (temp == null)
                    {
                        Debug.LogError("GetAssetData temp is empty! "); 
                        return; 
                    }

                    //Singleton._sceneManager._assetDatas.Add(accountName, temp);
                    if (action != null)
                    {
                        action(temp);
                    }
                });
            //}
        }
    }
}
