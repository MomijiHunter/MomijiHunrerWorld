using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICanvas_openOtherUI : UICanvas
{
    [SerializeField] bool isSleepCanvas;
    [SerializeField] int beforeUIIndex=-1;

    protected override void SubmitAction()
    {
        var d = nowSelectComponent.GetComponent<UI_selectComp_button_uiData>();



        uictrl.OpenUICanvas(d.TargetUIIndex, (isSleepCanvas) ? sortOrder : sortOrder + 1);
        if (isSleepCanvas)
        {
            uictrl.SleepUICanvas(myUIIndex);
        }
        else
        {
            uictrl.CloseUICanvas(myUIIndex);
        }
    }

    protected override void CancelAction()
    {
        if (beforeUIIndex >= 0)
        {
            uictrl.OpenUICanvas(beforeUIIndex, sortOrder - 1);
            uictrl.CloseUICanvas(myUIIndex);
        }
    }
}
