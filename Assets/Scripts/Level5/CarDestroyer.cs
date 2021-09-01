using UnityEngine;

public class CarDestroyer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "car")
        {
            Destroy(other.gameObject);
        }
    }
}
