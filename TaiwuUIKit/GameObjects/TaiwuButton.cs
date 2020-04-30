using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityUIKit.Core;
using UnityUIKit.GameObjects;

namespace TaiwuUIKit.GameObjects
{
    [YamlOnlySerializeSerializable]
    public class TaiwuButton : UnityUIKit.GameObjects.Button
    {
        // Load static background image
        private static readonly Image _image;
        private static readonly PointerEnter _pointerEnter;
        private static readonly PointerClick _pointerClick;

        public override Image Res_Image => _image;
        public virtual PointerEnter Res_PointerEnter => _pointerEnter;
        public virtual PointerClick Res_PointerClick => _pointerClick;

        static TaiwuButton()
        {
            var startBtn = Resources.Load<GameObject>("oldsceneprefabs/mianmenuback").transform.Find("MainMenu/StartMenuButton");
            _image = startBtn.GetComponent<Image>();
            _pointerEnter = startBtn.GetComponent<PointerEnter>();
            _pointerClick = startBtn.GetComponent<PointerClick>();
        }

        [YamlSerializable]
        public bool UseBoldFont
        {
            get
            {
                return (Label as BaseText).UseBoldFont;
            }
            set
            {
                (Label as BaseText).UseBoldFont = value;
            }
        }

        [YamlSerializable]
        public bool UseOutline
        {
            get
            {
                return (Label as BaseText).UseOutline;
            }
            set
            {
                (Label as BaseText).UseOutline = value;
            }
        }

        public override Label Label => m_Label;
        private BaseText m_Label = new BaseText();

        public override void Create(bool active)
        {
            Group.Padding = new List<int>() { 10, 20 };
            if(base.ImageColor == Color.clear)
                base.ImageColor = Res_Image.color;

            base.Create(active);

            if (Res_PointerClick != null)
            {
                var pc = Get<PointerClick>();
                pc.playSE = Res_PointerClick.playSE;
                pc.SEKey = Res_PointerClick.SEKey;
            }
            if (Res_PointerEnter != null)
            {
                PointerEnter pointerEnter = Get<PointerEnter>();
                pointerEnter.changeSize = Res_PointerEnter.changeSize;
                pointerEnter.restSize = Res_PointerEnter.restSize;
                pointerEnter.xMirror = Res_PointerEnter.xMirror;
                pointerEnter.yMirror = Res_PointerEnter.yMirror;
                pointerEnter.move = Res_PointerEnter.move;
                pointerEnter.moveX = Res_PointerEnter.moveX;
                pointerEnter.moveSize = Res_PointerEnter.moveSize;
                pointerEnter.restMoveSize = Res_PointerEnter.restMoveSize;
                pointerEnter.SEKey = Res_PointerEnter.SEKey;
                pointerEnter.changeTarget = Res_PointerEnter.changeTarget;
            }
        }
    }
}
