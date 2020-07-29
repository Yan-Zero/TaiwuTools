using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityUIKit.Core.GameObjects;
using UnityUIKit.GameObjects;
using YamlDotNet.Core.Tokens;

namespace TaiwuUIKit.GameObjects
{
    /// <summary>
    /// 太吾的 Actor 相貌显示组件
    /// </summary>
    [Serializable]
    public class TaiwuActorFace : BoxElementGameObject
    {
        private int[] _faceData = new int[8];
        private int[] _faceColor = new int[8];
        private int _genderChange = 0;
        private int _gender = 1;
        private int _clothesIndex = 0;
        private int _age = 16;
        private bool _life = true;
        private bool _fixHigh = false;
        private static readonly string[] imageName =
        {
            "body",
            "ageImage",
            "nose",
            "faceOther",
            "eye",
            "eyePupil",
            "eyebrows",
            "mouth",
            "beard",
            "hair1",
            "clothes",
            "clothesColor",
            "hair2",
            "hairOther",
        };

        /// <summary>
        /// 相貌数据
        /// </summary>
        public int[] FaceData
        {
            get => _faceData;
            set
            {
                _faceData = value;
                Apply();
            }
        }
        /// <summary>
        /// 相貌的颜色
        /// </summary>
        public int[] FaceColor
        {
            get => _faceColor;
            set
            {
                _faceColor = value;
                Apply();
            }
        }
        /// <summary>
        /// 生相
        /// 0 = 本
        /// 1 = 男生女相/女生男相
        /// </summary>
        public int GenderChange
        {
            set
            {
                _genderChange = value;
                Apply();
            }
            get => _genderChange;
        }
        /// <summary>
        /// 性别, 1 或 2
        /// 不能为 0
        /// </summary>
        public int Gender
        {
            get => _gender;
            set
            {
                if (value < 1 || value > 2)
                    throw new SystemException("性别只能 1 或 2");
                _gender = value;
                Apply();
            }
        }
        /// <summary>
        /// 衣服
        /// </summary>
        public int ClothesIndex
        {
            get => _clothesIndex;
            set
            {
                _clothesIndex = value;
                Apply();
            }
        }
        /// <summary>
        /// 显示坟头还是相貌
        /// </summary>
        public bool Life
        {
            get => _life;
            set
            {
                _life = value;
                Apply();
            }
        }
        /// <summary>
        /// 修正大小，False 是宽根据高变化，True 是高根据宽
        /// </summary>
        public bool FixSize
        {
            get => _fixHigh;
            set
            {
                _fixHigh = value;
                Apply();
            }
        }
        /// <summary>
        /// 外貌是男是女， 1 男 2 女
        /// </summary>
        public int AppGender
        {
            get
            {
                if (GenderChange == 0)
                    return Gender;
                else if (Gender == 1)
                    return 2;
                else 
                    return 1;
            }
        }
        /// <summary>
        /// 年龄
        /// </summary>
        public int Age
        {
            get => _age;
            set
            {
                _age = value;
                Apply();
            }
        }

        /// <summary>
        /// 创建对象
        /// </summary>
        public override void Create(bool active)
        {
            if (Element.PreferredSize.Count == 0)
                Element.PreferredSize = new List<float>()
                {
                    0 , 0
                };

            else if (Element.PreferredSize.Count == 1)
                Element.PreferredSize.Add(Element.PreferredSize[0]);
            if (_fixHigh)
                Element.PreferredSize[1] = (float)(Element.PreferredSize[0] / (216 / 264.00));
            else
                Element.PreferredSize[0] = (float)(Element.PreferredSize[1] * (216 / 264.00));

            base.Create(active);
            var af = Get<ActorFace>();
            foreach(var part in imageName)
            {
                var body = new BoxElementGameObject()
                {
                    Name = part
                };
                body.RectTransform.sizeDelta = new Vector2(-8, -4);
                body.RectTransform.anchorMax = Vector2.one;
                body.RectTransform.anchorMin = Vector2.zero;
                body.RectTransform.pivot = new Vector2(0.5f, 0.5f);
                af.GetType().GetField(part).SetValue(af, body.Get<UnityEngine.UI.Image>());
                body.SetParent(this);
            }
            Get<UnityEngine.UI.AspectRatioFitter>().aspectRatio = 216f / 264f;
            Apply();
        }

        /// <summary>
        /// 应用修正
        /// </summary>
        public void Apply()
        {
            if (!Created)
                return;
            if (_fixHigh)
                Get<UnityEngine.UI.AspectRatioFitter>().aspectMode = UnityEngine.UI.AspectRatioFitter.AspectMode.WidthControlsHeight;
            else
                Get<UnityEngine.UI.AspectRatioFitter>().aspectMode = UnityEngine.UI.AspectRatioFitter.AspectMode.HeightControlsWidth;
            
            Get<ActorFace>().UpdateFace(-1, Age, Gender, GenderChange, FaceData, FaceColor, ClothesIndex, Life);
        }
    }
}
