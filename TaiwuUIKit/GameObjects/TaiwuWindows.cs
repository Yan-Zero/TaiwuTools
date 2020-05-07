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
        public TaiwuTitle TaiwuTitle;
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
                
                if (TaiwuTitle != null)
                {
                    TaiwuTitle.Text = titleText;

                    if (string.IsNullOrEmpty(value))
                        TaiwuTitle.SetActive(false);
                    else
                        TaiwuTitle.SetActive(true);
                }
            }
        }

        public override void Create(bool active)
        {
            TaiwuTitle = new TaiwuTitle
            {
                Name = "Title",
                Text = titleText
            };
            CloseButton = new CloseButton()
            {
                Name = "Close",
            };

            base.Create(active);
            TaiwuTitle.SetParent(this);
            TaiwuTitle.RectTransform.SetAsFirstSibling();
            CloseButton.SetParent(this);

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
