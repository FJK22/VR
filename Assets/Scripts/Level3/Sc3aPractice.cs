using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.UI;
using TMPro;

public class Sc3aPractice : MonoBehaviour
{
    public GameObject ThisGameObject;
    public Button StartPracticeBTNl;
    public GameObject buttonStartPractice;
    bool praticeButtonIsClicked = false;
    public Canvas PracticeCanvas;
    public Text CanvasText;
    public GameObject EEG;
    public GameObject Hand;
    public Camera camera;
    

    [SerializeField] Transform[] SpawnPoses = null;
    [SerializeField] GameObject[] SpawnPrefabs = null;
    [SerializeField] float CarShowTime = 0.5f;
    [SerializeField] float CarSpeed = 55f;
    [SerializeField] float Delay = 1f;
    [SerializeField] int TotalCount = 10;
    List<int> mylist = new List<int>();


    int SpawnPosIndex;
    int count;
    int count2 = 0;

    bool isPressed;
    int PressCorrect1 = 0;
    int PressCorrect2 = 0;

    public GameObject Pointer;

    [Space]
    [Header("VR Touchpad")]
    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Vector2 touchPadAction = SteamVR_Input.GetAction<SteamVR_Action_Vector2>("TouchpadLeftRight");
    public SteamVR_Action_Boolean touchPadClick = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("TouchpadClick");


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

                mylist.Clear();
                mylist.Add(0);
                mylist.Add(1);
                mylist.Add(0);
                mylist.Add(1);
                mylist.Add(0);
                mylist.Add(1);
                mylist.Add(0);
                mylist.Add(1);
                mylist.Add(0);
                mylist.Add(1);

                StartPractice();
                Pointer.SetActive(false);
                buttonStartPractice.SetActive(false);
            }
        }

        Vector2 touchpadValue = touchPadAction.GetAxis(handType);
        bool touchpadClicked = touchPadClick.GetStateDown(handType);

        if (SpawnPosIndex == 0) //Left
        {
            if (touchpadValue.x < 0 && touchpadClicked)
            {
                //Debug.Log("Pressed Left");
                PressCorrect1++;

            }
            if (touchpadValue.x > 0 && touchpadClicked)
            {
                //Debug.Log("Pressed Right");
                PressCorrect1 = PressCorrect1 + 0;
            }
        }
        if (SpawnPosIndex == 1) //Right
        {
            if (touchpadValue.x < 0 && touchpadClicked)
            {
                //Debug.Log("Pressed Left");
                PressCorrect2 = PressCorrect2 + 0;

            }
            if (touchpadValue.x > 0 && touchpadClicked)
            {
                //Debug.Log("Pressed Right");
                PressCorrect2++;
            }
        }

        if(PressCorrect1 >= 2 && PressCorrect2 >= 2)
        {
            StartCoroutine(PracticeCompleted());
           
        }

    }
    void buttonIsClicked()
    {
        praticeButtonIsClicked = true;
        PracticeCanvas.enabled = false;

    }

    void StartPractice()
    {
        
        StartCoroutine(ShowCar());

    }

    IEnumerator ShowCar()
    {

        yield return new WaitForSeconds(2);
        SpawnPosIndex = mylist[Random.Range(0, mylist.Count)];
        int _carIndex = Random.Range(0, SpawnPrefabs.Length);
        Instantiate(SpawnPrefabs[_carIndex], SpawnPoses[SpawnPosIndex]).AddComponent<AutoCar>().Set(CarShowTime, CarSpeed);
        mylist.Remove(SpawnPosIndex);
        yield return new WaitForSeconds(CarShowTime + Delay);
        count++;

        if (count < TotalCount)
        {

            StartCoroutine(ShowCar());

        }
        else if ((PressCorrect1 <= 1 && PressCorrect2 <= 1) || (PressCorrect1 >= 1 && PressCorrect2 <= 1) || (PressCorrect1 <= 1 && PressCorrect2 >= 1))
        {
            StartCoroutine(StartAgain());

        }
        

    }
    IEnumerator StartAgain()
    {
        PracticeCanvas.enabled = true;
        CanvasText.text = "Please start again. Make sure you press the corresponding correct direction in the rounded button of the controller depending where the car is shown.";
        buttonStartPractice.SetActive(true);
        praticeButtonIsClicked = false;
        Pointer.SetActive(true);
        PressCorrect1 = 0;
        PressCorrect2 = 0;
        count2 = 0;
        count = 0;
        yield return new WaitForSeconds(1f);



    }


    IEnumerator PracticeCompleted()
    {
        
        SpawnPoses = null;
        SpawnPrefabs = null;
        PracticeCanvas.enabled = true;
        CanvasText.text = "Practice completed. You will now start the calibration process.";
        yield return new WaitForSeconds(5f);
        EEG.SetActive(true);
        ThisGameObject.gameObject.SetActive(false);
        Hand.SetActive(false);
        camera.clearFlags = CameraClearFlags.SolidColor;
    }
}
