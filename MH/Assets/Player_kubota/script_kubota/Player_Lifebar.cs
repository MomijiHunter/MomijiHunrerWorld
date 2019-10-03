using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using kubota;

namespace kubota
{
    public class Player_Lifebar : MonoBehaviour
    {
        Player player;
        public Image lifeRed, lifeGreen, lifeYellow;
        public bool healing;
        
        void Start()
        {
            player = GameObject.Find("PlayerAll").GetComponent<Player>();
        }
        
        void Update()
        {
            if (!healing)
            {
                lifeRed.fillAmount = Mathf.Lerp(lifeRed.fillAmount, lifeGreen.fillAmount, 2 * Time.deltaTime);
            }
            else
            {
                lifeGreen.fillAmount = Mathf.MoveTowards(lifeGreen.fillAmount, lifeYellow.fillAmount, 0.25f * Time.deltaTime);
            }
        }

        public void Heal()
        {
            healing = true;
            lifeRed.fillAmount = player.currentHP / player.MaxHP;
            lifeYellow.fillAmount = player.currentHP / player.MaxHP;
        }

        public void Damage()
        {
            healing = false;
            lifeGreen.fillAmount = player.currentHP / player.MaxHP;
            lifeYellow.fillAmount = player.currentHP / player.MaxHP;
        }
    }
}
