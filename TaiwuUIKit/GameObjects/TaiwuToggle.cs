using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityUIKit.Core;
using UnityUIKit.Core.GameObjects;

namespace TaiwuUIKit.GameObjects
{
    [YamlOnlySerializeSerializable]
    public class TaiwuToggle : TaiwuToggleButton
    {
        // Load static background image
        private static readonly PointerEnter _pointerEnter;
        private static readonly PointerClick _pointerClick;
        private static readonly ColorBlock _colors;

        private static readonly Image _BackgroundImage;

        public override Image Res_Image => _BackgroundImage;
        public override PointerEnter Res_PointerEnter => _pointerEnter;
        public override PointerClick Res_PointerClick => _pointerClick;
        public virtual ColorBlock Res_Colors => _colors;

        static TaiwuToggle()
        {
            var SetScreenToggle = Resources.Load<GameObject>("prefabs/ui/views/ui_systemsetting").transform.Find("SystemSetting/SetScreen/FullScreenToggle,702");
            _pointerEnter = SetScreenToggle.GetComponent<PointerEnter>();
            _pointerClick = SetScreenToggle.GetComponent<PointerClick>();
            _BackgroundImage = SetScreenToggle.GetComponent<CToggle>().image;
            _colors = new ColorBlock()
            {
                normalColor = new Color32(251,251,251,255),
                highlightedColor = new Color32(245, 245, 245,255),
                pressedColor = new Color32(142, 142, 142, 255),
                disabledColor = new Color32(75, 75, 75, 255),
                colorMultiplier = 1,
                fadeDuration = 0.1f
            };
        }

        public Action<bool, TaiwuToggle> onValueChanged = delegate { };

        [YamlSerializable]
        public bool isOn
        {
            get
            {
                return m_isOn;
            }
            set
            {
                m_isOn = value;
                if(Created)
                    Get<Toggle>().isOn = m_isOn;
            }
        }
        public bool m_isOn = false;

        [YamlSerializable]
        public List<float> PreferredSize = new List<float> { 50, 50 };

        private List<string> TipParm = new List<string>() { "", "" };
        [YamlSerializable]
        public string TipTitle
        {
            get => TipParm[0];
            set
            {
                TipParm[0] = value;
                if (Created) Get<MouseTipDisplayer>().param = TipParm.ToArray();
            }
        }
        [YamlSerializable]
        public string TipContant
        {
            get => TipParm[1];
            set
            {
                TipParm[1] = value;
                if (Created) Get<MouseTipDisplayer>().param = TipParm.ToArray();
            }
        }

        public override void Create(bool active = true)
        {
            if(Element.PreferredSize.Count == 0)
                Element.PreferredSize = PreferredSize;
            Label._Text.Color = Color.white;

            base.Create(active);

            var Toggle = Get<Toggle>();
            Toggle.transition = Selectable.Transition.ColorTint;
            Toggle.colors = Res_Colors;
            Toggle.isOn = m_isOn;

            if (Res_Image != null)
            {
                BoxModelGameObject BackgroundContainer;
                (BackgroundContainer = new BoxModelGameObject()
                {
                    Name = "Label"
                }).SetParent(this);

                var bgOn = BackgroundContainer.Get<Image>();
                bgOn.type = Res_Image.type;
                bgOn.sprite = Res_Image.sprite;
                bgOn.color = new Color(156f / 255, 54f / 255, 54f / 255, 1);

                BackgroundContainer.RectTransform.sizeDelta = Vector2.zero;
                BackgroundContainer.RectTransform.anchorMin = Vector2.zero;
                BackgroundContainer.RectTransform.anchorMax = Vector2.one;
                BackgroundContainer.RectTransform.SetAsFirstSibling();

                BackgroundContainer.Get<UnityEngine.UI.LayoutElement>().ignoreLayout = true;

                var bgOff = Get<Image>();
                bgOff.type = Res_Image.type;
                bgOff.sprite = Res_Image.sprite;
                bgOff.color = new Color(50f / 255, 50f / 255, 50f / 255, 1);

                Toggle.graphic = bgOn;
                Toggle.image = bgOff;
                Toggle.targetGraphic = bgOff;
            }
            
            Toggle.onValueChanged.AddListener(onValueChanged_Invoke);

            if (Res_PointerClick != null)
            {
                var pc = Get<PointerClick>();
                pc.playSE = Res_PointerClick.playSE;
                pc.SEKey = Res_PointerClick.SEKey;
            }
        }

        private void onValueChanged_Invoke(bool isOn)
        {
            onValueChanged.Invoke(isOn, this);
        }
    }
}
