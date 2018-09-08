using RememberPos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Decorate
{
    public class BoardRect : MonoBehaviour
    {
        [SerializeField] ScrollRect _scrollView;
        [SerializeField] BoardItem _itemPrefab;
        [SerializeField] Transform _itemContent;
        List<BoardItem> _items = new List<BoardItem>();
        public List<BoardVO> _vos = new List<BoardVO>();

        public void Open()
        {
            _scrollView.verticalNormalizedPosition = 1;
            _itemPrefab.gameObject.SetActive(false);
            CreateItems();
            gameObject.SetActive(true);
        }

        public void Close()
        {
            DestroyItems();
            gameObject.SetActive(false);
        }

        void CreateItems()
        {
            AssetData data = null;
            PlayerData playerData = null;
            int state = 0;
            Action<int> onState = (int index) =>
            {
                state |= 1 << index;
                if (state == 3)
                {
                    OnCreateItem(data, playerData);
                }
            };
            Singleton._sceneService.GetPlayerData(Singleton._accountName, (PlayerData temp) =>
            {
                playerData = temp;
                onState(0);
            });

            Singleton._sceneService.GetAsset(Singleton._accountName, (AssetData temp) =>
            {
                data = temp;
                onState(1);
            });
        }

        void OnCreateItem(AssetData data, PlayerData playerData)
        {
            if (data == null || playerData == null)
            {
                return;
            }

            var bs = data._boards;
            for (int i = 0, length = _items.Count; i < length; i++)
            {
                var b = _items[i];
                b.Hide();
                GameObject.Destroy(b.gameObject);
            }
            _items.Clear();
            _vos.Clear();

            for (int i = 0, length = bs.Length; i < length; i++)
            {
                var c = bs[i];
                var cell = Singleton._excelUtil.GetBoard(c);
                if (cell == null)
                {
                    continue;
                }
                BoardVO vo = new BoardVO();
                vo._id = cell._id;
                vo.name0 = cell._name;
                vo.name1 = cell._gold.ToString();
                
                if (playerData._boardIdInUse == cell._id)
                {
                    vo.count = 1;
                }

                // 装备
                vo.onClick0 = () => Singleton._sceneService.EquipItem(2, vo._id, true, (int[] inUse, int[] owneds) =>
                {
                    Debug.LogError("EquipItem true");
                    UpdateEquip(inUse);
                });
                // 卸下
                vo.onClick1 = () => Singleton._sceneService.EquipItem(2, vo._id, false, (int[] inUse, int[] owneds) =>
                {
                    UpdateEquip(inUse);
                });
                _vos.Add(vo);

                var b = GameObject.Instantiate(_itemPrefab);
                b.gameObject.SetActive(true);
                b.transform.SetParent(_itemContent);
                b.transform.localScale = Vector3.one;
                b.vo = vo;
                b.Show();
                _items.Add(b);
            }
        }

        void DestroyItems()
        {
            for (int i = 0, length = _items.Count; i < length; i++)
            {
                var b = _items[i];
                b.Hide();
                GameObject.Destroy(b.gameObject);
            }
            _items.Clear();
            _vos.Clear();
        }

        void UpdateEquip(int[] ids)
        {
            foreach (var item in _vos)
            {
                item.count = 0;
            }

            for (int j = 0, max = ids.Length; j < max; j++)
            {
                var use = ids[j];
                var v = _vos.Find((BoardVO temp) => temp._id == use);
                if (v != null)
                {
                    v.count = 1;
                }
            }
        }
    }
}