using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisionCall : MonoBehaviour
{
    public GameObject IncomingCall;
    public GameObject Calling;
    public GameObject SceneScript;
    [SerializeField] AudioSource IncomingCallAudio = null;
    [SerializeField] AudioSource CallingAudio = null;



    void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.tag == "RightHand")
        {
            IncomingCallAudio.Stop();
            IncomingCall.SetActive(false);
            Calling.SetActive(true);
            CallingAudio.Play();
           // SceneScript.GetComponent<Sc5Street>().ReceiveCall();
            Debug.Log("Incoming Call button pressed");



        }
    }
}
