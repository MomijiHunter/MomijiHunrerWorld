using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_selectComp_text_image : UI_selectComp_text
{
    [SerializeField] Image image;
    
    [SerializeField] Color color_select_image;
    Color color_default_image;

    protected override void Start()
    {
        base.Start();
        color_default_image = image.color;
    }

    public override void SelectOn()
    {
        base.SelectOn();
        image.color = color_select_image;
    }

    public override void SelectOff()
    {
        base.SelectOff();
        image.color = color_default_image;
    }

    public override T GetData<T>()
    {
        return base.GetData<T>();
    }
}
