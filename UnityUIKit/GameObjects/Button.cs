using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;

namespace UnityUIKit.GameObjects
{
	public class Button : BaseTogleButton
	{
		public Action<UnityUIKit.GameObjects.Button> OnClick;

		private void OnClick_Invoke()
		{
			OnClick?.Invoke(this);
		}

		public override void Create(bool active)
		{
			if (Group.Padding.Count == 0)
				Group.Padding = new List<int> { 10, 20 };

			base.Create(active);

			UnityEngine.UI.Button button = Get<UnityEngine.UI.Button>();
			button.onClick.AddListener(OnClick_Invoke);
			button.image = Get<Image>();
		}
	}
}
