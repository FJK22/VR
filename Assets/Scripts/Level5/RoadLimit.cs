using UnityEngine;

public class RoadLimit : MonoBehaviour
{
    [SerializeField] Obstacle obstacle;
    [SerializeField] int limit = 3;
    int carCount;
    int CarCount
    {
        get { return carCount; }
        set
        {
            carCount = value;
            if (obstacle) obstacle.isLimit = carCount >= limit;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "car")
        {
            CarCount++;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "car")
        {
            CarCount--;
        }
    }
}
