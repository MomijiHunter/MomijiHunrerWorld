using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayCastObject : MonoBehaviour
{
    [SerializeField] Transform stratPos;
    [SerializeField] Vector2 direction;
    [SerializeField] float distance;
    RaycastHit2D hit;

    [SerializeField] bool isDrawRay;

    [SerializeField] string[] tags;
    [SerializeField] LayerMask mask;

    [SerializeField] bool isHitTarget;
    public bool IsHitTarget { get { return isHitTarget; } }

    private void Update()
    {
        hit = Physics2D.Raycast(stratPos.position, direction, distance,mask);

        isHitTarget = CheckTags(hit.collider);

        if (isDrawRay)
        {
            Debug.DrawRay(stratPos.position, direction*distance, Color.blue);
        }
    }

    bool CheckTags(Collider2D col)
    {
        if (col == null) return false;
        bool result = false;
        foreach(var tag in tags)
        {
            if (col.tag == tag) result = true;
        }
        return result;
    }
}
