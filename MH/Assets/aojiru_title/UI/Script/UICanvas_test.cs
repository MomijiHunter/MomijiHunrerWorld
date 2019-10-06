using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICanvas_test : UICanvas
{
    [SerializeField] int myIndex;
    [SerializeField] int targetIndex;

    protected override void SubmitAction()
    {
        base.SubmitAction();
        uictrl.SleepUICanvas(myIndex);
        uictrl.OpenUICanvas(targetIndex,sortOrder+1);
    }

    protected override void CancelAction()
    {
        base.CancelAction();
        uictrl.CloseUICanvas(myIndex);
        uictrl.OpenUICanvas(targetIndex, sortOrder - 1);
    }
}
