using System;
using UnityUIKit.Components;
using UnityUIKit;
using UnityEngine.UI;
using YamlDotNet.Serialization;
using UnityUIKit.Core.GameObjects;
using UnityUIKit.Core;
using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;

namespace UnityUIKit.GameObjects
{
    [YamlOnlySerializeSerializable]
    public class Slider : UnityUIKit.Core.GameObjects.BoxElementGameObject
    {
        public virtual Image Res_SilderHandleImage => null;
        public virtual Image Res_BackgroundImage => null;
        public virtual Image Res_FillAreaImage => null;


        protected ManagedGameObject Handle;
        public UnityEngine.UI.Slider UnitySlider;
        public BoxModelGameObject BackgroundContainer;


        protected float m_MaxValue = 100;
        protected float m_MinValue = 0;
        protected float m_NormalizedValue = 50;
        protected float m_Value = 50;
        protected bool m_wholeNumber = false;
        protected bool m_interactable = true;

        [YamlSerializable]
        public bool Interactable
        {
            get => m_interactable;
            set
            {
                m_interactable = value;
                if (UnitySlider) UnitySlider.interactable = m_interactable;
            }
        }
        [YamlSerializable]
        public float MaxValue
        {
            get => m_MaxValue;
            set
            {
                m_MaxValue = value;
                if (UnitySlider) UnitySlider.maxValue = m_MaxValue;
            }
        }
        [YamlSerializable]
        public float MinValue
        {
            get => m_MinValue;
            set
            {
                m_MinValue = value;
                if(UnitySlider) UnitySlider.minValue = m_MinValue;
            }
        }
        [YamlSerializable]
        public float NormalizedValue
        {
            get => m_NormalizedValue;
            set
            {
                m_NormalizedValue = value;
                if(UnitySlider) UnitySlider.normalizedValue = m_NormalizedValue;
            }
        }
        [YamlSerializable]
        public bool WholeNumber
        {
            get => m_wholeNumber;
            set
            {
                m_wholeNumber = value;
                if (UnitySlider) UnitySlider.wholeNumbers = m_wholeNumber;
            }
        }
        public float Value
        {
            get => m_Value;
            set 
            {
                m_Value = value;
                if (UnitySlider) 
                    UnitySlider.value = m_Value;
            }
        }
        [YamlSerializable]
        public UnityEngine.UI.Slider.Direction Direction = UnityEngine.UI.Slider.Direction.LeftToRight;
        public Action<float,Slider> OnValueChanged;

        public override void Create(bool active)
        {
            base.Create(active);

            if (Res_BackgroundImage != null)
            {
                (BackgroundContainer = new BoxModelGameObject()
                {
                    Name = "Background"
                }).SetParent(this);

                var bg = BackgroundContainer.Get<Image>();
                bg.type = Res_BackgroundImage.type;
                bg.sprite = Res_BackgroundImage.sprite;
                bg.color = Res_BackgroundImage.color;

                BackgroundContainer.RectTransform.sizeDelta = Vector2.zero;
                BackgroundContainer.RectTransform.anchorMin = Vector2.zero;
                BackgroundContainer.RectTransform.anchorMax = Vector2.one;

                BackgroundContainer.Get<UnityEngine.UI.LayoutElement>().ignoreLayout = true;
            }

            var Slider = Get<UnityEngine.UI.Slider>();
            Slider.direction = Direction;
            Slider.maxValue = MaxValue;
            Slider.minValue = MinValue;
            Slider.normalizedValue = m_NormalizedValue;
            Slider.value = m_Value;
            Slider.wholeNumbers = m_wholeNumber;
            Slider.interactable = m_interactable;
            Slider.onValueChanged.AddListener(onValueChanged);

            BoxElementGameObject fillArea;
            new BoxModelGameObject()
            {
                Name = "Fill Area",
                Group =
                {
                    Padding = { 5 },
                    ControlChildHeight = false,
                },
                Children =
                {
                    (fillArea = new BoxElementGameObject()
                    {
                        Name = "Image"
                    })
                }
            }.SetParent(this);
            var sliderFillArea_image = fillArea.Get<Image>();
            if (Res_FillAreaImage != null)
            {
                sliderFillArea_image.type = Res_FillAreaImage.type;
                sliderFillArea_image.sprite = Res_FillAreaImage.sprite;
                sliderFillArea_image.color = Res_FillAreaImage.color;
            }
            Slider.fillRect = sliderFillArea_image.rectTransform.parent.gameObject.GetComponent<RectTransform>();
            Slider.fillRect.sizeDelta = Vector2.zero;


            ManagedGameObject i;
            (i = new BoxElementGameObject()
            {
                Name = "Handle Slide Area",
                Children =
                {
                    (Handle = new BoxModelGameObject()
                    {
                        Name = "Handle"
                    })
                }
            }).SetParent(this);

            i.RectTransform.anchoredPosition = Vector2.zero;
            i.RectTransform.pivot = Vector2.zero;
            i.RectTransform.anchorMax = new Vector2(1, 0.5f);
            i.RectTransform.anchorMin = new Vector2(0, 0.5f);
            var tempVec = this.RectTransform.sizeDelta;
            tempVec.y = 0;
            i.RectTransform.sizeDelta = tempVec;


            Handle.RectTransform.anchoredPosition = Vector2.zero;
            var silderHandle_image = Handle.Get<Image>();
            if (Res_SilderHandleImage != null)
            {
                silderHandle_image.type = Res_SilderHandleImage.type;
                silderHandle_image.sprite = Res_SilderHandleImage.sprite;
                silderHandle_image.color = Res_SilderHandleImage.color;

                Slider.image = silderHandle_image;
            }
            Slider.handleRect = silderHandle_image.rectTransform;
            UnitySlider = Slider;
        }

        private void onValueChanged(float value)
        {
            OnValueChanged?.Invoke(value, this);
        }
    }
}
