using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace TaiwuUIKit.GameObjects
{
    public class LayoutIgnorer : MonoBehaviour, ILayoutIgnorer
    {
        public bool ignoreLayout => true;
    }

    [Serializable]
    public class TaiwuButton : TaiwuTogleButton
    {
        // Load static background image
        private static readonly Image _image;
        private static readonly PointerEnter _pointerEnter;
        private static readonly PointerClick _pointerClick;

        public override Image Res_Image => _image;
        public override PointerEnter Res_PointerEnter => _pointerEnter;
        public override PointerClick Res_PointerClick => _pointerClick;

        static TaiwuButton()
        {
            var startBtn = Resources.Load<GameObject>("oldsceneprefabs/mianmenuback").transform.Find("MainMenu/StartMenuButton");
            _image = startBtn.GetComponent<Image>();
            _pointerEnter = startBtn.GetComponent<PointerEnter>();
            _pointerClick = startBtn.GetComponent<PointerClick>();
        }

        public Action<TaiwuButton> OnClick;

        public override void Create(bool active = true)
        {
            Group.Padding = new List<int>() { 10, 20 };

            base.Create(active);

            var bt = Get<Button>();
            bt.onClick.AddListener(OnClick_Invoke);
            bt.image = Get<Image>();

            if (Res_PointerClick != null)
            {
                var pc = Get<PointerClick>();
                pc.playSE = Res_PointerClick.playSE;
                pc.SEKey = Res_PointerClick.SEKey;
            }
        }

        private void OnClick_Invoke()
        {
            OnClick?.Invoke(this);
        }
    }
}
