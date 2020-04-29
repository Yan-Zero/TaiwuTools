using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityUIKit.Core;

namespace UnityUIKit.Components
{
    public class BoxSizeFitter : ManagedComponent
    {
        public ContentSizeFitter ContentSizeFitter => Get<ContentSizeFitter>();


        public override void Apply(ManagedComponent.ComponentAttributes componentAttributes)
        {
            var attributes = componentAttributes as ComponentAttributes;
            if (!attributes) return;

            ContentSizeFitter.horizontalFit = attributes.HorizontalFit;
            ContentSizeFitter.verticalFit = attributes.VerticalFit;
        }


        public new class ComponentAttributes : ManagedComponent.ComponentAttributes
        {
            public ContentSizeFitter.FitMode HorizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            public ContentSizeFitter.FitMode VerticalFit = ContentSizeFitter.FitMode.Unconstrained;
        }
    }
}