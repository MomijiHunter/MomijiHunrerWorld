using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace aojilu
{
    public class QuestDicisionButton : MonoBehaviour
    {
        [SerializeField] string sceneName;
        [SerializeField] TitleCtrl tcl;

        Button myButton;

        private void Start()
        {
            myButton = GetComponent<Button>();
        }

        private void Update()
        {
            myButton.interactable = (sceneName != "");
        }

        public void SetSceneName(string m)
        {
            sceneName = m;
        }

        public void ChengeScene()
        {
            tcl.ChengeScene(sceneName);
        }
    }
}
