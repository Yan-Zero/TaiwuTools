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
using System.Collections.Generic;
using UnityUIKit.Core;
using UnityEngine;
using UnityEngine.UI;
using YamlDotNet.Serialization;

namespace UnityUIKit.Components
{
    public class BoxGroup : ManagedComponent
    {
        //Why to serialize this?
        private Direction Direction;

        public HorizontalOrVerticalLayoutGroup LayoutGroup {
            get
            {
                var layoutGroup = (HorizontalOrVerticalLayoutGroup)GameObject.GetComponent<HorizontalLayoutGroup>()
                    ?? GameObject.GetComponent<VerticalLayoutGroup>();

                switch (Direction)
                {
                    case Direction.Horizontal:
                        if (layoutGroup is VerticalLayoutGroup) DestroyImmediate(layoutGroup);
                        layoutGroup = Get<HorizontalLayoutGroup>();
                        break;
                    case Direction.Vertical:
                        if (layoutGroup is HorizontalLayoutGroup) DestroyImmediate(layoutGroup);
                        layoutGroup = Get<VerticalLayoutGroup>();
                        break;
                    default:
                        throw new ArgumentException($"BoxModel with {Direction} is not supported");
                }

                return layoutGroup;
            }
        }

        public override void Apply(ManagedComponent.ComponentAttributes componentAttributes)
        {
            var attributes = componentAttributes as ComponentAttributes;
            if (!attributes) return;

            Direction = attributes.Direction;
            LayoutGroup.padding = attributes.RectOffset;
            LayoutGroup.childAlignment = attributes.ChildrenAlignment;
            LayoutGroup.spacing = attributes.Spacing;
            LayoutGroup.childControlHeight = attributes.ControlChildHeight;
            LayoutGroup.childControlWidth = attributes.ControlChildWidth;
            LayoutGroup.childForceExpandHeight = attributes.ForceExpandChildHeight;
            LayoutGroup.childForceExpandWidth = attributes.ForceExpandChildWidth;
        }


        public new class ComponentAttributes : ManagedComponent.ComponentAttributes
        {
            public Direction Direction = Direction.Vertical;
            public TextAnchor ChildrenAlignment = TextAnchor.MiddleCenter;
            public List<int> Padding = new List<int>();

            [YamlIgnore]
            public RectOffset RectOffset
            {
                get
                {
                    var (top, right, bottom, left) = (0, 0, 0, 0);

                    if (Padding.Count > 0)
                    {
                        top = Padding[0];
                        right = top;
                        bottom = top;
                        left = top;
                    }

                    if (Padding.Count > 1)
                    {
                        right = Padding[1];
                        left = right;
                    }

                    if (Padding.Count > 2)
                    {
                        bottom = Padding[2];
                    }

                    if (Padding.Count > 3)
                    {
                        left = Padding[3];
                    }

                    return new RectOffset(left, right, top, bottom);
                }
            }

            public float Spacing = 0;
            public bool ControlChildHeight = true;
            public bool ControlChildWidth = true;
            public bool ForceExpandChildHeight = false;
            public bool ForceExpandChildWidth = false;
        }
    }
}
