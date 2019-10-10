using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICanvas_openTargetUI : UICanvas
{
    [SerializeField] int targetIndex;
    protected int TargetIndex { get {return targetIndex; } }
    [SerializeField] bool isSleepCanvas;
    [SerializeField] int beforeUIIndex = -1;

    protected override void SubmitAction()
    {
        uictrl.OpenUICanvas(targetIndex, (isSleepCanvas) ? sortOrder+1 : sortOrder);
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
            ResetIndex();
        }
    }
}
