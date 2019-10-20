using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace aojilu
{
    public class SEPlayer : MonoBehaviour
    {
        AudioSource audioSource;
        [SerializeField] AudioClip[] Ses;

        private void Awake()
        {
            audioSource=GetComponent<AudioSource>();
        }

        public void PlaySE_oneShot(int n)
        {
            if (Ses.Length > n)
            {
                audioSource.PlayOneShot(Ses[n]);
            }
            else
            {
                Debug.Log(gameObject.name + " : SEPlayer outRange");
            }
        }
    }
}
