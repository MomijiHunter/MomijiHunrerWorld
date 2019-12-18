using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] Image PauseMenuPanel;
    [SerializeField] Selectable ContinueButton;

    public void Continue()
    {
        Time.timeScale = 1;
        PauseMenuPanel.gameObject.SetActive(false);
    }

    public void Retire()
    {
        SceneManager.LoadScene("Title");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
                PauseMenuPanel.gameObject.SetActive(false);
            }
            else
            {
                Time.timeScale = 0;
                PauseMenuPanel.gameObject.SetActive(true);
                ContinueButton.Select();
            }
        }
    }
}
