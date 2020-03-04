using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boton4 : MonoBehaviour
{
    public Camera principal;
    public Camera planeta;
    public int c = 0;

    // Start is called before the first frame update
    void Start()
    {
        principal.enabled = true;
        planeta.enabled = false;
    }

    public void plan() { if (c == 0) { c = 1; } else { c = 0; } }

    void comprobante()
    {
        if (c == 0)
        {
            principal.enabled = true;
            planeta.enabled = false;
        }
        if (c == 1)
        {
            principal.enabled = false;
            planeta.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        comprobante();

    }
}
