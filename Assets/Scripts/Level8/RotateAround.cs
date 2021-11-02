using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{
    public GameObject target;
    float timeLeft = 10.0f;
    void Update()
    {
        // Spin the object around the target at 20 degrees/second.
        transform.RotateAround(target.transform.position, Vector3.up, 170 * Time.deltaTime);
        
        timeLeft -= Time.deltaTime;
        
        if (timeLeft < 0)
        {
            this.gameObject.SetActive(false);
        }
    }
}
