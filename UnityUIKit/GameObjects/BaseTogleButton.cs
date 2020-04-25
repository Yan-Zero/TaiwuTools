using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityUIKit.Core;
using UnityUIKit.Core.GameObjects;
using YamlDotNet.Serialization;

namespace UnityUIKit.GameObjects
{
    
    public class BaseTogleButton : BoxModelGameObject
    {
        private string text = null;

        [YamlIgnore]
        public virtual Image Res_Image => null;

        [YamlIgnore]
        public Label Label = null;

        public Color ImageColor = Color.clear;
        public Color FontColor = Color.gray;
        public HorizontalAnchor Alignment = HorizontalAnchor.Center;


        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
                if (Label != null && Label.Created)
                {
                    Label.Text = text;
                    if (!string.IsNullOrEmpty(value))
                        Label.SetActive(true);
                }
            }
        }


        public override void Create(bool active = true)
        {
            base.Create(active);

            LayoutGroup.childForceExpandWidth = true;

            var background = Get<Image>();
            background.type = Res_Image.type;
            background.sprite = Res_Image.sprite;
            background.color = ImageColor;

            if(Label == null)
            {
                Label = new Label()
                {
                    Name = $"{Name}:Text",
                    Text = text,
                    _Text =
                    {
                        Color = FontColor
                    }
                };
            }

            if (string.IsNullOrEmpty(text))
                Label.Create(false);
            Label.SetParent(this);
        }
    }
}
