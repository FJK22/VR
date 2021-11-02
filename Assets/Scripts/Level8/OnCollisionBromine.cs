using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySimpleLiquid;
using Valve.VR.InteractionSystem;
using Valve.VR.InteractionSystem.Sample;

public class OnCollisionBromine : MonoBehaviour
{
    public GameObject BeakerExam2;
    public GameObject Aluminum;
    public MeshCollider meshColliderAluminum;

    private void Start()
    {
        meshColliderAluminum = Aluminum.GetComponent<MeshCollider>();
        meshColliderAluminum.enabled = false;

  
    }

    void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.tag == "BeakerExam2")
        {

            BeakerExam2.GetComponent<LiquidContainer>().fillAmountPercent = 0.9f;
            meshColliderAluminum.enabled = true;
       

        }
    }
}
