using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_selectComp_text : UI_selectComponentBase
{
    [SerializeField]  Text text;
    protected Text myText { get { return text; } }

    [SerializeField] Color color_select;
    [SerializeField] Color color_cantSelect;
    [SerializeField] Color color_sleep;
    Color color_default;

    protected override void Start()
    {
        base.Start();
        color_default = text.color;
    }

    public override void SelectOn()
    {
        base.SelectOn();
        if (!Sleep && Selectable)
        {
            text.color = color_select;
        }
    }

    public override void SelectOff()
    {
        base.SelectOff();
        if (!Sleep && Selectable)
        {
            text.color = color_default;
        }
    }

    protected override void SelectableUpdate()
    {
        base.SelectableUpdate();
        if (!Selectable) text.color = color_cantSelect;
    }

    protected override void SleepUpdate()
    {
        base.SleepUpdate();
        if (Sleep) text.color = color_sleep;
    }

    protected override void SleepEndAction()
    {
        base.SleepEndAction();
        text.color = color_default;
        if (selected) SelectOn();
    }
}
