using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LoadCtrl : MonoBehaviour
{
    public enum FadeState
    {
        CLEAR,DOWN,BLACK,UP
    }
    [SerializeField]FadeState fadeState;

    [SerializeField] float fadeSpeed;
    [SerializeField]Image panel;

    Dictionary<string,UnityAction>  blackActionList=new Dictionary<string, UnityAction>();

    //黒画面で止まる時間
    [SerializeField] float waitTime;

    public bool IsFadeNow { get { return fadeState != FadeState.CLEAR; } }

    private void Update()
    {
        switch (fadeState)
        {
            case FadeState.CLEAR:
                break;
            case FadeState.DOWN:
                {
                    var color = panel.color;
                    color.a += fadeSpeed * Time.fixedDeltaTime;
                    if (color.a >= 1)
                    {
                        color.a = 1;
                        fadeState = FadeState.BLACK;
                    }
                    panel.color = color;
                }
                break;
            case FadeState.BLACK:
                BlackAction();
                WaitTimeAction(waitTime, () =>fadeState = FadeState.UP);
                break;
            case FadeState.UP:
                {
                    var color = panel.color;
                    color.a -= fadeSpeed * Time.fixedDeltaTime;
                    if (color.a <= 0)
                    {
                        color.a = 0;
                        fadeState = FadeState.CLEAR;
                    }
                    panel.color = color;
                }
                break;
        }
    }

    void ChengeFadeState(FadeState fs)
    {
        fadeState = fs;
    }

    void BlackAction()
    {
        foreach (var act in blackActionList)
        {
            act.Value.Invoke();
        }
        blackActionList = new Dictionary<string, UnityAction>();
    }

    public void AddBlackAction(string key,UnityAction action)
    {
        UnityAction ua = null;
        blackActionList.TryGetValue(key, out ua);
        if (ua != null) return;
        blackActionList.Add(key, action);
    }
    
    [ContextMenu("fadeTest")]
    public void FadeAction()
    {
        if (fadeState == FadeState.CLEAR)
        {
            fadeState = FadeState.DOWN;
        }
    }

    /// <summary>
    /// 黒画面で止まる時間の指定
    /// </summary>
    /// <param name="f"></param>
    public void SetWaitBlackTime(float f)
    {
        waitTime = f;
    }

    void WaitTimeAction(float f, UnityAction ua)
    {
        StartCoroutine(WaitTimeCoroutine(f, ua));
    }

    IEnumerator WaitTimeCoroutine(float f,UnityAction ua)
    {
        yield return new WaitForSeconds(f);
        ua.Invoke();
    }
}
