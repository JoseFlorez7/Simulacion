using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitaMov : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform ObjetoOrb;
    public Ellipse OrbitPath;

    Boton boton;

    [Range(0f, 1f)]
    public float ProgresoOrb = 0f;
    public float PeriodoOrb = 5f;
    public bool ActivaOrb = true;


    void Start()
    {
        if (ObjetoOrb == null)
        {
            ActivaOrb = false;
            return;
        }

        posicion();
        StartCoroutine(AnimateOrbit());
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void posicion()
    {
        Vector2 Posorbita = OrbitPath.Evaluate(ProgresoOrb);
        ObjetoOrb.localPosition = new Vector3(Posorbita.x, Posorbita.y);
    }

    IEnumerator AnimateOrbit()
    {
        if(PeriodoOrb < 0.1f)
        {
            PeriodoOrb = 0.1f;
        }
        
        
        float VelOrb = 5f / PeriodoOrb;

        while (ActivaOrb)
        {
            ProgresoOrb += Time.deltaTime * VelOrb;
            ProgresoOrb %= 1f;
            posicion();
            yield return null;
        }
    }
}
