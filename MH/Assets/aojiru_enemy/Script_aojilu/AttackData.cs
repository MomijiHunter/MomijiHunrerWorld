using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class AttackData : MonoBehaviour
    {
        [SerializeField] int attackPower;
        public int AttackPower { get { return attackPower; } }

        public void SetAttackPower(int n)
        {
            attackPower = n;
        }
    }
