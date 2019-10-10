using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetDataText_QuestData : GetDataText_base<QuestData>
{
    [SerializeField] Text nameText;
    [SerializeField] Text explaText;

    protected override QuestData GetMydata()
    {
        var d = targetButton.GetComponent<UI_selectComp_button_QuestData>();
        if (d == null) return null;
        return d.GetQuestData();
    }

    protected override void DisplayData()
    {
        nameText.text = myData.QuestName;
        explaText.text = myData.QuestExplanation;
    }

    protected override void DisplayData_nullData()
    {
        nameText.text = "";
        explaText.text = "";
    }
}
