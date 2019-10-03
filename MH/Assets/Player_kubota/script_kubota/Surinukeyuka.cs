using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using kubota;


namespace kubota
{
    public class Surinukeyuka : MonoBehaviour
    {
        [SerializeField] Collider2D col;

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Player" && Input.GetKey(KeyCode.DownArrow))
            {
                col.isTrigger = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                col.isTrigger = false;
            }
        }
    }
}
