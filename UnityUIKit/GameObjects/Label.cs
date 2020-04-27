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

using System;
using UnityUIKit.Components;
using UnityUIKit.Core;
using YamlDotNet.Serialization;

namespace UnityUIKit.GameObjects
{

    public class Label : ManagedGameObject
    {
        public TextControl.ComponentAttributes _Text = new TextControl.ComponentAttributes();
        [YamlIgnore]
        public TextControl TextControl => Get<TextControl>();

        public string Text
        {
            get
            {
                return _Text.Content;
            }
            set
            {
                _Text.Content = value;
                Apply();
            }
        }

        public override void Create(bool active)
        {
            base.Create(active);

            TextControl.Apply(_Text);
        }

        public virtual void Apply()
        {
            if(Created)
                TextControl.Apply(_Text);
        }
    }
}
