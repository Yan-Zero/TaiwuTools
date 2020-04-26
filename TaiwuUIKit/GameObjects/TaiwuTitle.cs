using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityUIKit.Core;
using UnityUIKit.GameObjects;

namespace TaiwuUIKit.GameObjects
{
    [Serializable]
    public class TaiwuTitle : Container
    {
        public Direction Direction = Direction.Vertical;

        private string text = "Default Content";
        private Label Label;

        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
                if (Label != null)
                {
                    Label._Text.Content = text;
                    Label.Apply();
                }
            }
        }

        public override void Create(bool active)
        {
            var nameBack = Resources.Load<GameObject>("prefabs/ui/views/ui_systemsetting").transform.Find("SystemSetting/NameBack");
            var backgroundImage = nameBack.GetComponent<CImage>() as Image;
            BackgroundImage = backgroundImage;

            // Default padding
            Group.Direction = Direction;
            Group.Padding = new List<int>() { 0 };
            Group.ChildrenAlignment = TextAnchor.MiddleCenter;

            Element.PreferredSize = new List<float> { 0, 40 };

            base.Create(active);

            // Default Label
            Label = new Label()
            {
                Name = "Text",
                _Text =
                {
                    FontSize = 21,
                    Font = DateFile.instance.boldFont,
                    Color = new Color(185f / 255, 125f / 255, 75f / 255, 1),
                    Content = text
                }
            };

            Label.SetParent(RectTransform);
            Label.Get<Outline>();
        }
    }
}
