using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySimpleLiquid;

public class OnCollisionExp2 : MonoBehaviour
{
    public GameObject Beaker;
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "BeakerExp2")
        {
            Beaker.GetComponent<LiquidContainer>().fillAmountPercent = 0.9f;
        }
    }
}
