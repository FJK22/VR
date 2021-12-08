using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using Valve.VR;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Sc2Practice : MonoBehaviour
{
    [SerializeField] TextMeshPro text = null;
    [SerializeField] float delay = 1.15f;
    int count = 0;
    int currentNumber = 0;
    int countPress = 0;
    public Button StartPracticeBTNl;
    bool praticeButtonIsClicked = false;
    public Canvas PracticeCanvas;
    public Text CanvasText;
    public GameObject EEG;
    public GameObject Hand;
    public GameObject buttonStartPractice;
    public Camera camera;
    int count2 = 0;


    [Space]
    [Header("VR Trigger")]
    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Boolean grabPinchAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabPinch");
    public GameObject Pointer;

   


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

            if (count2 == 1) {

                StartPractice();
                Pointer.SetActive(false);
                buttonStartPractice.SetActive(false);

                
                
            }
            if (currentNumber == 3 && grabPinchAction.GetStateDown(handType))
            {
                countPress++;


                if (countPress == 2)
                {
                    StartCoroutine(PracticeCompleted());
                    text.enabled = false;


                }



            }

        }

      

        

    }

    void buttonIsClicked()
    {
        praticeButtonIsClicked = true;
       


    }

    void StartPractice()
    {
        text.enabled = true;
        StartCoroutine(ShowNumber());

    }



    IEnumerator ShowNumber(bool _startDelay = false)
    {
        if (_startDelay)
        {
            yield return new WaitForSeconds(3);
        }
        // this is for remove repeat
        while (true)
        {
            int newNumber = Random.Range(1, 10);
            if (newNumber != currentNumber)
            {
                currentNumber = newNumber;
                break;
            }
            
        }
        text.text = currentNumber.ToString();

        yield return new WaitForSeconds(delay);
     
        count++;
        if (count < 25)
        {
            StartCoroutine(ShowNumber());

        }
        else if (countPress <= 1 && count == 25)
        {

            StartCoroutine(StartAgain());
        }
    }

    IEnumerator StartAgain()
    {
        
        CanvasText.text = "Please start again. Make sure you press the trigger button of the controller when digit 3 is shown.";
        buttonStartPractice.SetActive(true);
        praticeButtonIsClicked = false;
        Pointer.SetActive(true);
        countPress = 0;
        count2 = 0;
        count = 0;
        text.enabled = false;
        yield return new WaitForSeconds(1f);
        //StartCoroutine(ShowNumber(false));

        
    }

    IEnumerator PracticeCompleted()
    {
        
        CanvasText.text = "Practice completed. You will now start the calibration process.";
        yield return new WaitForSeconds(5f);
        EEG.SetActive(true);
        this.gameObject.SetActive(false);
        Hand.SetActive(false);
        camera.clearFlags = CameraClearFlags.SolidColor;
    }
}
