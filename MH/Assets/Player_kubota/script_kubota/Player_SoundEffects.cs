using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using kubota;

namespace kubota
{
    public class Player_SoundEffects : MonoBehaviour
    {
        [SerializeField] AudioSource au;
        [SerializeField] AudioClip attack, damage, walk, jump, heal, stamp;

        public void AttackSound()
        {
            au.PlayOneShot(attack);
        }

        public void DamageSound()
        {
            au.PlayOneShot(damage);
        }

        public void WalkSound()
        {
            au.PlayOneShot(walk);
        }

        public void JumpSound()
        {
            au.PlayOneShot(jump);
        }

        public void HealSound()
        {
            au.PlayOneShot(heal);
        }
    }
}