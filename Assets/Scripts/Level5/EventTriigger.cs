using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EventTriigger : MonoBehaviour
{
    [SerializeField] int EventIndex = 0;
    [SerializeField] string TriggerEventName = "OnEventTrigger";
    bool triggered = false;
    LevelScript levelManager;
    void Start()
    {
        levelManager = FindObjectOfType<LevelScript>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!triggered && other.tag == "Player")
        {
            levelManager.SendMessage(TriggerEventName, EventIndex);
            triggered = true;
        }
    }
}
