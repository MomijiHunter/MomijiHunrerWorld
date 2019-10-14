using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIAddtionalAction_questData : UIAddtionalAction_buttonBase
{
    [SerializeField] UI_buttonData_quest myData;
    public void GetQuestData_fromButton()
    {
        myData = myCanvas_Index.nowSelectComponent.GetData<UI_buttonData_quest>();
    }

    public void GoQuest()
    {
        SceneManager.LoadScene(myData.SceneName);
    }
}
