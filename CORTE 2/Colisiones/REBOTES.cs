using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class REBOTES : MonoBehaviour
{
    //VARIABLES INICIALES
    float t = 0;
    float K1, K2, K3, K4, L1, L2, L3, L4;
    float h = 0.1f;
    public float vx1 = 0f;
    public float vx2 = 0f;
    public float vy1 = 0f;
    public float vy2 = 0f;
    float xo = 0, yo = 0;
    bool coli, coli2;
    float d;
    bool condM;

    public float m1, m2;
    float e;

    public GameObject Esf1;
    public GameObject Esf2;

    Vector3 pos1;
    Vector3 pos2;
    Vector2 pos1ini;
    Vector2 pos2ini;

    // Start is called before the first frame update
    void Start() //DEFINIR LOS PARAMETROS INICIALES 
    {
        pos1ini = new Vector3(-3.93f, -4.87f, -0.56f);
        pos2ini = new Vector3(3.96f, -4.87f, -0.56f);
        pos1 = pos1ini;
        pos2 = pos2ini;

        coli = false;
        coli2 = true;
        d = 0;
        condM = false;

        Esf1.GetComponent<Transform>().position = new Vector3(pos1.x, pos1.y, 0);
        Esf2.GetComponent<Transform>().position = new Vector3(pos2.x, pos2.y, 0);
        e = 0.9f;
        vx1 = 0.4f;
        vx2 = -0.4f;
        vy1 = vy2 = 0.3f;
    }

    // Update is called once per frame
    void Update()
    {
        //COMPROBANTE DE LA EJECUCIOND EL PROGRAMA
        if (condM)
        {
            mov(); 
        }
        
        //TECLAS
        if (Input.GetKeyDown("space"))
        {//DEPURAR PROGRMA
            condM = true;
        }//PAUSAR Y REINICIAR
        if (Input.GetKeyDown("p"))
        {
            Start();
        }
    }

    void mov()
    {
        //ESTOS VALORES ALMACENAN LAS POSICIONES X y Y DE LAS PELOTAS
        //EN BASE AL METODO DE RUNGE QUE LAS CALCULA
        pos1.x += runge_pos(pos1.x,vx1,1);
        pos2.x += runge_pos(pos2.x, vx2, 2);
        pos1.y += runge_pos(pos1.y, vy1, 1);
        pos2.y += runge_pos(pos2.y, vy2, 2);

        //LA VARIABLE DE CALCULA LA DISTANCIA DE LAS PELOTAS
        d = Mathf.Sqrt(Mathf.Pow(pos2.x - pos1.x, 2) + Mathf.Pow(pos2.y - pos1.y, 2));
        
        //EL CONDICIONAL DETECTA CUANDO LA VARIABLE d ES MENOR A LA DISTANCIA DE LAS PELOTAS PARA DETECTAR EL CHOQUE
        if (d > 1.2)
        { //LA VARIABLE coli ES NECESARIA PARA ENTRAR EN UN CONDICIONAL EN EL METODO DE runge()
            //SI ENTRA EN EL CONDICIONAL SE REALIZAN LAS RESPECTIVAS OPERACIONES DEL CALCULO DE LA VELOCIDAD DESPUES DE LA COLISION
            coli = false;
        }
        else
        {
            coli = true;
        }

        //ACTUALIZA LAS POSICIONES EN UNITY
        Esf1.GetComponent<Transform>().position = new Vector3(pos1.x, pos1.y, 0);
        Esf2.GetComponent<Transform>().position = new Vector3(pos2.x, pos2.y, 0);
    }

    float runge_pos(float pos, float v,int cnd)
    {//EL METODO DE RUNGE HACE LAS RESPECTIVAS OPERACIONES DEL METODO MATEMATICO y RETORNA EL VALOR DE LA POSICION

        K1 = h * v;
        L1 = h * func(t, pos, v);
        K2 = h * (v + (L1 / 2));
        L2 = h * func(t + (h / 2), pos + (K1 / 2), v + (L1 / 2));
        K3 = h * (v + (L2 / 2));
        L3 = h * func(t + (h / 2), pos + (K2 / 2), v + (L2 / 2));
        K4 = h * (v + L3);
        L4 = h * func(t + h, pos + K3, v + L3);

        t = t + h;

        //DEVULVE LA POSICION CALCULADA POR EL METODO
        pos = ((K1 + 2 * K2 + 2 * K3 + K4) / 6);
        
        //CALCULA LA POSICION QUE DIO EL METODO
        if(cnd == 1) { vx1 = vx1 + ((L1 + 2 * L2 + 2 * L3 + L4) / 6); }
        if(cnd == 2) { vx2 = vx2 + ((L1 + 2 * L2 + 2 * L3 + L4) / 6); }

        //SI DETECTA LA COLISION QUE ES UN VALOR QUE SE EVALUA EN EL METODO "mov()"
        //ENTONCES ENTRA Y HACE LAS RESPECTIVAS OPERACIONES MATEMATICAS PROPUESTAS EN EL LIBRO
        if (coli && coli2)
        {
            //CALCULO DE VELOCIDAD POST-COLISION EN X
            vx1 = ((m1 - (e * m2)) / (m1 + m2)) * vx1 + (((1 + e) * m2) / (m1 + m2)) * vx2; 
            vx2 = ((m2 - (e * m1)) / (m1 + m2)) * vx2 + (((1 + e) * m1) / (m1 + m2)) * vx1;

            //CALCULO DE VELOCIDAD POST-COLISION EN Y
            vy1 = ((m1 - (e * m2)) / (m1 + m2)) * vy1 + (((1 + e) * m2) / (m1 + m2)) * vy2;
            vy2 = ((m2 - (e * m1)) / (m1 + m2)) * vy2 + (((1 + e) * m1) / (m1 + m2)) * vy1;
            coli2 = false;
        }

        //RETORNA LA POSICION
        return pos;
    }

    //FUNCION QUE DETERMINA EL MOVIMIENTO SEGUN LA ECUACION DIFERENCIAL
    //EN ESTE CASO NO ESTAMOS TOMANDO UNA ACELERACION, POR LO CUAL ES CONTANTE(0)
    float func(float t, float x1, float vx1)
    {
        float res = 0;
        return res;
    }
}
