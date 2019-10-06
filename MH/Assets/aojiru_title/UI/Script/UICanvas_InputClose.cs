using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICanvas_InputClose : UICanvas_useAnimation
{
    [SerializeField] bool submit;
    [SerializeField] bool cancel;
    [SerializeField] int nextUIIndex;

    protected override void SubmitAction()
    {
        if (submit)
        {
            uictrl.OpenUICanvas(nextUIIndex);
            uictrl.CloseUICanvas(myUIIndex);
        }
    }

    protected override void CancelAction()
    {
        if (cancel)
        {
            uictrl.OpenUICanvas(nextUIIndex);
            uictrl.CloseUICanvas(myUIIndex);
        }
    }
}
