using System.Collections;
using UnityEngine;

public class CarSpwaner : MonoBehaviour
{
    [MinMaxSlider(5, 10)] [SerializeField]
    Vector2 spawnDelay = new Vector2(5, 10);
    [SerializeField] bool isReady = true;
    [MinMaxSlider(5, 10)][SerializeField]
    private Vector2 speedRange = new Vector2(5, 10);
    Sc5Street manager;
    private void Start()
    {
        manager = FindObjectOfType<Sc5Street>();
        StartCoroutine(Spawn());
    }
    private void OnTriggerExit(Collider other)
    {
        isReady = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "car")
        {
            isReady = false;
        }
    }
    IEnumerator Spawn()
    {
        if (isReady)
        {
            int carIndex = Random.Range(0, manager.CarPrefabs.Length);
            GameObject car = Instantiate(manager.CarPrefabs[carIndex], transform);
            car.transform.localPosition = Vector3.zero;
            var r = car.AddComponent<Rigidbody>();
            r.useGravity = false;
            r.constraints = RigidbodyConstraints.FreezeAll;
            car.AddComponent<CarAI>().speed = Random.Range(speedRange.x, speedRange.y);
        }
        yield return new WaitForSeconds(Random.Range(spawnDelay.x, spawnDelay.y));
        StartCoroutine(Spawn());
    }
}
