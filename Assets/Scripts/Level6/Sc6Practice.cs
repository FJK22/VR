using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Networking;
using System.Collections.Generic;
using Valve.VR;
using UnityEngine.UI;
using PupilLabs;


public class Sc6Practice : MonoBehaviour
{

    public Button StartPracticeBTNl;
    public GameObject buttonStartPractice;
    bool praticeButtonIsClicked = false;
    public Canvas PracticeCanvas;
    public GameObject Canvas;
    public GameObject StatusText;
    public Text CanvasText;
    public GameObject EEG;
    public GameObject Hand;
    public Camera camera;
    public GameObject VRController;
    public GameObject Pointer;
    public Transform cameraTransform;

    int count = 0;
    bool timer = false;
    float distanceFromCamera = 3;
    float MaxLimitTime = 15;
    [Space]
    [Header("VR Trigger")]
    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Boolean grabPinchAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabPinch");
    public SteamVR_Action_Vector2 touchPadAction = SteamVR_Input.GetAction<SteamVR_Action_Vector2>("TouchpadLeftRight");
    public SteamVR_Action_Boolean touchPadClick = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("TouchpadClick");


    void Start()
    {

        Pointer.SetActive(true);
     
    }

   
   
    void Update()
    {
        StartPracticeBTNl.onClick.AddListener(buttonIsClicked);

        if (praticeButtonIsClicked)
        {
            Canvas.SetActive(false);
            VRController.GetComponent<VRController>().enabled = true;
            Pointer.SetActive(false);

            Vector2 touchpadValue = touchPadAction.GetAxis(handType);
            bool touchpadClicked = touchPadClick.GetStateDown(handType);
           
            if ((touchpadValue.y > 0.5 || touchpadValue.y < 0.5) && touchpadClicked)
            {
                count++;
                
                if (count == 1) {

                    
                    Timer();

                   

                }
             


            }

            if (timer == true && MaxLimitTime > 0)
            {
                MaxLimitTime -= Time.deltaTime;
                
            }
            if (MaxLimitTime <= 0)
            {
                
                StartCoroutine(PracticeCompleted());
            }


        }

        


    }

    void Timer()
    {
        timer = true;
        
    }

    void buttonIsClicked()
    {
        praticeButtonIsClicked = true;
        


    }

    IEnumerator PracticeCompleted()
    {
        buttonStartPractice.SetActive(false);
        VRController.GetComponent<VRController>().enabled = false;
        Vector3 resultingPosition = cameraTransform.position + cameraTransform.forward * distanceFromCamera;
        PracticeCanvas.transform.position = resultingPosition;

        Canvas.SetActive(true);
        PracticeCanvas.enabled = true;

        CanvasText.text = "Practice completed. You will now start the calibration process.";

        yield return new WaitForSeconds(6);
        VRController.transform.position = new Vector3(1.277f, 1.91f, 22.266f);

        //VRController.transform.position = VRControllerInitialPos;
        //camera.transform.position = CameraInitialPos;
        EEG.SetActive(true);
        this.gameObject.SetActive(false);
        Hand.SetActive(false);
        camera.clearFlags = CameraClearFlags.SolidColor;




    }

   



}
