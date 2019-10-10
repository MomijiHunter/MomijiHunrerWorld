using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using kubota;
using UnityEngine.Events;
using UnityEngine.UI;

namespace kubota
{
    public class Player : CharBase
    {
        public float moveSpeed = 5;
        public float jumpPower = 15;
        private bool jumping = false;

        [SerializeField] IsGrounded isGrounded;
        [SerializeField] Animator anim;
        [SerializeField] bool canChainCombo;
        public bool attacking = false;
        public bool canNotMove = false;
        public bool superArmor = false;
        public bool invincible = false;

        public bool hiding = false;

        public bool dead = false;

        public int remainItems = 3;

        List<UnityAction> actions = new List<UnityAction>();
        List<UnityAction> actions_Air = new List<UnityAction>();

        [SerializeField] PlayerAttack attack;
        [SerializeField] Player_Lifebar lifebar;

        [SerializeField] ManjuuIcon manjuu;

        Player_SoundEffects SE;
        

        protected override void Start()
        {
            actions.Add(() => Attack1());
            actions_Air.Add(() => Air_Attack1());

            ChangeAnimationSpeed(1.3f);

            lifebar = GameObject.FindGameObjectWithTag("Player_LifeBar").GetComponent<Player_Lifebar>();
            manjuu = GameObject.FindGameObjectWithTag("MomijiManjuu").GetComponent<ManjuuIcon>();

            SE = GameObject.FindGameObjectWithTag("Player_Sound").GetComponent<Player_SoundEffects>();
        }

        protected override void CharacterUpdate()
        {
            if (!dead)
            {
                float x = Input.GetAxisRaw("Horizontal");

                if (x < 0 && !attacking)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                else if (x > 0 && !attacking)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }

                if (Input.GetAxisRaw("Horizontal") != 0 && !canNotMove)
                {
                    Move(moveSpeed * x);
                    anim.SetBool("Walk", true);
                }
                else
                {
                    if (!canNotMove)
                    {
                        rb.velocity = new Vector2(0, rb.velocity.y);
                    }
                    anim.SetBool("Walk", false);
                }

                if (Input.GetButtonDown("Cancel") && !attacking)
                {
                    if (isGrounded.isGrounded)
                    {
                        StartCoroutine(Jump());
                    }
                }
                if (Input.GetButton("Cancel") && jumping)
                {
                    Jump(jumpPower);
                }
                if (Input.GetButtonUp("Cancel"))
                {
                    jumping = false;
                }
                if (rb.velocity.y > 0 && !isGrounded.isGrounded)
                {
                    anim.SetBool("Jump", true);
                }
                else if (rb.velocity.y < 0 && !isGrounded.isGrounded)
                {
                    anim.SetBool("Fall", true);
                }
                else if (isGrounded.isGrounded)
                {
                    anim.SetBool("Jump", false);
                    anim.SetBool("Fall", false);
                }

                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    anim.SetBool("Hide", true);
                }
                else if (Input.GetKeyUp(KeyCode.DownArrow))
                {
                    anim.SetBool("Hide", false);
                }

                if (Input.GetButtonDown("Submit"))
                {
                    Attack();
                }


                currentHP = Mathf.Clamp(currentHP, 0, MaxHP);
                if (Input.GetButtonDown("ButtonX"))
                {
                    Eat();
                }
            }
        }

        private IEnumerator Jump()
        {
            jumping = true;
            yield return new WaitForSeconds(0.3f);
            jumping = false;
        }

        void Attack()
        {
            if (isGrounded.isGrounded)
            {
                if (canChainCombo || !attacking)
                {
                    actions[0].Invoke();
                }
            }
            else
            {
                actions_Air[0].Invoke();
            }
        }

        private void Attack1()
        {
            attacking = true;
            anim.SetTrigger("Attack");
        }

        private void Air_Attack1()
        {
            anim.SetTrigger("Attack_Air");
            canChainCombo = false;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            GameObject col = collision.gameObject;

            if (collision.gameObject.tag == "DropItem")
            {
                PickItem(collision.gameObject);
            }
        }

        public void DamageAction(int damage)
        {
            if (!invincible)
            {
                if (damage >= 1)
                {
                    StartCoroutine(BlowAway());
                }
                else if (!superArmor)
                {
                    anim.SetTrigger("DamageReaction");
                }
                anim.SetTrigger("Damage");
                Damage(damage);
                if (IsDead())
                {
                    dead = true;
                    rb.velocity = Vector2.zero;
                    ChangeAnimationSpeed(0.8f);
                    anim.SetBool("Death", true);
                }
                lifebar.Damage();
            }
        }

        IEnumerator BlowAway()
        {
            canNotMove = true;
            anim.SetTrigger("BlowAway");
            yield return new WaitForEndOfFrame();
            rb.velocity = new Vector2(-50, 50);
            yield return new WaitForSeconds(0.5f);
            yield return new WaitUntil(() => isGrounded.isGrounded);
            anim.SetBool("Down", true);
            yield return new WaitUntil(() => Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Vertical") > 0 || Input.GetButton("Submit"));
            anim.SetBool("Down", false);
        }

        void Heal()
        {
            remainItems -= 1;
            currentHP += 15;
            lifebar.Heal();
            manjuu.RemoveManjuu();
        }

        void Eat()
        {
            if (remainItems > 0 && !canNotMove)
            {
                anim.SetTrigger("Heal");
            }
        }

        void PickItem(GameObject obj)
        {
            if (obj.GetComponent<DropMomiji>().canPick)
            {
                Destroy(obj);
                remainItems++;
                manjuu.AddManjuu();
            }
        }

        void ChangeAnimationSpeed(float speed)
        {
            anim.speed = speed;
        }


        void AttackSound()
        {
            SE.AttackSound();
        }
        void DamageSound()
        {
            SE.DamageSound();
        }
        void WalkSound()
        {
            SE.WalkSound();
        }
        void JumpSound()
        {
            SE.JumpSound();
        }
        void HealSound()
        {
            SE.HealSound();
        }
    }
}
