using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UICanvas_YesNo_ChengeQuestScene : UICanvas_YesNo
{
    [SerializeField] int beforeIndex;
    [SerializeField] string loadSceneName;

    protected override void YesAction()
    {
        if (loadSceneName != "")
        {
            SceneManager.LoadScene(loadSceneName);
        }
    }
    

    protected override void NoAction()
    {
        uictrl.CloseUICanvas(myUIIndex);
        uictrl.OpenUICanvas(beforeIndex,sortOrder);
    }

    public void SetLoadSceneName(string s)
    {
        loadSceneName = s;
    }
}
