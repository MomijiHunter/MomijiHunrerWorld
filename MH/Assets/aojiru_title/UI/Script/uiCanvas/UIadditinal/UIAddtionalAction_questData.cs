using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIAddtionalAction_questData : UIAddtionalAction_buttonBase
{
    [SerializeField] QuestData myData;
    public void GetQuestData_fromButton()
    {
       var data= myCanvas_Index.nowSelectComponent.GetComponent<UI_selectComp_button_QuestData>();
       myData=data.GetQuestData();
    }

    public void GoQuest()
    {
        SceneManager.LoadScene(myData.SceneName);
    }
}
