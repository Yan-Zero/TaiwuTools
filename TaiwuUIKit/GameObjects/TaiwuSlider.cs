using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityUIKit.Core;
using YamlDotNet.Serialization;

namespace TaiwuUIKit.GameObjects
{
    [YamlOnlySerializeSerializable]
    public class TaiwuSlider : UnityUIKit.GameObjects.Slider
    {
        private static readonly Image _handleImage = null;
        private static readonly Image _fillAreaImage = null;
        private static readonly Image _backgroundImage = null;
        private static readonly PointerEnter _pointerEnter = null;
        private static readonly ColorBlock _colorBlock;

        public override Image Res_SilderHandleImage => _handleImage;
        public override Image Res_FillAreaImage => _fillAreaImage;
        public override Image Res_BackgroundImage => _backgroundImage;

        static TaiwuSlider()
        {
            var silder = Resources.Load<GameObject>("prefabs/ui/views/ui_systemsetting").transform.Find("SystemSetting/SetBackup/BackupIntervalSlider");
            _handleImage = silder.GetComponent<CSlider>().image;
            _backgroundImage = silder.Find("Background").GetComponent<CImage>() as Image;
            _fillAreaImage = silder.Find("SEVolumeBack/Image").GetComponent<CImage>() as Image;
            _pointerEnter = silder.Find("Handle Slide Area/BackupHandle").GetComponent<PointerEnter>() as PointerEnter;

            _colorBlock = new ColorBlock()
            {
                normalColor = new Color32(251, 251, 251, 255),
                highlightedColor = new Color32(245, 245, 245, 255),
                pressedColor = new Color32(142, 142, 142, 255),
                disabledColor = new Color32(75, 75, 75, 255),
                colorMultiplier = 1,
                fadeDuration = 0.1f
            };
        }

        private List<string> TipParm = new List<string>() { "","" };

        [YamlSerializable]
        public string TipTitle
        {
            get => TipParm[0];
            set
            {
                TipParm[0] = value;
                if (Created) Handle.Get<MouseTipDisplayer>().param = TipParm.ToArray();
            }
        }
        [YamlSerializable]
        public string TipContant
        {
            get => TipParm[1];
            set
            {
                TipParm[1] = value;
                if (Created) Handle.Get<MouseTipDisplayer>().param = TipParm.ToArray();
            }
        }

        public override void Create(bool active)
        {
            if (Element.PreferredSize.Count == 0)
                Element.PreferredSize = new List<float>() { 0, 48 };

            base.Create(active);

            if (!string.IsNullOrEmpty(TipTitle) || !string.IsNullOrEmpty(TipContant))
                Get<MouseTipDisplayer>().param = TipParm.ToArray();

            var slider = Get<UnityEngine.UI.Slider>();
            var fillRectLayoutGroup = slider.fillRect.GetComponent<LayoutGroup>();
            var i = slider.fillRect.GetChild(0);
            i.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 3);
            fillRectLayoutGroup.padding.left = 10;
            fillRectLayoutGroup.padding.right = 10;
            i.GetComponent<Image>().raycastTarget = false;
            BackgroundContainer.Get<Image>().raycastTarget = false;
            slider.fillRect = null;

            slider.colors = _colorBlock;

            var sliderHandle = slider.handleRect;
            sliderHandle.sizeDelta = new Vector2(24, 24);
            sliderHandle.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(20, 0);
            sliderHandle.parent.GetComponent<RectTransform>().offsetMax = new Vector2(-20, 0);


            Get<PointerDrag>();

            var pe = slider.handleRect.gameObject.AddComponent<PointerEnter>();
            pe.changeSize = _pointerEnter.changeSize;
            pe.restSize = _pointerEnter.restSize;
            pe.xMirror = _pointerEnter.xMirror;
            pe.yMirror = _pointerEnter.yMirror;
            pe.move = _pointerEnter.move;
            pe.moveX = _pointerEnter.moveX;
            pe.moveSize = _pointerEnter.moveSize;
            pe.restMoveSize = _pointerEnter.restMoveSize;
            pe.SEKey = _pointerEnter.SEKey;
            pe.changeTarget = _pointerEnter.changeTarget;

            
        }
	}
}
