using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

namespace Decorate
{
    public class BlockVO : BaseVO
    {
        public int _id;
        public event Action<bool> _onEquipped;
        bool _isEquipped;
        public bool isEquipped
        {
            set
            {
                _isEquipped = value;
                if (_onEquipped != null)
                {
                    _onEquipped(value);
                }
            }
            get
            {
                return _isEquipped;
            }
        }
    }

    public class BlockItem : BaseItem
    {
        [SerializeField] GameObject _buyObj;
        [SerializeField] GameObject _ownObj;
        [SerializeField] Image _tex;
        public BlockVO vo;
        Sprite tex
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
            vo._onEquipped += OnEquipped;
            OnEquipped(vo.isEquipped);
            var conf = Singleton._gameReference._blockLib.GetBlock(vo._id);
            if (conf != null)
            {
                tex = conf._sprite;
            }
            base.Show();
        }

        public override void Hide()
        {
            vo._onEquipped -= OnEquipped;
            base.Hide();
        }

        void OnEquipped(bool value)
        {
            if (vo == null)
            {
                return;
            }
            _buyObj.SetActive(!value);
            _ownObj.SetActive(value);
        }
    }
}
