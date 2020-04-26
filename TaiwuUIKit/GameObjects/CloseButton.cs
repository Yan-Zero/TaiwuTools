using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TaiwuUIKit.GameObjects
{
    public class CloseButton : TaiwuButton
    {
        // Load static background image
        private static readonly Image _image;
        private static readonly PointerEnter _pointerEnter;
        private static readonly PointerClick _pointerClick;

        public override Image Res_Image => _image;
        public override PointerEnter Res_PointerEnter => _pointerEnter;
        public override PointerClick Res_PointerClick => _pointerClick;

        static CloseButton()
        {
            var exitButton = Resources.Load<GameObject>("prefabs/ui/views/ui_systemsetting").transform.Find("SystemSetting/Close");
            _image = exitButton.GetComponent<CButton>().image;
            _pointerEnter = exitButton.GetComponent<PointerEnter>();
            _pointerClick = exitButton.GetComponent<PointerClick>();
        }

        public void Click()
        {
            if (Res_PointerClick != null)
                AudioManager.instance.PlaySE(Res_PointerClick.SEKey);
            OnClick.Invoke(this);
        }

        public override void Create(bool active)
        {
            base.Text = null;
            base.Create(active);

            Get<LayoutElement>().ignoreLayout = true;

            RectTransform.sizeDelta = new Vector2(40, 40);
            RectTransform.anchorMax = new Vector2(1, 1);
            RectTransform.anchorMin = RectTransform.anchorMax;
            RectTransform.anchoredPosition = Vector2.zero;
        }
    }
}
