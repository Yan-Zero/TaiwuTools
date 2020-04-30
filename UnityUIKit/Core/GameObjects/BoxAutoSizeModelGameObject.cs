using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityUIKit.Components;

namespace UnityUIKit.Core.GameObjects
{
    [YamlOnlySerializeSerializable]
    public class BoxAutoSizeModelGameObject : BoxSizeFitterGameObject
    {
        [YamlSerializable]
        public BoxGroup.ComponentAttributes Group = new BoxGroup.ComponentAttributes();
        public BoxGroup BoxGroup => Get<BoxGroup>();

        public HorizontalOrVerticalLayoutGroup LayoutGroup => BoxGroup.LayoutGroup;

        public override void Create(bool active)
        {
            base.Create(active);

            BoxGroup.Apply(Group);
        }
    }
}
