using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class QuestDataData
{
    [SerializeField] string questName;//クエスト名
    public string QuestName { get { return questName; } }

    [SerializeField] string explanation;//クエストの説明
    public string Explanation { get { return explanation; } }
}

public class QuestData : MonoBehaviour
{
    [SerializeField] QuestDataData qData;
    public QuestDataData Qdata { get { return qData; } }

    public static void SetQuestData(QuestDataData data,Text qN,Text qD)
    {
        qN.text = data.QuestName;
        qD.text = data.Explanation;
    }
}
