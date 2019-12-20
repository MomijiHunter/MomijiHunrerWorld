using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] Button continueButton;
    [SerializeField] Image PauseMenuPanel;

    public void Continue()
    {
        Time.timeScale = 1;
        PauseMenuPanel.gameObject.SetActive(false);
    }

    public void Retire()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Title");
    }

    private void Update()
    {
        if (Input.GetButtonDown("Pause"))
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
                continueButton.Select();
            }
        }
    }
}
