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
    int count3 = 0;
    int count4 = 0;
    int count5 = 0;
    public Transform cameraTransform;
    //Set it to whatever value you think is best
    float distanceFromCamera = 3;
    [SerializeField] GameObject phone = null;
    [SerializeField] GameObject mapPan = null;
    [SerializeField] float mapDelay = 2;
    float startTime = 0;
    public GameObject[] CarPrefabs = null;
    int mapOpenCount = 0;
    bool isMapOpened = false;
    [SerializeField] float MaxLimitTime = 50f;
    public Canvas smartphoneCanvas;

    [Space]
    [Header("VR Trigger")]
    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Boolean grabPinchAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabPinch");

  
    void Start()
    {
        Pointer.SetActive(true);
   

    }

 
    void Update()
    {
        StartPracticeBTNl.onClick.AddListener(buttonIsClicked);

       

        if (count5 == 1)
        {
            startTime = 0;
        }


        if (praticeButtonIsClicked)
        {
            count2++;

            startTime = Time.time;

            if (count2 == 1)
            {
                
                
               
                Pointer.SetActive(false);
                VRController.GetComponent<VRController>().enabled = true;
                buttonStartPractice.SetActive(false);
                PracticeCanvas.enabled = false;
               

            }
        }

        if (praticeButtonIsClicked && grabPinchAction.GetStateDown(handType))
        {
            MapOpen();
        }
        if (mapOpenCount >= 1 && count3 >= 1)
        {
            StartCoroutine(PracticeCompleted());
            Destroy(phone);
            smartphoneCanvas.GetComponent<Canvas>().enabled = false;
            smartphoneCanvas.GetComponent<CanvasScaler>().enabled = false;
            smartphoneCanvas.GetComponent<GraphicRaycaster>().enabled = false;
        }
        if (startTime >= MaxLimitTime)
        {
            count4++;
            if (count4 == 1) {
                StartCoroutine(LimitTimer());
            }
        }
        

    }

    void buttonIsClicked()
    {
        praticeButtonIsClicked = true;
        count5++;


    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
           
            count3++;
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
        Vector3 resultingPosition = cameraTransform.position + cameraTransform.forward * distanceFromCamera;
        PracticeCanvas.transform.position = resultingPosition;
        PracticeCanvas.enabled = true;
        VRController.GetComponent<VRController>().enabled = false;
        
        CanvasText.text = "Practice completed. You will now start the calibration process.";
        yield return new WaitForSeconds(6f);
        VRController.transform.position = new Vector3(-317.638f, 70.174f, 1.519f);
        GazeTracker.SetActive(true);
        Recorder.SetActive(true);
        ThisGameObject.gameObject.SetActive(false);
        Hand.SetActive(false);
        camera.clearFlags = CameraClearFlags.SolidColor;
    }

  

    public IEnumerator LimitTimer()
    {
        Vector3 resultingPosition = cameraTransform.position + cameraTransform.forward * distanceFromCamera;
        PracticeCanvas.transform.position = resultingPosition;
        PracticeCanvas.enabled = true;
        CanvasText.text = "Make sure you press the trigger button of the controller at least once to open the map and cross the road.";
        yield return new WaitForSeconds(5f);
        PracticeCanvas.enabled = false;
    }

}
