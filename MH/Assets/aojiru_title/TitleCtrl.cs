using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace aojilu
{
    public class TitleCtrl : MonoBehaviour
    {
        [SerializeField] AudioClip decision;
        AudioSource audioSource;
 
        public enum TITLESTATE
        {
            FIRST,INPUTST,SELECT
        }
        [SerializeField]TITLESTATE titleState = TITLESTATE.FIRST;

        [SerializeField] float firstWait;

        float setTime;
        Animator animator;

        private void Awake()
        {
            setTime = Time.fixedTime;
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            switch (titleState)
            {
                case TITLESTATE.FIRST:
                    if (Time.fixedTime > setTime + firstWait)
                    {
                        titleState = TITLESTATE.INPUTST;
                        setTime = Time.fixedTime;
                        animator.SetTrigger("first");
                    }
                    break;
                case TITLESTATE.INPUTST:
                    if (Input.anyKeyDown)
                    {
                        animator.SetBool("second",true);
                        titleState = TITLESTATE.SELECT;
                        audioSource.PlayOneShot(decision);
                    }
                    break;
                case TITLESTATE.SELECT:
                    if (Input.GetKeyDown(KeyCode.X))
                    {
                        animator.SetBool("second", true);
                        SetUnderBar(false);
                    }
                    break;
            }
        }

        void SetUnderBar(bool flag)
        {
            animator.SetBool("underBarOn", flag);
        }

        public void ChengeFromSelect(string trigger)
        {
            animator.SetBool("second", false);
            animator.SetTrigger(trigger);
            SetUnderBar(true);
        }

        public void BackToSelect()
        {
            animator.SetBool("second", true);
            SetUnderBar(false);
        }

        public void ChengeScene(string key)
        {
            SceneManager.LoadScene(key);
        }
    }
}
