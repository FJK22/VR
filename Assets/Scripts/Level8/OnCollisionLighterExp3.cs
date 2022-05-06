using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OnCollisionLighterExp3 : MonoBehaviour
{
    public GameObject PlateExp3;
    public GameObject FlameExp3;

    void Start()
    {
        FlameExp3.SetActive(false);
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "PlateExp3")
        {
            FlameExp3.SetActive(true);
        }
    }
}
