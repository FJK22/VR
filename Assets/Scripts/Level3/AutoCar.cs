using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoCar : MonoBehaviour
{
    float startTime = -1;
    float showTime;
    float speed;
    public void Set(float ShowTime, float Speed)
    {
        showTime = ShowTime;
        speed = Speed;
        startTime = Time.time;
    }
    private void Update()
    {
        if (startTime < 0) return;
        if(Time.time - startTime > showTime)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }
    }
}