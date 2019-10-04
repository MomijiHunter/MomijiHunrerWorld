using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire_sorairo_bunretu : MonoBehaviour
{
    [SerializeField] animationUtil myUtil;
    [SerializeField] Vector2 firstSpeed;
    [SerializeField] float chiltBreakSet;//子の破壊までの時間

    [SerializeField] animationUtil nextFire;
    [SerializeField] int fireCount;
    [SerializeField] float nextGravity;
    [SerializeField] float fireRange;

    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myUtil.SetVelocity(firstSpeed);
    }

    private void Update()
    {
        if (rb.velocity.y < 0.1)
        {
            DestroyAction();
        }
    }

    void DestroyAction()
    {

        float rangeUnit =(fireCount>0)?  2.0f / (fireCount - 1):0;

        for (int i = 0; i < fireCount; i++)
        {
            var obj = myUtil.InstanateObj(nextFire.gameObject, transform.position);
            var an = obj.GetComponent<animationUtil>();
            Vector2 makeRote = new Vector2(-1+rangeUnit*i, 0);
            an.SetVelocity(makeRote*fireRange);
            an.SetGravity(nextGravity);
            an.SetDedLength(chiltBreakSet);
        }


        Destroy(this.gameObject);
    }
}
