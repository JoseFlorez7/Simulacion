﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RungeKutta : MonoBehaviour
{
    public float m = 1.5f;
    public float am = 0.08f;
    public float k = 60f;
    public float px, py;
    float g = 1.2f;

    float t = 0;
    float K1, K2, K3, K4, L1, L2, L3, L4;
    float k1, k2, k3, k4, l1, l2, l3, l4;
    float h = 0.1f;
    float vx = 0f;
    float vy = 0f;
    Vector3 pos;
    float xo= 0, yo = 0;

    bool cond = false;

    // Start is called before the first frame update
    void Start()
    {
        cond = false;
        t = 0;
        h = 0.1f;

        yo = px;
        xo = px;
        pos = new Vector3(xo, yo, 0);

        vx = 0f;
        vy = -yo * Mathf.Sqrt(k / m) * Mathf.Sin(Mathf.Sqrt(k / m));
    }

    // Update is called once per frame
    void Update()
    {
        runge();
        gameObject.GetComponent<Transform>().position = pos;


        if (Input.GetKeyDown("space"))
        {
            cond = true;
        }
        if (Input.GetKeyDown("p"))
        {
            Start();
        }
        if (Input.GetKeyDown("a"))
        {
            positionx();
        }

    }
    void positionx()
    {
        gameObject.GetComponent<Transform>().position = new Vector3(px, py, 0);
        pos.y = py;
        pos.x = px;
    }

    void runge()
    {
        if (cond)
        {
            K1 = h * vy;
            L1 = h * func(t, pos.y, vy);
            K2 = h * (vy + (L1 / 2));
            L2 = h * func(t + (h / 2), pos.y + (K1 / 2), vy + (L1 / 2));
            K3 = h * (vy + (L2 / 2));
            L3 = h * func(t + (h / 2), pos.y + (K2 / 2), vy + (L2 / 2));
            K4 = h * (vy + L3);
            L4 = h * func(t + h, pos.y + K3, vy + L3);

            k1 = h * vx;
            l1 = h * func1(t, pos.x, vx);
            k2 = h * (vx + (l1 / 2));
            l2 = h * func1(t + (h / 2), pos.x + (k1 / 2), vx + (l1 / 2));
            k3 = h * (vx + (l2 / 2));
            l3 = h * func1(t + (h / 2), pos.x + (k2 / 2), vx + (l2 / 2));
            k4 = h * (vx + l3);
            l4 = h * func1(t + h, pos.x + k3, vx + l3);

            t = t + h;

            pos.y = pos.y + ((K1 + 2 * K2 + 2 * K3 + K4) / 6);
            vy = vy + ((L1 + 2 * L2 + 2 * L3 + L4) / 6);

            pos.x = pos.x + ((k1 + 2 * k2 + 2 * k3 + k4) / 6);
            vx = vx + ((l1 + 2 * l2 + 2 * l3 + l4) / 6);
        }
    }

    float func(float t, float x1, float vx1)
    {
        float res;
        res = (-am*vx1 - k * x1)/m;
        //res = ((k * (L - Lo) * ((Le-x1) / L)) - (m*g)) / m;
        return res;
    }

    float func1(float t, float x1, float vx1)
    {
        float res;
        res = 0;
        return res;
    }



}
