using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using YamlDotNet.Serialization.EventEmitters;

namespace UnityUIKit.Components
{
    public class AutoGridLayoutGroup : GridLayoutGroup
    {
        [SerializeField]
        protected bool m_AutoWidth = false;

        public bool autoWidth
        {
            get
            {
                return m_AutoWidth;
            }
            set
            {
                SetProperty(ref m_AutoWidth, value);
            }
        }


        public override void CalculateLayoutInputHorizontal()
        {
            if((m_AutoWidth || cellSize.x == 0) && constraint == Constraint.FixedColumnCount)
            {
                float width = rectTransform.rect.width - padding.left - padding.right;
                width = width - (constraintCount - 1) * spacing.x;
                width = width / constraintCount;
                var tempVec = cellSize;
                tempVec.x = width;
                cellSize = tempVec;
            }

            base.CalculateLayoutInputHorizontal();
        }
    }
}
