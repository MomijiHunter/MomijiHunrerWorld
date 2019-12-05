using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using aojilu;

public class FixEnemy : MonoBehaviour
{
    MonstarMapChengeCtrl ec;
    [SerializeField] float ResetDistance;
    Transform tr;
    MapParent nowParent;
    float nowTIme;

    private void Awake()
    {
        ec = GetComponent<MonstarMapChengeCtrl>();
        tr = transform;
        nowParent = ec.nowMapParent;
    }

    private void Update()
    {
        float dist = Mathf.Abs(tr.position.y - nowParent.transform.position.y);
        if (dist > ResetDistance)
        {
            nowTIme += Time.deltaTime;
            if (nowTIme > 3.0f)
            {
                ResetPOs();
                nowTIme = 0;
            }
        }
        else
        {
            nowTIme = 0;
        }
    }


    [ContextMenu("Reset")]
    public void ResetPOs()
    {
        ec.transform.position = ec.nowMapParent.centerPos.position;
        //ec.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }
}
