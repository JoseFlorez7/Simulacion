using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colider : MonoBehaviour
{
    public Movimiento movimiento;
    public GameObject obj;
    private GameObject objCol;

    float m1, m2;
    float e;
    float teta, fi, alfa;

    Vector3 vel1;
    Vector3 vel2;

    Vector3 pos1;
    Vector3 pos2;
    static Vector3 scal1 = new Vector3(4f, 0.6f, 4.5f);
    Vector3 scal2;

    Vector3 max1, min1, max2, min2;
    Vector3 dis;

    float vp, vn, vt;
    float vp2, vn2, vt2;
    float v1prim, vx1prim, vy1prim, vz1prim;
    float v2prim, vx2prim, vy2prim, vz2prim;

    bool comp;


    // Start is called before the first frame update
    void Start()
    {
        objCol = GameObject.Find("AreaCol");

        vel1 = new Vector3(0, 0, 0);
        vel2 = new Vector3(0, 0, 0); 

        pos1 = objCol.GetComponent<Transform>().position;
        pos2 = obj.GetComponent<Transform>().position;
        //scal1 = objCol.GetComponent<Transform>().localScale;
        //scal1 = 
        scal2 = obj.GetComponent<Transform>().localScale;

        e = 0.98f;

        m1 = 4f;
        m2 = Mathf.Pow(scal2.x, 1) * 2;

        vp = vn = vn2 = vp2 = 0;
        v1prim = vy1prim = vx1prim = 0;

        comp = true;

        Debug.Log("masa1 =" + m1);
        Debug.Log("masa2 =" + m2);
    }

    // Update is called once per frame
    void Update()
    {
        actualizar();
        MaximosMinimos();
        dete();

        suelo();
        mov();
    }

    void MaximosMinimos()
    {
        max1.x = pos1.x + (scal1.x/2f);
        min1.x = pos1.x - (scal1.x/2f);
        max1.y = pos1.y + (scal1.y/2f);
        min1.y = pos1.y - (scal1.y/2f);
        max1.z = pos1.z + (scal1.z/2f);
        min1.z = pos1.z - (scal1.z/2f);

        max2.x = pos2.x + (scal2.x / 2f);
        min2.x = pos2.x - (scal2.x / 2f);
        max2.y = pos2.y + (scal2.y / 2f);
        min2.y = pos2.y - (scal2.y / 2f);
        max2.z = pos2.z + (scal2.z / 2f);
        min2.z = pos2.z - (scal2.z / 2f);

    }

    void mov()
    {
        pos2 += vel2;
        vel2 = vel2 * e;
        obj.GetComponent<Transform>().position = pos2;
    }

    void actualizar()
    {
        pos1 = objCol.GetComponent<Transform>().position;
    }

    void dete()
    {

        float de;
        float de2;
        float de3;
        float de4;
        float aux123;
        int b = movimiento.a;

        if ((min1.x < max2.x) && (max1.x > min2.x) && (min1.y < max2.y) && (max1.y > min2.y) && (min1.z < max2.z) && (max1.z > min2.z))
        {
            //Debug.Log("HAS PENETRADO");
            vel1 = movimiento.vel;

            de = Mathf.Sqrt(Mathf.Pow(pos2.x - pos1.x, 2) + Mathf.Pow(pos2.y - pos1.y, 2) + Mathf.Pow(pos2.z - pos1.z, 2));

            dis.x = Mathf.Abs(pos2.x - pos1.x);
            dis.y = Mathf.Abs(pos2.y - pos1.y);
            dis.z = Mathf.Abs(pos2.z - pos1.z);

            if(b == 3 || b == 4)
            {
                vel1.x = vel1.z;
            }
            if(b==5 || b== 6)
            {
                aux123 = vel1.x;
                vel1.x = vel1.y;
                vel1.y = aux123;
            }

            de2 = Mathf.Sqrt(Mathf.Pow(dis.x, 2) + Mathf.Pow(dis.z, 2));
            teta = Mathf.Acos(dis.x / de2);

            vp = vel1.x * Mathf.Cos(teta) + vel1.y * Mathf.Sin(teta);
            vn = -vel1.x * Mathf.Sin(teta) + vel1.y * Mathf.Cos(teta);
            
            //------------------------
            vp2 = vel2.x * Mathf.Cos(teta) + vel2.y * Mathf.Sin(teta);
            vn2 = -vel2.x * Mathf.Sin(teta) + vel2.y * Mathf.Cos(teta);

            v1prim = ((m1 - (e * m2)) / (m1 + m2)) * vel1.x + (((1 + e) * m2) / (m1 + m2)) * vel2.x;
            vx1prim = v1prim * Mathf.Cos(teta) - vn * Mathf.Sin(teta);
            vy1prim = v1prim * Mathf.Sin(teta) + vn * Mathf.Cos(teta);
            //--------------------------
            v2prim = ((m2 - (e * m1)) / (m1 + m2)) * vel2.x + (((1 + e) * m1) / (m1 + m2)) * vel1.x;
            vx2prim = v2prim * Mathf.Cos(teta) - vn * Mathf.Sin(teta);
            vy2prim = v2prim * Mathf.Sin(teta) + vn * Mathf.Cos(teta);

            movimiento.vel = new Vector3(vx1prim, vy1prim, 0);
            if (b == 3 || b == 4)
            {
                vel2 = (new Vector3(0, vy2prim, vx2prim)) * 0.001f;
            }
            else
            {
                vel2 = (new Vector3(vx2prim, vy2prim, 0)) * 0.001f;
            }
            if(b==5 || b == 6)
            {
                vel2 = (new Vector3(vy2prim, vx2prim, 0)) * 0.001f;
            }
        }
    }

    void suelo()
    {
        pos2.x += movimiento.runge_pos(pos2.x, vel2.x,1).x;
        vel2.x += movimiento.runge_pos(pos2.x, vel2.x, 1).y;

        pos2.y += movimiento.runge_pos(pos2.y, vel2.y, 2).x;
        vel2.y += movimiento.runge_pos(pos2.y, vel2.y, 2).y;

        pos2.z += movimiento.runge_pos(pos2.z, vel2.z, 3).x;
        vel2.z += movimiento.runge_pos(pos2.z, vel2.z, 3).y;

        if(pos2.y - (scal2.y/2) < 0)
        {
            vel2.y = vel2.y * -0.4f;
        }
    }
}

