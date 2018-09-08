using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

namespace Mall
{
    public class BlockBoardRect : MonoBehaviour
    {
        [SerializeField] ScrollRect _scrollView;
        [SerializeField] BlockBoardItem _itemPrefab;
        [SerializeField] Transform _itemContent;
        List<BlockBoardItem> _items = new List<BlockBoardItem>();
        public List<BlockBoardVO> _vos = new List<BlockBoardVO>();

        public void Open()
        {
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
            var cells = Singleton._excelUtil.GetBlockBoardTable().ToArray();
            for (int i = 0, length = _items.Count; i < length; i++)
            {
                var b = _items[i];
                b.Hide();
                GameObject.Destroy(b.gameObject);
            }
            _items.Clear();
            _vos.Clear();

            for (int i = 0, length = cells.Length; i < length; i++)
            {
                var c = cells[i];
                BlockBoardVO vo = new BlockBoardVO();
                vo._id = c._id;
                vo.name0 = c._name;
                vo.name1 = c._gold.ToString();
                vo.onClick0 = () => Singleton._sceneService.BuyItem(1, vo._id, 1, (int code) =>
                {
                    Debug.Log("BuyItem code=" + code);
                    if (code == 0)
                    {
                        vo.count = 1;
                    }
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

        void OnSetItem(bool value)
        {
            if (value)
            {
                _scrollView.verticalNormalizedPosition = 1;
                CreateItems();
            }
        }
    }
}
