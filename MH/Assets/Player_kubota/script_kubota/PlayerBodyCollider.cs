using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using kubota;

namespace kubota
{
    public class PlayerBodyCollider : MonoBehaviour
    {
        [SerializeField] Player player;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            GameObject col = collision.gameObject;

            if (col.tag == "EnemyAttack")
            {
                player.DamageAction(col.GetComponent<AttackData>().AttackPower);
            }
        }
    }
}