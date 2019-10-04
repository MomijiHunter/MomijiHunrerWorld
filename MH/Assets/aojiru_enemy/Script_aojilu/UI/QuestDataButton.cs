using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace aojilu
{
    public class QuestDataButton : MonoBehaviour
    {
        [SerializeField] QuestData myData;
        [SerializeField] Text questText;
        [SerializeField] Text explanation;
        [SerializeField] QuestDicisionButton dicitionButton;

        Button myButton;

        private void Start()
        {
            myButton = GetComponent<Button>();
        }

        public void SetData(string s)
        {
            questText.text = myData.Qdata.QuestName;
            explanation.text = myData.Qdata.Explanation;
            dicitionButton.SetSceneName(s);
        }
    }
}
