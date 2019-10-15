using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayCastTest : MonoBehaviour
{
    [SerializeField] Vector3 pos;
    [SerializeField] Collider2D obj;
    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(pos, new Vector3(0, -10,0), 100);
        Debug.DrawRay(pos, new Vector3(0, -10, 0), Color.blue);

        Debug.Log(hit.collider);
        obj = hit.collider;
    }
}
