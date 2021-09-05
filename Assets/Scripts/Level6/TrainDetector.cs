using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainDetector : MonoBehaviour
{
    [SerializeField] Collider collider;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "car")
        {
            collider.enabled = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "car")
        {
            collider.enabled = true;
        }
    }
}
