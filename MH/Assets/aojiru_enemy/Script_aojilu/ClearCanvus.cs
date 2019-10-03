using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using kubota;
using UnityEngine.SceneManagement;

namespace aojilu
{
    public class ClearCanvus : MonoBehaviour
    {
        public enum ClearState
        {
            NONE,CLEAR,GAMEOVER,ToTitle
        }
        [SerializeField]ClearState clearState = ClearState.NONE;

        [SerializeField] float clearWaitTime;
        [SerializeField] float overWaitTime;
        [SerializeField] float cleaeToTitleTime;
        float waitStartTime;

        [SerializeField] CharBase player;
        [SerializeField] CharBase[] targetList;
        [SerializeField] Timer timer;

        Animator animator;

        AudioSource au;

        private void Start()
        {
            animator = GetComponent<Animator>();

            au = GetComponent<AudioSource>();
        }

        private void Update()
        {
            switch (clearState)
            {
                case ClearState.NONE:
                    if (player != null && player.currentHP <= 0)
                    {
                        waitStartTime = Time.fixedTime;
                        clearState = ClearState.GAMEOVER;
                    }
                    if (targetList != null && IsDeadAllTarget())
                    {
                        waitStartTime = Time.fixedTime;
                        clearState = ClearState.CLEAR;
                        timer.EndTimer();
                    }
                    break;
                case ClearState.CLEAR:
                    if (Time.fixedTime > waitStartTime + clearWaitTime)
                    {
                        animator.SetTrigger("clear");
                        clearState = ClearState.ToTitle;
                        waitStartTime = Time.fixedTime;
                    }
                    break;
                case ClearState.GAMEOVER:
                    if (Time.fixedTime > waitStartTime + overWaitTime)
                    {
                        animator.SetTrigger("failed");
                        clearState = ClearState.ToTitle;
                        waitStartTime = Time.fixedTime;
                    }
                    break;
                case ClearState.ToTitle:
                    if (Time.fixedTime > waitStartTime + cleaeToTitleTime)
                    {
                        SceneManager.LoadScene("Title");
                    }
                    break;
            }
        }

        bool IsDeadAllTarget()
        {
            bool flag = true;
            foreach(var obj in targetList)
            {
                if (obj.currentHP > 0) flag = false;
            }
            return flag;
        }

        void StampSound()
        {
            au.Play();
        }
    }
}
