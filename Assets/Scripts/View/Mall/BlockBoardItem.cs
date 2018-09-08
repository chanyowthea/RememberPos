using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

namespace Mall
{
    public class BlockBoardVO : BaseVO
    {
        public int _id;
        public event Action<int> _onCountChanged;
        int _count;
        public int count
        {
            set
            {
                _count = value;
                if (_onCountChanged != null)
                {
                    _onCountChanged(value);
                }
            }
            get
            {
                return _count;
            }
        }
    }

    public class BlockBoardItem : BaseItem
    {
        public BlockBoardVO vo;
        [SerializeField] GameObject _buyObj;
        [SerializeField] GameObject _ownObj;
        [SerializeField] Image _tex;

        public Sprite tex
        {
            set
            {
                _tex.sprite = value;
            }
            get
            {
                return _tex.sprite;
            }
        }

        public override void Show()
        {
            if (vo == null)
            {
                return;
            }
            base._vo = (BaseVO)vo;
            vo._onCountChanged += OnCountChange;
            OnCountChange(vo.count);
            var conf = Singleton._gameReference._blockBoardLib.GetBlockBoard(vo._id);
            if (conf != null)
            {
                tex = conf._sprite;
            }
            base.Show();
        }

        public override void Hide()
        {
            vo._onCountChanged -= OnCountChange;
            base.Hide();
        }

        void OnCountChange(int value)
        {
            if (vo == null)
            {
                return;
            }
            bool have = value != 0;
            _buyObj.SetActive(!have);
            _ownObj.SetActive(have);
        }
    }
}