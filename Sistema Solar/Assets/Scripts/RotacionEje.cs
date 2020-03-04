using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotacionEje : MonoBehaviour
{

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 25 * Time.deltaTime, 0); 
    }
}
