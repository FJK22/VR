using System;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    bool isBlock;
    public Transform target;
    public Action Alarm;
    [HideInInspector] public CarAI currentCar;

    public bool IsBlock
    {
        get { return isBlock; }
        set
        {
            isBlock = value;
            if (!isBlock && currentCar) currentCar.WaitRelease();
        }
    }
}
