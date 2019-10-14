using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAdditionalAction_open : UIAdditionalActon_base
{
    [SerializeField] int targetIndex;

    public void OpenTarget_selfClose(int i)
    {
        uiCtrl.OpenUICanvas(i, myCanvas.SortOrder);
        uiCtrl.CloseUICanvas(myCanvas.MyUIIndex);
    }
    public void OpenTarget_selfClose()
    {
        uiCtrl.OpenUICanvas(targetIndex, myCanvas.SortOrder);
        uiCtrl.CloseUICanvas(myCanvas.MyUIIndex);
    }
    public void OpenTarget_selfSleep(int i)
    {
        uiCtrl.OpenUICanvas(i, myCanvas.SortOrder + 1);
        uiCtrl.SleepUICanvas(myCanvas.MyUIIndex);
    }

    public void OpenTarget_selfSleep()
    {
        uiCtrl.OpenUICanvas(targetIndex, myCanvas.SortOrder + 1);
        uiCtrl.SleepUICanvas(myCanvas.MyUIIndex);
    }

    public void CloseAnimation()
    {
        myCanvas.AnimationEV_CloseCanvas();
    }
}
