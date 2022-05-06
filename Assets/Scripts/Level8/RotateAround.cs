using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{

    public GameObject Target;
    float timeLeft = 10.0f;

    void Update()
    {
        transform.RotateAround(Target.transform.position, Vector3.up, 170 * Time.deltaTime);

        timeLeft -= Time.deltaTime;

        if (timeLeft < 0)
        {
            this.gameObject.SetActive(false);
        }

    }
}
