using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace aojilu {
    public class MusicCtrl : MonoBehaviour
    {
        public enum FadeState
        {
            NONE, DOWN, UP
        }
        [SerializeField] FadeState fadeState;
        [SerializeField] AudioClip[] clips;
        [SerializeField] float upSpeed;
        [SerializeField] float downSpeed;
        AudioClip nowClip;
        float firstVolume;

        AudioSource audioSource;

        [SerializeField] EnemyBase[] targets;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            firstVolume = audioSource.volume;
        }

        private void Update()
        {
            Test_ChengeMusic();

            switch (fadeState)
            {
                case FadeState.NONE:
                    if (audioSource.clip != nowClip) fadeState = FadeState.DOWN;
                    break;
                case FadeState.DOWN:
                    audioSource.volume -= downSpeed * Time.fixedDeltaTime;
                    if (audioSource.clip == nowClip) fadeState = FadeState.UP;

                    if (audioSource.volume <= 0||downSpeed<0)
                    {
                        audioSource.volume = 0;
                        fadeState = FadeState.UP;
                        ChengeClip(nowClip);
                    }
                    break;
                case FadeState.UP:
                    audioSource.volume += upSpeed * Time.fixedDeltaTime;
                    
                    if (audioSource.volume >= firstVolume||upSpeed<0)
                    {
                        audioSource.volume = firstVolume;
                        fadeState = FadeState.NONE;
                    }
                    break;
            }
        }

        void ChengeClip(AudioClip ac)
        {
            audioSource.clip = null;
            audioSource.Stop();
            audioSource.clip = ac;
            audioSource.Play();
        }

        public void ChengeMusic(int i)
        {
            nowClip = clips[i];
        }


        void Test_ChengeMusic()
        {
            if (Test_checkDetectTarget())
            {
                ChengeMusic(1);
            }else 
            {
                ChengeMusic(0);
            }
        }
        
        /// <summary>
        /// どれか一匹でも対象がdetectかどうか
        /// </summary>
        /// <returns></returns>
        bool Test_checkDetectTarget()
        {
            bool result = false;
            foreach(var target in targets)
            {
                if (target == null) continue;

                if (target.DetectState == EnemyBase.DETECTSTATE.DETECT)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }
        
    }
}
