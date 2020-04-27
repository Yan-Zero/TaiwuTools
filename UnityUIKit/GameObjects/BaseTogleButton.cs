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
        [YamlIgnore]
        public virtual Image Res_Image => null;
        private Label m_Label = new Label();

        [YamlIgnore]
        public virtual Label Label => m_Label ;
        public BaseTogleButton()
        {
            Label._Text.Color = Color.gray;
        }


        public Color ImageColor = Color.clear;
        public Color FontColor
        {
            get => Label._Text.Color;
            set => Label._Text.Color = value;
        }


        public string Text
        {
            get => Label.Text;
            set => Label.Text = value;
        }


        public override void Create(bool active)
        {
            base.Create(active);

            LayoutGroup.childForceExpandWidth = true;

            var background = Get<Image>();
            background.type = Res_Image.type;
            background.sprite = Res_Image.sprite;
            background.color = ImageColor;

            Label.Name = $"{Name}:Text";
            Label.SetParent(this);
        }
    }
}
