using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PersonBlock : MonoBehaviour
{
    [SerializeField] Collider[] colliders;
    [SerializeField] TrafficLight traffic;
    public bool isFirstRoad = true;
    private void Start()
    {
        colliders = GetComponents<Collider>().Where(item => !item.isTrigger).ToArray();
        traffic = transform.parent.GetComponent<TrafficLight>();
    }
    public void SetCollider(bool enable)
    {
        foreach(Collider c in colliders)
        {
            c.enabled = enable;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if (isFirstRoad)
            {
                traffic.isEnter1 = true;
            }
            else
            {
                traffic.isEnter2 = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            if (isFirstRoad)
            {
                traffic.isEnter1 = false;
            }
            else
            {
                traffic.isEnter2 = false;
            }
            traffic.IsFirstRoadRed = traffic.IsFirstRoadRed;
        }
    }
}
