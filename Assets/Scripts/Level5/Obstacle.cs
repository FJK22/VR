using System;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    bool isBlock;
    public Transform target;
    [HideInInspector] public CarAI currentCar;
    [HideInInspector] public bool isLimit = false;
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
