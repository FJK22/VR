using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisionLighter : MonoBehaviour
{
    public GameObject BurnerFlame;
    void Start()
    {
        BurnerFlame.SetActive(false);
    }

   

    void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.tag == "FluidPlate")
        {

            BurnerFlame.SetActive(true);



        }
    }
}
