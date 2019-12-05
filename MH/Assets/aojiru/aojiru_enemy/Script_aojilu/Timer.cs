using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] bool awakeStart;

    [SerializeField] Text timerText;
    bool timerNow = false;
    float time;

    private void Awake()
    {
        if (awakeStart)
        {
            StartTimer();
        }
    }

    private void Update()
    {
        if (timerNow)
        {
            time += Time.deltaTime;
        }
        timerText.text = time.ToString()+"秒";
    }

    public void StartTimer()
    {
        time = 0;
        timerNow = true;
    }

    public void EndTimer()
    {
        timerNow = false;
    }
}
