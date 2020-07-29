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
        /// <summary>
        /// 字体颜色
        /// </summary>
        public Color FontColor
        {
            get => Label._Text.Color;
            set => Label._Text.Color = value;
        }

        /// <summary>
        /// 内容
        /// </summary>
        public string Text
        {
            get => Label.Text;
            set => Label.Text = value;
        }
        /// <summary>
        /// 私有的
        /// </summary>
        protected bool m_interactable = true;
        /// <summary>
        /// 可交互的
        /// </summary>
        public virtual bool Interactable
        {
            get => m_interactable;
            set
            {
                m_interactable = value;
            }
        }

        /// <summary>
        /// 创建对像
        /// </summary>
        /// <param name="active"></param>
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
