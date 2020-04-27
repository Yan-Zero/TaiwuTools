using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityUIKit.Components;
using UnityUIKit.Core;
using UnityUIKit.GameObjects;
using YamlDotNet.Serialization;

namespace TaiwuUIKit.GameObjects
{
    public class TaiwuLabel : Container
    {
        private static readonly Image _background_subtitle;
        private static readonly Image _background_value;

        static TaiwuLabel()
        {
            var Label = Resources.Load<GameObject>("prefabs/ui/views/ui_systemsetting").transform.Find("SystemSetting/SetBackup/BackupBack");
            _background_subtitle = Label.GetComponent<CImage>() as Image;
            _background_value = Label.Find("SEVolumeBack").GetComponent<CImage>() as Image;
        }

        public enum Style
        {
            Subtitle,
            Value
        }
        public Style BackgroundStyle = Style.Subtitle;

        [YamlIgnore]
        public BaseText _Text = new BaseText();

        public HorizontalAnchor Alignment
        {
            get => _Text.Alignment;
            set => _Text.Alignment = value;
        }
        public bool UseBoldFont
        {
            get => _Text.UseBoldFont;
            set => _Text.UseBoldFont = value;
        }
        public bool UseOutline
        {
            get => _Text.UseOutline;
            set => _Text.UseOutline = value;
        }
        public string Text
        {
            get => _Text.Text;
            set => _Text.Text = value;
        }

        public override void Create(bool active)
        {
#if DEBUG
            ManagedGameObjectIO.debugLogger.Start($"TaiwuLabel.{Name}.Create");
            ManagedGameObjectIO.debugLogger.WriteLine($"_Text.UseBoldFont : {_Text.UseBoldFont}");
            ManagedGameObjectIO.debugLogger.End();
#endif

            base.Create(active);

            _Text.Name = $"Text.{Name}";
            _Text.SetParent(this);

            Image bg;
            switch(BackgroundStyle)
            {
                case Style.Subtitle:
                    {
                        bg = _background_subtitle;
                        _Text._Text.Color = new Color(0.314f, 0.275f, 0.255f, 1);
                        _Text._Text.FontSize = 22;
                        break;
                    }
                case Style.Value:
                    {
                        bg = _background_value;
                        _Text._Text.Color = new Color(0.882f, 0.804f, 0.667f, 1);
                        break;
                    }
                default:
                    throw new ArgumentException($"BoxModel with {BackgroundStyle} is not supported");
            }

            var imger = Get<Image>();
            imger.type = bg.type;
            imger.sprite = bg.sprite;
            imger.color = bg.color;
            _Text._Text.FontSize = 20;
            _Text.Apply();
        }
    }
}
