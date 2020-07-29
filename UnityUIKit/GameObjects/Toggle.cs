using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityUIKit.Core;

namespace UnityUIKit.GameObjects
{
	/// <summary>
	/// 开关
	/// </summary>
	[YamlOnlySerializeSerializable]
	public class Toggle : BaseTogleButton
	{
		public virtual ColorBlock Res_Colors => ColorBlock.defaultColorBlock;

		public Action<bool, UnityUIKit.GameObjects.Toggle> onValueChanged = delegate {} ;

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
				if (Created)
					Get<UnityEngine.UI.Toggle>().isOn = m_isOn;
			}
		}
		private bool m_isOn = false;

		/// <summary>
		/// 预设大小
		/// </summary>
		[YamlSerializable]
		public List<float> PreferredSize = new List<float> { 0, 50 };

		public override void Create(bool active)
		{
			if (Element.PreferredSize.Count == 0)
				Element.PreferredSize = PreferredSize;

			base.Create(active);
			
			UnityEngine.UI.Toggle toggle = Get<UnityEngine.UI.Toggle>();
			toggle.isOn = m_isOn;
			toggle.image = Get<Image>();
			toggle.onValueChanged.AddListener(onValueChanged_Invoke);
		}

		private void onValueChanged_Invoke(bool isOn)
		{
			onValueChanged?.Invoke(isOn, this);
		}
	}

}
