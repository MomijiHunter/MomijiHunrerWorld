using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackData : MonoBehaviour
{
    [SerializeField] int attackPower;
    public int AttackPower { get { return attackPower; } }

    /// <summary>
    /// 吹っ飛ぶかどうか
    /// </summary>
    [SerializeField] int isBlowAway=0;
    public int IsBlowAway { get { return isBlowAway; } }

    public void SetAttackPower(int n)
    {
        attackPower = n;
    }

    public void SetBlowAway(int flag)
    {
        isBlowAway = flag;
    }
}
