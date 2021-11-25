using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySimpleLiquid;
using Valve.VR.InteractionSystem;
using Valve.VR.InteractionSystem.Sample;

public class OnCollisionPractice : MonoBehaviour
{
    public GameObject EmptyBeaker;


  

    void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.tag == "PracticeBeaker")
        {

            EmptyBeaker.GetComponent<LiquidContainer>().fillAmountPercent = 0.9f;
           


        }
    }
}
