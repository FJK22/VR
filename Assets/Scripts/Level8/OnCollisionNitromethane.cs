using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySimpleLiquid;
using Valve.VR.InteractionSystem;
using Valve.VR.InteractionSystem.Sample;

public class OnCollisionNitromethane : MonoBehaviour
{
    public GameObject FluidPlate;

    void Start()
    {
        
    }


    void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.tag == "FluidPlate")
        {

            FluidPlate.GetComponent<LiquidContainer>().fillAmountPercent = 0.3f;
         


        }
    }
}
