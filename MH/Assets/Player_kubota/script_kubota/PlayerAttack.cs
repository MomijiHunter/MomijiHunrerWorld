using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kubota
{
    public class PlayerAttack : MonoBehaviour
    {
        public int attackPower;
        [SerializeField] Player player;

        //攻撃が当たった敵のリスト
        public List<GameObject> attackedEnemy = new List<GameObject>();

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.name == "momiji")
            {
                if (attackedEnemy.Contains(collision.gameObject))
                    attackedEnemy.Add(collision.gameObject);
                player.DisableAttack();
            }
        }
    }
}
