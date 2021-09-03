using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Train : MonoBehaviour
{
    [SerializeField] GameObject[] seatingCharcters = null;
    [SerializeField] GameObject[] standingCharcters = null;
    [SerializeField] Transform seatingParent = null;
    [SerializeField] Transform standingParent = null;
    [SerializeField] Transform textParent = null;
    [SerializeField] float setingRate = 0.3f;
    [SerializeField] float standingRate = 0.3f;

    int trainIndex;
    Sc6Train levelManager;
    public int TrainIndex
    {
        get { return trainIndex; }
        set
        {
            trainIndex = value;
            if (trainIndex >= 0 && trainIndex < Sc6Train.trainDestinations.Length)
            {
                var texts = textParent.GetComponentsInChildren<TextMeshPro>();
                foreach (var t in texts)
                {
                    t.text = Sc6Train.trainDestinations[trainIndex];
                }
            }
        }
    }
    void Start()
    {
        levelManager = FindObjectOfType<Sc6Train>();
        foreach(Transform t in seatingParent)
        {
            if(Random.value < setingRate)
            {
                Instantiate(seatingCharcters[Random.Range(0, seatingCharcters.Length)], t);
            }
        }
        foreach(Transform t in standingParent)
        {
            if(Random.value < standingRate)
            {
                Instantiate(standingCharcters[Random.Range(0, standingCharcters.Length)], t)
                    .transform.Rotate(Vector3.up, Random.Range(0, 360));
            }
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            StartCoroutine(levelManager.TrainTrigger(true, trainIndex));
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            StartCoroutine(levelManager.TrainTrigger(false, trainIndex));
        }
    }
}
