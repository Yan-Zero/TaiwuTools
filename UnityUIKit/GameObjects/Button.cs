using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityUIKit.Core;

namespace UnityUIKit.GameObjects
{
	public class Button : BaseTogleButton
	{
		public Action<UnityUIKit.GameObjects.Button> OnClick;

		private void OnClick_Invoke()
		{
			OnClick?.Invoke(this);
		}

		public UnityEngine.UI.Button UnityButton;

		[YamlSerializable]
		public override bool Interactable
		{
			get => m_interactable;
			set
			{
				m_interactable = value;
				if (UnityButton) UnityButton.interactable = m_interactable;
			}
		}

		public override void Create(bool active)
		{
			if (Group.Padding.Count == 0)
				Group.Padding = new List<int> { 10, 20 };

			base.Create(active);

			UnityButton = Get<UnityEngine.UI.Button>();
			UnityButton.onClick.AddListener(OnClick_Invoke);
			UnityButton.image = Get<Image>();
			UnityButton.interactable = m_interactable;
		}
	}
}
