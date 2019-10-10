using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenTargetUI_checkChengeScene : UICanvas_openTargetUI
{
    [SerializeField] string sceneName;

    protected override void SubmitAction()
    {
        base.SubmitAction();
        var t= uictrl.GetUICanvas(TargetIndex).GetComponent<UICanvas_YesNo_ChengeQuestScene>();
        t.SetLoadSceneName(sceneName);
    }

    protected override void ChengeIndexAction()
    {
        base.ChengeIndexAction();
        var d = nowSelectComponent.GetComponent<UI_selectComp_button_QuestData>();
        if (d != null) sceneName = d.GetQuestData().SceneName;
        else sceneName = null;
    }
}
