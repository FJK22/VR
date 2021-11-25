using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Valve.VR;
using UnityEngine.UI;

public class Sc5Practice : MonoBehaviour
{
    public GameObject ThisGameObject;
    public Button StartPracticeBTNl;
    public GameObject buttonStartPractice;
    bool praticeButtonIsClicked = false;
    public Canvas PracticeCanvas;
    public Text CanvasText;
    public GameObject GazeTracker;
    public GameObject Recorder;
    public GameObject Hand;
    public Camera camera;
    public GameObject VRController;
    public GameObject Pointer;
    int count2 = 0;
    bool playerArrived = false;


    [SerializeField] GameObject phone = null;
    [SerializeField] GameObject mapPan = null;
    [SerializeField] float mapDelay = 3;

    public GameObject[] CarPrefabs = null;
    int mapOpenCount = 0;
    bool isMapOpened = false;
    int currentPointIndex = 0;
    


    [Space]
    [Header("VR Trigger")]
    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Boolean grabPinchAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabPinch");

    void Start()
    {
        Pointer.SetActive(true);
        
    }

    // Update is called once per frame
    void Update()
    {
        StartPracticeBTNl.onClick.AddListener(buttonIsClicked);

        if (praticeButtonIsClicked)
        {
            count2++;

            if (count2 == 1)
            {

                Pointer.SetActive(false);
                VRController.GetComponent<VRController>().enabled = true;
                buttonStartPractice.SetActive(false);

            }
        }

        if (praticeButtonIsClicked && grabPinchAction.GetStateDown(handType))
        {
            MapOpen();
        }
        if (mapOpenCount >= 1 && playerArrived)
        {
            StartCoroutine(PracticeCompleted());
        }
        else
        {
            
            StartCoroutine(StartAgain());
        }

    }

    void buttonIsClicked()
    {
        praticeButtonIsClicked = true;
        PracticeCanvas.enabled = false;

       

    }

    private void OnEventTrigger(int index)
    {
        mapPan.SetActive(false);

        switch (index)
        {
            case 0:
                playerArrived = true;
                break;

        }
    }
    public void MapOpen()
    {
        if (isMapOpened) return;
        isMapOpened = true;
        mapOpenCount++;

        phone.SetActive(true);
        mapPan.SetActive(true);
        StartCoroutine(MapClose());


    }

    IEnumerator MapClose()
    {
        yield return new WaitForSeconds(mapDelay);
        isMapOpened = false;
        phone.SetActive(false);
        mapPan.SetActive(false);
    }

    IEnumerator PracticeCompleted()
    {
        //transform.LookAt(transform.position + camera.transform.rotation * Vector3.back, camera.transform.rotation * Vector3.up);
        PracticeCanvas.enabled = true;
        CanvasText.text = "Practice completed. You will now start the calibration process.";
        yield return new WaitForSeconds(5f);
        GazeTracker.SetActive(true);
        Recorder.SetActive(true);
        ThisGameObject.gameObject.SetActive(false);
        Hand.SetActive(false);
        camera.clearFlags = CameraClearFlags.SolidColor;
    }

    IEnumerator StartAgain()
    {


        //transform.LookAt(transform.position + cticeCanvas.enabled = true;
        CanvasText.text = "Make sure you press the trigger button of the controller at least once to open the map and cross the road.";
        yield return new WaitForSeconds(5f);
        PracticeCanvas.enabled = false;



    }

}
