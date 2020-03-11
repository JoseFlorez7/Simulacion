using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movcam : MonoBehaviour
{
    public Camera cam;
    public Transform obj;
    public float distancia = 0f;

    public void poscam()
    {
        Vector3 pos = cam.GetComponent<Transform>().position;
        Vector3 posobj = obj.GetComponent<Transform>().position;
        pos = posobj;
        pos.z = posobj.z - distancia;
        cam.GetComponent<Transform>().position = pos;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        poscam();
    }
}
