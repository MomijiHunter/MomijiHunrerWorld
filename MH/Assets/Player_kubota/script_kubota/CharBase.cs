using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kubota
{
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class CharBase : MonoBehaviour
    {
        [SerializeField] protected Rigidbody2D rb;
        [SerializeField] public float MaxHP, currentHP;

        protected virtual void Awake()
        {

        }

        protected virtual void Start()
        {

        }

        void Update()
        {

            CharacterUpdate();
        }

        protected virtual void CharacterUpdate()
        {

        }

        protected virtual void Move(float speed)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }

        protected virtual void Jump(float speed)
        {
            rb.velocity = new Vector2(rb.velocity.x, speed);
        }

        protected virtual bool IsDead()
        {
            if (currentHP <= 0) return true;
            return false;
        }

        protected virtual void Damage(int damage)
        {
            currentHP -= damage;
        }
    }
}
