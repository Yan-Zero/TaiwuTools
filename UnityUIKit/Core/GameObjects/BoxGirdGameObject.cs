using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityUIKit.Components;
using YamlDotNet.Serialization;

namespace UnityUIKit.Core.GameObjects
{
    public class BoxGridGameObject : BoxSizeFitterGameObject
    {
        public BoxGrid.ComponentAttributes Grid = new BoxGrid.ComponentAttributes();

        [YamlIgnore]
        public BoxGrid BoxGrid => Get<BoxGrid>();

        public override void Create(bool active)
        {
            base.Create(active);

            BoxGrid.Apply(Grid);
            //BoxElement.Apply(Element);
        }
    }
}
