using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadBreaker : MonoBehaviour
{
    EnemyController ec;

    [SerializeField] float deadLength = 10.0f;
    bool deadFlag = false;
    float deadStartTime;

    private void Awake()
    {
        ec = GetComponent<EnemyController>();
    }

    private void Update()
    {
        if (ec.IsDeadSelf())
        {
            if (deadFlag)
            {
                if (Time.fixedTime > deadStartTime + deadLength)
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                deadFlag = true;
                deadStartTime = Time.fixedTime;
            }
        }
    }


}
