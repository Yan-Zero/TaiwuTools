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
using UnityUIKit.GameObjects;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace TaiwuUIKit.GameObjects
{
    public class BaseFrame : Container
    {
        public Direction Direction = Direction.Horizontal;

        public float Spacing = 20;

        public List<int> Padding = new List<int>() { 20 };

        public override void Create(bool active)
        {
            // FIXME: use a resource loader
            var dialog = Resources.Load<GameObject>("prefabs/ui/views/ui_dialog").transform.Find("Dialog");
            var backgroundImage = dialog.GetComponent<CImage>() as Image;
            BackgroundImage = backgroundImage;

            // Default padding
            Group.Direction = Direction;
            Group.Padding = Padding;
            Group.Spacing = Spacing;

            

            base.Create(active);
        }
    }
}
