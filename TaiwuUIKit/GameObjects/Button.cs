// This file is part of the TaiwuTools <https://github.com/vizv/TaiwuTools/>.
// Copyright (C) 2020  Taiwu Modding Community Members
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System.Collections.Generic;
using UnityUIKit.Core;
using UnityUIKit.Core.GameObjects;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityUIKit.GameObjects;

namespace TaiwuUIKit.GameObjects
{
    public class LayoutIgnorer : MonoBehaviour, ILayoutIgnorer
    {
        public bool ignoreLayout => true;
    }

    public class TaiwuTogleButton : BaseTogleButton
    {
        public override Image Res_Image => null;
        public virtual PointerEnter Res_PointerEnter => null;
        public virtual PointerClick Res_PointerClick => null;

        public bool UseBoldFont = false;

        public override void Create(bool active = true)
        {
            base.ImageColor = Res_Image.color;
            base.Label = new BaseText()
            {
                Name = $"{Name}:Text",
                Text = Text,
                Alignment = Alignment,
                UseBoldFont = UseBoldFont,
                UseOutline = true,
            };

            base.Create(active);
            if(Res_PointerEnter != null)
            {
                var pe = Get<PointerEnter>();
                pe.changeSize = Res_PointerEnter.changeSize;
                pe.restSize = Res_PointerEnter.restSize;
                pe.xMirror = Res_PointerEnter.xMirror;
                pe.yMirror = Res_PointerEnter.yMirror;
                pe.move = Res_PointerEnter.move;
                pe.moveX = Res_PointerEnter.moveX;
                pe.moveSize = Res_PointerEnter.moveSize;
                pe.restMoveSize = Res_PointerEnter.restMoveSize;
                pe.SEKey = Res_PointerEnter.SEKey;
                pe.changeTarget = Res_PointerEnter.changeTarget;
            }
        }
    }

    public class TaiwuButton : TaiwuTogleButton
    {
        // Load static background image
        private static readonly Image _image;
        private static readonly PointerEnter _pointerEnter;
        private static readonly PointerClick _pointerClick;

        public override Image Res_Image => _image;
        public override PointerEnter Res_PointerEnter => _pointerEnter;
        public override PointerClick Res_PointerClick => _pointerClick;

        static TaiwuButton()
        {
            var startBtn = Resources.Load<GameObject>("oldsceneprefabs/mianmenuback").transform.Find("MainMenu/StartMenuButton");
            _image = startBtn.GetComponent<Image>();
            _pointerEnter = startBtn.GetComponent<PointerEnter>();
            _pointerClick = startBtn.GetComponent<PointerClick>();
        }

        private Button.ButtonClickedEvent m_OnClick = new Button.ButtonClickedEvent();
        public Button.ButtonClickedEvent onClick
        {
            get
            {
                if (Destroyed)
                    return m_OnClick;
                return Get<Button>().onClick;
            }
            set
            {
                m_OnClick = value;
                if (Created)
                    Get<Button>().onClick = m_OnClick;
            }
        }

        public override void Create(bool active = true)
        {
            Group.Padding = new List<int>() { 10, 20 };

            base.Create(active);

            var bt = Get<Button>();
            bt.onClick = m_OnClick;
            bt.image = Get<Image>();

            if (Res_PointerClick != null)
            {
                var pc = Get<PointerClick>();
                pc.playSE = Res_PointerClick.playSE;
                pc.SEKey = Res_PointerClick.SEKey;
            }
        }
    }

    public class CloseButton : TaiwuButton
    {
        // Load static background image
        private static readonly Image _image;
        private static readonly PointerEnter _pointerEnter;
        private static readonly PointerClick _pointerClick;

        public override Image Res_Image => _image;
        public override PointerEnter Res_PointerEnter => _pointerEnter;
        public override PointerClick Res_PointerClick => _pointerClick;

        static CloseButton()
        {
            var exitButton = Resources.Load<GameObject>("prefabs/ui/views/ui_systemsetting").transform.Find("SystemSetting/Close");
            _image = exitButton.GetComponent<CButton>().image;
            _pointerEnter = exitButton.GetComponent<PointerEnter>();
            _pointerClick = exitButton.GetComponent<PointerClick>();
        }

        public void Click()
        {
            if(Res_PointerClick != null)
                AudioManager.instance.PlaySE(Res_PointerClick.SEKey);
            var button = Get<Button>();
            var p = new PointerEventData(EventSystem.current);
            p.button = PointerEventData.InputButton.Left;
            button.OnPointerClick(p);
        }


        public override void Create(bool active = true)
        {
            base.Text = null;
            base.Create(active);

            Get<LayoutIgnorer>();
            GameObject.Destroy(Get<LayoutElement>());

            RectTransform.sizeDelta = new Vector2(40, 40);
            RectTransform.anchorMax = new Vector2(1, 1);
            RectTransform.anchorMin = RectTransform.anchorMax;
            RectTransform.anchoredPosition = Vector2.zero;
        }
    }

    public class TaiwuToggle : TaiwuTogleButton
    {
        // Load static background image
        private static readonly Image _image_Off;
        private static readonly Image _image_On;
        private static readonly PointerEnter _pointerEnter;
        private static readonly PointerClick _pointerClick;
        private static readonly ColorBlock _colors;

        public virtual Image Res_Image_On => _image_On;
        public override Image Res_Image => _image_Off;
        public override PointerEnter Res_PointerEnter => _pointerEnter;
        public override PointerClick Res_PointerClick => _pointerClick;
        public virtual ColorBlock Res_Colors => _colors;

        static TaiwuToggle()
        {
            var SetScreenToggle = Resources.Load<GameObject>("prefabs/ui/views/ui_systemsetting").transform.Find("SystemSetting/SetScreen/FullScreenToggle,702");
            _image_Off = SetScreenToggle.GetComponent<CToggle>().image;
            _colors = SetScreenToggle.GetComponent<CToggle>().colors;
            _pointerEnter = SetScreenToggle.GetComponent<PointerEnter>();
            _pointerClick = SetScreenToggle.GetComponent<PointerClick>();
            SetScreenToggle = SetScreenToggle.transform.Find("Label");
            _image_On = SetScreenToggle.GetComponent<CImage>() as Image;
        }

        private Toggle.ToggleEvent m_OnValueChanged = new Toggle.ToggleEvent();
        public Toggle.ToggleEvent onValueChanged
        {
            get
            {
                if (Destroyed)
                    return m_OnValueChanged;
                return Get<Toggle>().onValueChanged;
            }
            set
            {
                m_OnValueChanged = value;
                if (Created)
                    Get<Toggle>().onValueChanged = m_OnValueChanged;
            }
        }


        private bool m_IsOn = false;
        public bool isOn
        {
            get
            {
                if (Destroyed)
                    return m_IsOn;
                return Get<Toggle>().isOn;
            }
            set
            {
                m_IsOn = value;
                if (Created)
                    Get<Toggle>().isOn = m_IsOn;
            }
        }


        public List<float> PreferredSize = new List<float> { 0, 50 };

        private ColorBlock m_Colors = ColorBlock.defaultColorBlock;
        public ColorBlock colors
        {
            get
            {
                if (Destroyed)
                    return m_Colors;
                return Get<Toggle>().colors;
            }
            set
            {
                m_Colors = value;
                if (Created)
                {
                    Get<Toggle>().colors = m_Colors;
                }
            }
        }

        public override void Create(bool active = true)
        {
            Element.PreferredSize = PreferredSize;
            if (colors == ColorBlock.defaultColorBlock)
                colors = Res_Colors;

            base.Create(active);

            var a = new GameObject("Label");
            var b = a.AddComponent<Image>();
            b.sprite = Res_Image_On.sprite;
            b.type = Res_Image_On.type;
            b.color = Res_Image_On.color;

            var Toggle = Get<Toggle>();
            Toggle.image = Get<Image>();
            Toggle.transition = Selectable.Transition.ColorTint;
            Toggle.graphic = Toggle.image;
            Toggle.onValueChanged = m_OnValueChanged;
            Toggle.isOn = m_IsOn;


            if (Res_PointerClick != null)
            {
                var pc = Get<PointerClick>();
                pc.playSE = Res_PointerClick.playSE;
                pc.SEKey = Res_PointerClick.SEKey;
            }
        }
    }
}
