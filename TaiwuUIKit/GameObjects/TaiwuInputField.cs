using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace TaiwuUIKit.GameObjects
{
    public class TaiwuInputField : UnityUIKit.GameObjects.InputField
    {
        private static readonly Image _backgroundImage;
        public override Image Res_BackgroundImage => _backgroundImage;

        static TaiwuInputField()
        {
            var silder = Resources.Load<GameObject>("prefabs/ui/views/ui_systemsetting").transform.Find("SystemSetting/SetBackup/BackupIntervalSlider");
            _backgroundImage = silder.Find("Background").GetComponent<CImage>() as Image;
        }

        public bool UseBoldFont = false;

        public override void Create(bool active)
        {
            base.Font = UseBoldFont ? DateFile.instance.boldFont : DateFile.instance.font;

            base.Create(active);

            _Text.RectTransform.anchoredPosition = new Vector2(20, 0);
            _Text.RectTransform.offsetMax = new Vector2(-20, 0);
            _Text._Text.Color = UnityUIKit.Core.UIKitHelper.HexToColor("C8B099FF");
            _Text.Apply();

            m_Placeholder.RectTransform.anchoredPosition = new Vector2(20, 0);
            m_Placeholder.RectTransform.offsetMax = new Vector2(-20, 0);

            UnityInputField.caretColor = UnityUIKit.Core.UIKitHelper.HexToColor("C8B099FF");
            UnityInputField.selectionColor = UnityUIKit.Core.UIKitHelper.HexToColor("786E69C8");
        }
    }
}
