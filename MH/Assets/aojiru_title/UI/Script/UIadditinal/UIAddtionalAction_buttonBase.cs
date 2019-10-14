using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAddtionalAction_buttonBase : UIAdditionalActon_base
{
    protected UICanvas_button myCanvas_Index { get; private set; }

    protected override void InitAction()
    {
        base.InitAction();
        myCanvas_Index = myCanvas.GetComponent<UICanvas_button>();
    }
}
