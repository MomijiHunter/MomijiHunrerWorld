using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixMap : MonoBehaviour
{
    Vector3 position;
    void Start()
    {
        position = transform.position;
    }
    
    void Update()
    {
        transform.position = position;
    }
}
