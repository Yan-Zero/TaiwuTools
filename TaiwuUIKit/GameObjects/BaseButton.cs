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

namespace TaiwuUIKit.GameObjects
{
    public class BaseButton : BoxGroupGameObject
    {
        // Load static background image
        private static readonly Image image;
        private static readonly PointerEnter pointerEnter;
        static BaseButton()
        {
            var startBtn = Resources.Load<GameObject>("oldsceneprefabs/mianmenuback").transform.Find("MainMenu/StartMenuButton");
            image = startBtn.GetComponent<Image>();
            pointerEnter = startBtn.GetComponent<PointerEnter>();
        }

        // FIXME: Add SerializableField Tag
        public Color Color = Color.gray;

        // FIXME: Add SerializableField Tag
        public string Text = null;

        // FIXME: Add SerializableField Tag
        public HorizontalAnchor Alignment = HorizontalAnchor.Center;

        // FIXME: Add SerializableField Tag
        public bool UseBoldFont = false;

        public override void Create(bool active = true)
        {
            Group.Padding = new List<int>() { 10, 20 };

            base.Create(active);

            LayoutGroup.childForceExpandWidth = true;

            var background = Get<Image>();
            background.type = image.type;
            background.sprite = image.sprite;
            background.color = Color;

            var text = new BaseText()
            {
                Name = $"{Name}:Text",
                Text = Text,
                Alignment = Alignment,
                UseBoldFont = UseBoldFont,
                UseOutline = true,
            };
            text.SetParent(this);

            // FIXME: debug
            var pe = Get<PointerEnter>();
            pe.changeSize = pointerEnter.changeSize;
            pe.restSize = pointerEnter.restSize;
            pe.xMirror = pointerEnter.xMirror;
            pe.yMirror = pointerEnter.yMirror;
            pe.move = pointerEnter.move;
            pe.moveX = pointerEnter.moveX;
            pe.moveSize = pointerEnter.moveSize;
            pe.restMoveSize = pointerEnter.restMoveSize;
            pe.SEKey = pointerEnter.SEKey;
            pe.changeTarget = pointerEnter.changeTarget;
        }
    }
}
