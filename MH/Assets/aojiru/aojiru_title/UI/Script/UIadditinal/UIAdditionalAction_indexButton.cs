using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAdditionalAction_indexButton : UIAddtionalAction_buttonBase
{
    public void OpenButtonTargetUI_selfClose()
    {
        ButtonAdditonalAction();
        //var bt = myCanvas_Index.nowSelectComponent.GetComponent<UI_selectComp_button_uiData>();
        //var data = myCanvas_Index.nowSelectComponent.GetData<UI_buttonData_nextUIData>();
        //uiCtrl.OpenUICanvas(data.TargetUIIndex, myCanvas.SortOrder);
        uiCtrl.CloseUICanvas(myCanvas_Index.MyUIIndex);
    }

    public void OpenButtonTargetUI_selfSleep()
    {
        ButtonAdditonalAction();
        //var bt = myCanvas_Index.nowSelectComponent.GetComponent<UI_selectComp_button_uiData>();
        //var data = myCanvas_Index.nowSelectComponent.GetData<UI_buttonData_nextUIData>();
        //uiCtrl.OpenUICanvas(data.TargetUIIndex, myCanvas.SortOrder);
        uiCtrl.SleepUICanvas(myCanvas_Index.MyUIIndex);
    }
}
