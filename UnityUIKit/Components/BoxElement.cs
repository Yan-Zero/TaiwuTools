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
using UnityEngine.UI;
using System;

namespace UnityUIKit.Components
{
    public class BoxElement : ManagedComponent
    {
        public LayoutElement LayoutElement => Get<LayoutElement>();


        public override void Apply(ManagedComponent.ComponentAttributes componentAttributes)
        {
            var attributes = componentAttributes as ComponentAttributes;
            if (!attributes) return;

            LayoutElement.minWidth = attributes.MinimalWidth;
            LayoutElement.minHeight = attributes.MinimalHeight;
            LayoutElement.preferredWidth = attributes.PreferredWidth;
            LayoutElement.preferredHeight = attributes.PreferredHeight;
            LayoutElement.flexibleWidth = attributes.FlexibleWidth;
            LayoutElement.flexibleHeight = attributes.FlexibleHeight;
        }


        [YamlOnlySerializeSerializable]
        public new class ComponentAttributes : ManagedComponent.ComponentAttributes
        {
            [YamlSerializable]
            public List<float> MinimalSize = new List<float>();
            public float MinimalWidth => MinimalSize.Count > 0 ? MinimalSize[0] : 0;
            public float MinimalHeight => MinimalSize.Count > 1 ? MinimalSize[1] : MinimalWidth;

            [YamlSerializable]
            public List<float> PreferredSize = new List<float>();
            public float PreferredWidth => PreferredSize.Count > 0 ? PreferredSize[0] : 0;
            public float PreferredHeight => PreferredSize.Count > 1 ? PreferredSize[1] : PreferredWidth;

            [YamlSerializable]
            public List<float> FlexibleSize = new List<float>();
            public float FlexibleWidth => FlexibleSize.Count > 0 ? FlexibleSize[0] : (PreferredWidth > 0 ? 0 : 1);
            public float FlexibleHeight => FlexibleSize.Count > 1 ? FlexibleSize[1] : (PreferredHeight > 0 ? 0 : 1);
        }
    }
}
