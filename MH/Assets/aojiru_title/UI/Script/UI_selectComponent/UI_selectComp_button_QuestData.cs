using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UI_buttonData_quest : UI_buttonDataBase
{
    [SerializeField,TextArea(1,3)] string questName;
    public string QuestName { get { return questName; } }
    [SerializeField,TextArea(1,6)] string questExplanation;
    public string QuestExplanation { get { return questExplanation; } }

    [SerializeField] string sceneName;
    public string SceneName { get { return sceneName; } }
}

public class UI_selectComp_button_QuestData : UI_selectComp_text_image
{
    [SerializeField] UI_buttonData_quest questData;

    protected override void Start()
    {
        base.Start();
        myText.text = questData.QuestName;
    }
    

    public override T GetData<T>()
    {
        return questData as T;
    }
}
