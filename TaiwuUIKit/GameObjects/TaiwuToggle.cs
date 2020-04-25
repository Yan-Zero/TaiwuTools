using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace TaiwuUIKit.GameObjects
{
    //public class TaiwuToggle : TaiwuTogleButton
    //{
    //    // Load static background image
    //    private static readonly PointerEnter _pointerEnter;
    //    private static readonly PointerClick _pointerClick;
    //    private static readonly ColorBlock _colors;

    //    private Image _image;

    //    public override Image Res_Image => _image;
    //    public override PointerEnter Res_PointerEnter => _pointerEnter;
    //    public override PointerClick Res_PointerClick => _pointerClick;
    //    public virtual ColorBlock Res_Colors => _colors;

    //    static TaiwuToggle()
    //    {
    //        var SetScreenToggle = Resources.Load<GameObject>("prefabs/ui/views/ui_systemsetting").transform.Find("SystemSetting/SetScreen/FullScreenToggle,702");
    //        _colors = SetScreenToggle.GetComponent<CToggle>().colors;
    //        _pointerEnter = SetScreenToggle.GetComponent<PointerEnter>();
    //        _pointerClick = SetScreenToggle.GetComponent<PointerClick>();
    //    }

    //    public Action<bool, TaiwuToggle> onValueChanged = delegate { };

    //    public bool isOn
    //    {
    //        get
    //        {
    //            return Get<Toggle>().isOn;
    //        }
    //        set
    //        {
    //            Get<Toggle>().isOn = value;
    //        }
    //    }

    //    public List<float> PreferredSize = new List<float> { 0, 50 };

    //    private ColorBlock Colors = ColorBlock.defaultColorBlock;

    //    public override void Create(bool active = true)
    //    {
    //        Element.PreferredSize = PreferredSize;
    //        if (Colors == ColorBlock.defaultColorBlock)
    //            Colors = Res_Colors;
    //        var SetScreenToggle = Resources.Load<GameObject>("prefabs/ui/views/ui_systemsetting").transform.Find("SystemSetting/SetScreen/FullScreenToggle,702");
    //        _image = SetScreenToggle.GetComponent<CToggle>().image;


    //        base.Create(active);

    //        var Toggle = Get<Toggle>();
    //        Toggle.image = Get<Image>();
    //        Toggle.transition = Selectable.Transition.ColorTint;
    //        Toggle.graphic = Toggle.image;
    //        Toggle.onValueChanged.AddListener(onValueChanged_Invoke);

    //        if (Res_PointerClick != null)
    //        {
    //            var pc = Get<PointerClick>();
    //            pc.playSE = Res_PointerClick.playSE;
    //            pc.SEKey = Res_PointerClick.SEKey;
    //        }
    //    }

    //    private void onValueChanged_Invoke(bool isOn)
    //    {
    //        onValueChanged.Invoke(isOn, this);
    //    }
    //}
}
