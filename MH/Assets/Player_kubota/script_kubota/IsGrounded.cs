using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kubota
{
    public class IsGrounded : MonoBehaviour
    {
        public bool isGrounded = false;

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Ground")
            {
                isGrounded = true;
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Ground")
            {
                isGrounded = false;
            }
        }
    }

}