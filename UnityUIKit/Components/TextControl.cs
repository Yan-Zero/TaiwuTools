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

using UnityUIKit.Core;
using UnityEngine;
using UnityEngine.UI;

namespace UnityUIKit.Components
{
    public class TextControl : ManagedComponent
    {
        public Text Text => Get<Text>();

        public override void Apply(ManagedComponent.ComponentAttributes componentAttributes)
        {
            var attributes = componentAttributes as ComponentAttributes;
            if (!attributes) return;

            Text.font = attributes.Font;
            Text.fontSize = attributes.FontSize;
            Text.color = attributes.Color;
            Text.alignment = attributes.Alignment;
            Text.text = attributes.Content;
            Text.horizontalOverflow = HorizontalWrapMode.Overflow;
        }

        public new class ComponentAttributes : ManagedComponent.ComponentAttributes
        {
            // FIXME: Add SerializableField Tag
            public Font Font = null;

            // FIXME: Add SerializableField Tag
            public int FontSize = 18;

            // FIXME: Add SerializableField Tag
            public Color Color = Color.white;

            // FIXME: Add SerializableField Tag
            public TextAnchor Alignment = TextAnchor.MiddleCenter;

            // FIXME: Add SerializableField Tag
            public string Content = null;
        }
    }
}
