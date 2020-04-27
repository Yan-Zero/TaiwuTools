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
using System;

namespace TaiwuUIKit.GameObjects
{
    [YamlOnlySerializeSerializable]
    public class TaiwuToggleButton : BaseTogleButton
    {
        public virtual PointerEnter Res_PointerEnter => null;
        public virtual PointerClick Res_PointerClick => null;

        [YamlSerializable]
        public bool UseBoldFont
        {
            get => (Label as BaseText).UseBoldFont;
            set => (Label as BaseText).UseBoldFont = value;
        }
        [YamlSerializable]
        public bool UseOutline
        {
            get => (Label as BaseText).UseOutline;
            set => (Label as BaseText).UseOutline = value;
        }

        private BaseText m_Label = new BaseText();
        public override Label Label => m_Label;

        public override void Create(bool active)
        {
            base.ImageColor = Res_Image.color;

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

}
