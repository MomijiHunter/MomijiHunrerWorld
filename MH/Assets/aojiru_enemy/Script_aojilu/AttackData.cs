using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackData : MonoBehaviour
{
    [SerializeField] int attackPower;
    public int AttackPower { get { return attackPower; } }
    
    [SerializeField] int blowAwayLevel = 0;
    public int BlowAwayLevel { get { return blowAwayLevel; } }

    public void SetAttackPower(int n)
    {
        attackPower = n;
    }

    public void SetBlowAway(int level)
    {
        blowAwayLevel = level;
    }

}
