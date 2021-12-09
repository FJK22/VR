using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using UnitySimpleLiquid;

public class Sc8Practice : MonoBehaviour
{

    [SerializeField] Text textInstruction = null;
    public Button StartPracticeBTNl;
    public Button StartInstructionBTNl;
    bool praticeButtonIsClicked = false;
    public Canvas PracticeCanvas;
    public Text CanvasText;
    public GameObject EEG;
    public GameObject Hand;
    public GameObject buttonStartPractice;
    int count2 = 0;
    public GameObject emptyBeaker;
    public GameObject liquidBeaker;

    [Space]
    [Header("VR Trigger")]
    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Boolean grabPinchAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabPinch");
    public GameObject Pointer;

    void Start()
    {
        Pointer.SetActive(true);
        StartInstructionBTNl.interactable = false;
        emptyBeaker.SetActive(false);
        liquidBeaker.SetActive(false);
    }

   
    void Update()
    {
        StartPracticeBTNl.onClick.AddListener(buttonIsClicked);
        StartInstructionBTNl.onClick.AddListener(instructionButtonIsClicked);

        if (praticeButtonIsClicked)
        {
            count2++;

            if (count2 == 1)
            {
                //Pointer.SetActive(false);
                buttonStartPractice.SetActive(false);
                StartInstructionBTNl.interactable = true;
                PracticeCanvas.enabled = false;
            }
        }
        if (emptyBeaker.GetComponent<LiquidContainer>().fillAmountPercent > 0.5f)
        {
            textInstruction.text = "Well done. This is how we can interact with the equipment.";
            StartCoroutine(PracticeCompleted());
        }
    }

    void buttonIsClicked()
    {
        praticeButtonIsClicked = true;



    }
    public void instructionButtonIsClicked()
    {
        StartInstructionBTNl.interactable = false;
        textInstruction.text = "Grab the beaker with liquid by holding the trigger button and pour the liquid into the other beaker.";
        emptyBeaker.SetActive(true);
        liquidBeaker.SetActive(true);
    }

    IEnumerator PracticeCompleted()
    {
        PracticeCanvas.enabled = true;
        CanvasText.text = "Practice completed. You will now start the calibration process.";
        yield return new WaitForSeconds(10f);
        Destroy(liquidBeaker);
        Destroy(emptyBeaker);
        EEG.SetActive(true);
        this.gameObject.SetActive(false);
        Hand.SetActive(false);
       
    }
}
