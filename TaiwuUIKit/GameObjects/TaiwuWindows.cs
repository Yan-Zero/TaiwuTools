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
    public class TaiwuWindows : BaseFrame
    {
        private TaiwuTitle title = null;
        
        private string titleText = "";
        
        public CloseButton CloseButton = null;

        public string Title 
        {
            get
            {
                return titleText;
            }
            set
            {
                titleText = value;
                
                if (title != null && title.Created)
                {
                    title.Text = titleText;

                    if (string.IsNullOrEmpty(value))
                        title.SetActive(false);
                    else
                        title.SetActive(true);
                }
            }
        }

        public override void Create(bool active = true)
        {
            title = new TaiwuTitle
            {
                Name = "Title",
                Text = titleText
            };
            Children.Insert(0, title);
            CloseButton = new CloseButton()
            {
                Name = "Close",
            };
            Children.Insert(1, CloseButton);

            base.Create(active);

            CloseButton.onClick.AddListener(delegate {
                if (null == RectTransform.parent)
                    GameObject.SetActive(false);
                else
                    RectTransform.parent.gameObject.SetActive(false);
            });

            CloseButton.RectTransform.anchoredPosition = Vector2.zero;
        }
    }

    public class TaiwuTitle : Container
    {
        public Direction Direction = Direction.Vertical;

        private string text = "Default Content";
        private Label Label = null;

        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;

                if(Label != null && Label.Created)
                {
                    Label._Text.Content = text;
                    Label.Apply();
                }
            }
        }

        public override void Create(bool active = true)
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

            Label.Create();
            Label.SetParent(RectTransform);
            Label.Get<Outline>();
        }
    }
}
