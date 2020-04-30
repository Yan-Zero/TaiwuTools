using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityUIKit.Core;
using YamlDotNet.Serialization;
using static UnityEngine.UI.GridLayoutGroup;

namespace UnityUIKit.Components
{
    /// <summary>
    /// 当子节点数据特别多的时候使用这种方法会很消耗资源，这种情况下需要考虑循环列表的使用。当结构功能复杂的布局也不能使用这种布局。
    /// </summary>
    public class BoxGrid : ManagedComponent
    {
        public AutoGridLayoutGroup GridLayoutGroup => Get<AutoGridLayoutGroup>();

        public override void Apply(ManagedComponent.ComponentAttributes componentAttributes)
        {
            var attributes = componentAttributes as ComponentAttributes;
            if (!attributes) return;

            GridLayoutGroup.padding = attributes.RectOffset;
            GridLayoutGroup.childAlignment = attributes.ChildrenAlignment;
            GridLayoutGroup.spacing = attributes.Spacing;
            GridLayoutGroup.startCorner = attributes.StartCorner;
            GridLayoutGroup.cellSize = attributes.CellSize;
            GridLayoutGroup.startAxis = new Dictionary<Direction, GridLayoutGroup.Axis>(){
                {Direction.Horizontal,Axis.Horizontal},
                {Direction.Vertical,Axis.Vertical}
            }[attributes.StartAxis];
            GridLayoutGroup.constraint = attributes.Constraint;
            GridLayoutGroup.constraintCount = attributes.ConstraintCount;
            GridLayoutGroup.autoWidth = attributes.AutoWidth;
        }

        public new class ComponentAttributes : ManagedComponent.ComponentAttributes
        {
            /// <summary>
            /// 第一个元素所在的角
            /// </summary>
            public GridLayoutGroup.Corner StartCorner = Corner.UpperLeft;

            /// <summary>
            /// 如果布局元素未填满所有可用空间，则用于这些元素的对齐方式。
            /// </summary>
            public TextAnchor ChildrenAlignment = TextAnchor.MiddleCenter;

            /// <summary>
            /// 布局组边缘内的填充
            /// </summary>
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

            /// <summary>
            /// 布局元素之间的间距
            /// </summary>
            public Vector2 Spacing = new Vector2(2 , 2);

            /// <summary>
            /// 组中每个布局元素要使用的大小
            /// </summary>
            public Vector2 CellSize = new Vector2(50, 50);

            /// <summary>
            /// 沿着哪个主轴放置元素。
            /// 在开始新行之前，Horizontal 将填满整个行。
            /// 在开始新列之前，Vertical 将填充整个列。
            /// </summary>
            public Direction StartAxis = Direction.Horizontal;

            /// <summary>
            /// 将网格限制为固定数量的行或列，以辅助自动布局系统。
            /// 
            /// Flexible - 不约束
            /// Fixed Column Count - 固定列数
            /// Fixed Row Count - 	固定行数
            /// </summary>
            public GridLayoutGroup.Constraint Constraint = 0;

            /// <summary>
            /// 约束数量
            /// </summary>
            public int ConstraintCount = 2;

            public bool AutoWidth = false;
        }
    }
}
