using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityUIKit.Core;
using UnityUIKit.GameObjects;
using YamlDotNet.Serialization;

namespace TaiwuUIKit.GameObjects
{
    [YamlOnlySerializeSerializable]
    public class TaiwuWindows : BaseFrame
    {
        private TaiwuTitle title;
        private string titleText = "";

        public CloseButton CloseButton;

        [YamlSerializable]
        public string Title 
        {
            get
            {
                return titleText;
            }
            set
            {
                titleText = value;
                
                if (title != null)
                {
                    title.Text = titleText;

                    if (string.IsNullOrEmpty(value))
                        title.SetActive(false);
                    else
                        title.SetActive(true);
                }
            }
        }

        public override void Create(bool active)
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
            Children.Add(CloseButton);

            base.Create(active);

            CloseButton.OnClick = delegate 
            {
                if (null == RectTransform.parent)
                    GameObject.SetActive(false);
                else
                    RectTransform.parent.gameObject.SetActive(false);
            };

            CloseButton.RectTransform.anchoredPosition = Vector2.zero;
        }
    }

}
