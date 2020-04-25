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
using UnityUIKit.Components;
using UnityUIKit.Core;
using UnityUIKit.Core.GameObjects;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace UnityUIKit.GameObjects
{
    public partial class Container
    {
        [Serializable]
        public class ScrollContainer : Container
        {
            private Content content;

            // Override group field as layout group for Content
            public new BoxGroup.ComponentAttributes Group = new BoxGroup.ComponentAttributes();
            public Dictionary<string, ManagedGameObject> ContentChildren = new Dictionary<string, ManagedGameObject>();


            public ScrollRect ScrollRect => Get<ScrollRect>();


            public override void Create(bool active = true)
            {
                base.Group.Padding = Group.Padding;
                base.Create(active);

                ScrollRect.horizontal = Group.Direction != Direction.Vertical;
                ScrollRect.vertical = Group.Direction != Direction.Horizontal;
                LayoutGroup.childForceExpandWidth = true;
                LayoutGroup.childForceExpandHeight = true;


                // FIXME - orphan on destroy
                var viewport = new Viewport() {
                    Name = $"{Name}:Viewport",
                    Group =
                    {
                        ForceExpandChildWidth = true,
                        ForceExpandChildHeight = true,
                    }
                };


                // FIXME - orphan on destroy
                content = new Content() {
                    Name = $"{Name}:Content",
                    Group = {
                        Direction = Group.Direction,
                        Spacing = Group.Spacing,
                        Padding = { 0, 20 }, // FIXME: hack
                        ControlChildHeight = true,
                        ControlChildWidth = true,
                        ForceExpandChildWidth = Group.Direction == Direction.Vertical,
                        ForceExpandChildHeight = Group.Direction == Direction.Horizontal,
                    },
                };

                foreach (var contentChild in ContentChildren) contentChild.Value.SetParent(content);
                content.SetParent(viewport);
                viewport.SetParent(this);

                ScrollRect.viewport = viewport.RectTransform;
                ScrollRect.content = content.RectTransform;
            }


            public bool Contains(string key) => ContentChildren.ContainsKey(key);
            public void Add(string key, ManagedGameObject gameObject) => (ContentChildren[key] = gameObject).SetParent(content);


            protected class Viewport : Container
            {
                public Mask Mask => Get<Mask>();

                public override void Create(bool active = true)
                {
                    base.Create(active);

                    // color doesn't matter here, as long as there is a valid image
                    Get<Image>().color = Color.black;
                    Mask.showMaskGraphic = false;
                }
            }


            protected class Content : BoxGroupGameObject
            {
                public ContentSizeFitter ContentSizeFitter => Get<ContentSizeFitter>();

                public override void Create(bool active = true)
                {
                    base.Create(active);

                    RectTransform.pivot = new Vector2(0, 1);
                    LayoutGroup.childAlignment = TextAnchor.UpperLeft;

                    ContentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
                }
            }
        }
    }
}
