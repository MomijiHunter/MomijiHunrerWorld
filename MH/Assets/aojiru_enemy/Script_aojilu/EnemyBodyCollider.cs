using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using kubota;

namespace aojilu {
    public class EnemyBodyCollider : MonoBehaviour
    {
        [SerializeField] EnemyBase enemyBase;
        
        private void OnTriggerEnter2D(Collider2D col)
        {

            if (col.tag == "PlayerAttack")
            {
                var data= col.GetComponent<PlayerAttack>();
                if (data == null)
                {
                    enemyBase.DamageAction(1);
                }
                else
                {
                    enemyBase.DamageAction(data.attackPower);
                }
            }
        }
    }
}
