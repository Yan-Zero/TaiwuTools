using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityUIKit.GameObjects
{
	public class ToggleGroup : BoxModelGameObject
	{
		public override void Create(bool active)
		{
			base.Create(active);
			foreach (ManagedGameObject child in Children)
			{
				if (child is UnityUIKit.GameObjects.Toggle)
					child.Get<UnityEngine.UI.Toggle>().group = Get<UnityEngine.UI.ToggleGroup>();
			}
		}
	}

}
