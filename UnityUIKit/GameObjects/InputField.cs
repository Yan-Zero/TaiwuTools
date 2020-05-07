using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.InputField;

namespace UnityUIKit.GameObjects
{
    public class InputField : Core.GameObjects.BoxElementGameObject
    {
        public virtual Image Res_BackgroundImage => null;


        protected Label m_Placeholder = new Label
        {
            Name = "Placeholder",
            _Text =
            {
                Color = new Color32(75,75,75,255),
            }
        };
        public string Placeholder
        {
            get => m_Placeholder.Text;
            set => m_Placeholder.Text = value;
        }


        public Label _Text = new Label
        {
            Name = "Text",
            _Text =
            {
                FontSize = 20,
            }
        };
        public Font Font;
        public int FontSize
        {
            get => _Text._Text.FontSize;
            set => _Text._Text.FontSize = value;
        }
        public Color FontColor
        {
            get => _Text._Text.Color;
            set 
            {
                _Text._Text.Color = value;
                _Text.Apply();
            }
        }
        public TextAnchor TextAlignment
        {
            get => _Text._Text.Alignment;
            set
            {
                _Text._Text.Alignment = value;
                _Text.Apply();
                m_Placeholder._Text.Alignment = value;
                m_Placeholder.Apply();
            }
        }


        public UnityEngine.UI.InputField UnityInputField;
        private ContentType m_ContentType;
        public ContentType ContentType
        {
            get => m_ContentType;
            set
            {
                m_ContentType = value;
                if (UnityInputField)
                    UnityInputField.contentType = m_ContentType;
            }
        }
        private InputType m_InputType;
        public InputType InputType
        {
            get => m_InputType;
            set
            {
                m_InputType = value;
                if (UnityInputField)
                    UnityInputField.inputType = m_InputType;
            }
        }
        private TouchScreenKeyboardType m_KeyboardType;
        public TouchScreenKeyboardType KeyboardType
        {
            get => m_KeyboardType;
            set
            {
                m_KeyboardType = value;
                if (UnityInputField)
                    UnityInputField.keyboardType = m_KeyboardType;
            }
        }
        private LineType m_LineType;
        public LineType LineType 
        {
            get => m_LineType;
            set
            {
                m_LineType = value;
                if (UnityInputField)
                    UnityInputField.lineType = m_LineType;
            }
        }
        private string m_Text;
        public string Text
        {
            get => UnityInputField?.text ?? m_Text;
            set
            {
                m_Text = value;
                if (UnityInputField)
                    UnityInputField.text = m_Text;
            }
        }
        private int m_CharacterLimit;
        public int CharacterLimit
        {
            get => m_CharacterLimit;
            set
            {
                m_CharacterLimit = value;
                if (UnityInputField)
                    UnityInputField.characterLimit = m_CharacterLimit;
            }
        }
        private bool m_ReadOnly;
        public bool ReadOnly
        {
            get => m_ReadOnly;
            set
            {
                m_ReadOnly = value;
                if (UnityInputField)
                    UnityInputField.readOnly = m_ReadOnly;
            }
        }


        public Action<string,InputField> OnEndEdit;
        public Action<string,InputField> OnValueChanged;


        public override void Create(bool active)
        {

            m_Placeholder._Text.Font = Font;
            _Text._Text.Font = Font;
            
            base.Create(active);
            m_Placeholder.SetParent(this);
            _Text.SetParent(this);

            if (Res_BackgroundImage)
            {
                var bg = Get<Image>();
                bg.type = Res_BackgroundImage.type;
                bg.sprite = Res_BackgroundImage.sprite;
                bg.color = Res_BackgroundImage.color;
            }

            UnityInputField = Get<UnityEngine.UI.InputField>();
            UnityInputField.placeholder = m_Placeholder.Get<Text>();
            UnityInputField.textComponent = _Text.Get<Text>();
            UnityInputField.contentType = m_ContentType;
            UnityInputField.inputType = m_InputType;
            UnityInputField.keyboardType = m_KeyboardType;
            UnityInputField.lineType = m_LineType;
            UnityInputField.text = m_Text;
            UnityInputField.onEndEdit.AddListener(onEndEdit);
            UnityInputField.onValueChanged.AddListener(onValueChanged);
            UnityInputField.readOnly = m_ReadOnly;
            UnityInputField.characterLimit = CharacterLimit;

            m_Placeholder.RectTransform.sizeDelta = Vector2.zero;
            m_Placeholder.RectTransform.anchorMax = Vector2.one;
            m_Placeholder.RectTransform.anchorMin = Vector2.zero;

            _Text.RectTransform.sizeDelta = Vector2.zero;
            _Text.RectTransform.anchorMax = Vector2.one;
            _Text.RectTransform.anchorMin = Vector2.zero;
        }


        private void onEndEdit(string value)
        {
            OnEndEdit?.Invoke(value, this);
        }
        private void onValueChanged(string value)
        {
            OnValueChanged?.Invoke(value, this);
        } 

    }
}
