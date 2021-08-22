using System.Collections;
using UnityEngine;

public class Sc5Street : LevelScript
{
    void Update()
    {
        if (!isStarted && Input.GetKey(KeyCode.Space))
        {
            StartTask();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            NextScene();
        }
    }
}
