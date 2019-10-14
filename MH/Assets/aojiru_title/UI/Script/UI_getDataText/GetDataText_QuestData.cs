using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetDataText_QuestData : GetDataText_base<UI_buttonData_quest>
{
    [SerializeField] Text nameText;
    [SerializeField] Text explaText;

    protected override UI_buttonData_quest GetMydata()
    {
        var d = targetButton.GetData<UI_buttonData_quest>();
        return d;
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
