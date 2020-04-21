using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimiento : MonoBehaviour
{
    //VARIABLES

    public Camera Cam;
    private GameObject Dron = null;
    Animator anim;
    public GameObject Acol;

    //VARIABLES DE KUTTA
    float t = 0;
    float K1, K2, K3, K4, L1, L2, L3, L4;
    public float h = 0.005f;
    public float g = 9.8f;

    //VARIABLES DRON
    Vector3 pos;
    public Vector3 vel;
    Vector3 acel;
    Vector3 rot;
    float v01;
    bool piso = true;
    public int a = 0;

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        Dron = GameObject.Find("avio_animado");
        //pos = new Vector3(0, -0.221f, 0);
        //Dron.GetComponent<Transform>().position = pos;
        Cam.GetComponent<Transform>().position = new Vector3(-5f,2f,0);
        pos = Dron.GetComponent<Transform>().position;
        vel = new Vector3(0, 0, 0);
        acel = new Vector3(2f, 2f, 2f);
        v01 = 5f;
        rot = new Vector3(0, 0, 0);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        mov();

        Botones();
        rotation();
        colision();
    }

    void Botones()
    {
        if (Input.GetKey("w") || Input.GetKey("s")) {
            if (Input.GetKey("w"))
            {
                if (vel.x < 500)
                {
                    vel.x += acel.x;
                    rot.z += Time.deltaTime * -2.5f;
                    a = 1;
                }
            }
            if(Input.GetKey("s"))
            {
                if (vel.x > -400)
                {
                    vel.x -= acel.x;
                    rot.z += Time.deltaTime * 2.5f;
                    a = 2;
                }
            }
        } else {
            if (vel.x > 0)
            {
                rot.z += Time.deltaTime * 14f;
                vel.x -= acel.x*8;
            }
            else
            {
                rot.z += Time.deltaTime * -14f;
                vel.x += acel.x*8;
            }
        }


        if (Input.GetKey("a") || Input.GetKey("d"))
        {
            if (Input.GetKey("a"))
            {
                if (vel.z < 420)
                {
                    rot.x += Time.deltaTime *2f;
                    vel.z += acel.z;
                    a = 3;
                }
            }
            if (Input.GetKey("d"))
            {
                if(vel.z > -420)
                {
                    rot.x += Time.deltaTime * -2f;
                    vel.z -= acel.z;
                    a = 4;
                }
            }
        }
        else
        {
            if (vel.z > 0) {
                rot.x += Time.deltaTime * -16f;
                vel.z -= acel.z * 8;
            }
            else
            {
                rot.x += Time.deltaTime * 16f;
                vel.z += acel.z * 8;
            }
            
        }

        if(Input.GetKey("space") || Input.GetKey("c"))
        {
           
            if (Input.GetKey("space"))
            {
                anim.SetBool("mov", true);
                if (vel.y < 45)
                {
                    vel.y += acel.y;
                    a = 5;
                }
            }
            if (Input.GetKey("c"))
            {
                if(vel.y > -45)
                {
                    vel.y -= acel.y;
                    a = 6;
                }
            }
        }
        else
        {
            if (vel.y > 0)
            {
                vel.y -= acel.y;
            }
            else
            {
                vel.y += acel.y;
            }
        }

        if (Input.GetKey("p"))
        {
            Start();
        }
    }

    void rotation()
    {

        if (vel.z > -4 && vel.z < 4)
        {
            if (rot.x > 0)
            {
                rot.x += Time.deltaTime * -5f;
            }
            else
            {
                rot.x += Time.deltaTime * 5f;
            }
        }
        if (vel.x > -4 && vel.x < 4)
        {
            if (rot.z > 0)
            {
                rot.z += Time.deltaTime * -5f;
            }
            else
            {
                rot.z += Time.deltaTime * 5f;
            }
        }
        Dron.GetComponent<Transform>().localRotation = Quaternion.Euler(rot.x, rot.y, rot.z);
    }

    void mov()
    {
        //DETERMINANDO LAS POSICIONES Y VELOCIDADES EN LAS DIFERENTES DIRECCIONES
        pos.x += runge_pos(pos.x, vel.x, 1).x;
        vel.x += runge_pos(pos.x, vel.x, 1).y;

        pos.y += runge_pos(pos.y, vel.y, 2).x;
        vel.y += runge_pos(pos.y, vel.y, 2).y;

        pos.z += runge_pos(pos.z, vel.z, 3).x;
        vel.z += runge_pos(pos.z, vel.z, 3).y;


        Cam.GetComponent<Transform>().position = new Vector3(pos.x - 6f, pos.y + 2f, pos.z);
        Dron.GetComponent<Transform>().position = pos;

        //Debug.Log("POs="+ pos);
        //Debug.Log("Vel=" + vel);
    }
    
    void colision()
    {
        //Area De Colision
        float scale = Acol.GetComponent<Transform>().localScale.y;
        if ((pos.y - scale) < 0 && piso)
        {
            vel.y = vel.y * -0.4f;
            piso = false;
        }
        if(piso == false && pos.y > 1)
        {
            piso = true;
        }

        Vector3 a = pos;
        Acol.GetComponent<Transform>().position = new Vector3(a.x, a.y, a.z);
    }

    public Vector2 runge_pos(float pos, float v, int cond)
    {//EL METODO DE RUNGE HACE LAS RESPECTIVAS OPERACIONES DEL METODO MATEMATICO y RETORNA EL VALOR DE LA POSICION

        K1 = h * v;
        L1 = h * func(t, pos, v, cond);
        K2 = h * (v + (L1 / 2));
        L2 = h * func(t + (h / 2), pos + (K1 / 2), v + (L1 / 2), cond);
        K3 = h * (v + (L2 / 2));
        L3 = h * func(t + (h / 2), pos + (K2 / 2), v + (L2 / 2), cond);
        K4 = h * (v + L3);
        L4 = h * func(t + h, pos + K3, v + L3, cond);

        t = t + h;

        //DEVULVE LA POSICION CALCULADA POR EL METODO
        pos = ((K1 + 2 * K2 + 2 * K3 + K4) / 6);
        v = ((L1 + 2 * L2 + 2 * L3 + L4) / 6);

        Vector2 res;
        res = new Vector2(pos, v);

        //RETORNA LA POSICION
        return res;
    }
    float func(float t, float x1, float vx1, int cond)
    {
        if(cond <= 1)
        {
            return 0;
        }
        if(cond <= 2)
        {
            float res = -g;
            return res;
        }else
        {
            return 0;
        }
 
    }

}
