using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAdditionalAction_indexButton : UIAddtionalAction_buttonBase
{


    public void OpenButtonTargetUI_selfClose()
    {
        var bt = myCanvas_Index.nowSelectComponent.GetComponent<UI_selectComp_button_uiData>();
        uiCtrl.OpenUICanvas(bt.TargetUIIndex, myCanvas.SortOrder);
        uiCtrl.CloseUICanvas(myCanvas_Index.MyUIIndex);
    }

    public void OpenButtonTargetUI_selfSleep()
    {
        var bt = myCanvas_Index.nowSelectComponent.GetComponent<UI_selectComp_button_uiData>();
        uiCtrl.OpenUICanvas(bt.TargetUIIndex, myCanvas.SortOrder);
        uiCtrl.SleepUICanvas(myCanvas_Index.MyUIIndex);
    }
}
