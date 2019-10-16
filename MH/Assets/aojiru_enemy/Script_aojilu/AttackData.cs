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
    [SerializeField] bool isBlowAway=false;
    public bool IsBlowAway { get { return isBlowAway; } }

    public void SetAttackPower(int n)
    {
        attackPower = n;
    }

    public void SetBlowAway(bool flag)
    {
        isBlowAway = flag;
    }
}
