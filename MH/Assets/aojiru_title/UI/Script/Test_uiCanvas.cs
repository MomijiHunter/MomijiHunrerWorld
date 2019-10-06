using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_uiCanvas : MonoBehaviour
{
    [SerializeField] UIListCtrl uiCtrl;
    public void OpenUI(int i)
    {
        uiCtrl.OpenUICanvas(i,uiCtrl.NowUICanvas.sortOrder+1);
    }

    public void CloseUI(int i)
    {
        uiCtrl.CloseUICanvas(i);
    }

    public void SleepUI(int i)
    {
        uiCtrl.SleepUICanvas(i);
    }
}
