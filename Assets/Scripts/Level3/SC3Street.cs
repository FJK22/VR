﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Valve.VR;
using UnityEngine.UI;
using PupilLabs;
using UnityEngine.SceneManagement;

public class SC3Street : LevelScript
{
    [SerializeField] Transform[] SpawnPoses = null;
    [SerializeField] GameObject[] SpawnPrefabs = null;
    [SerializeField] float CarShowTime = 1f;
    [SerializeField] float CarSpeed = 50f;
    [SerializeField] float Delay = 1f;
    [SerializeField] int TotalCount = 100;
    [SerializeField] bool Correspond = false;

    int SpawnPosIndex;
    int count;
    float startTime;
    bool isPressed;

    public GameObject Pointer;

    [Space]
    [Header("VR Touchpad")]
    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Vector2 touchPadAction = SteamVR_Input.GetAction<SteamVR_Action_Vector2>("TouchpadLeftRight");
    public SteamVR_Action_Boolean touchPadClick = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("TouchpadClick");

    [Space]
    [Header("Eye Tracker")]
    public RecordingController recorder;
    public Text statusText;
    public Camera camera;

    void Awake()
    {
        Pointer.SetActive(true);

        Scene scene = SceneManager.GetActiveScene();

        string date = System.DateTime.Now.ToString("yyyy_MM_dd");

        if (scene.name == "Sc3AStreet")
        {
            recorder.customPath = $"{Application.dataPath}/Data/{UserGroup}/{UserName + "_" + date}/Sc4Street/EyeTracking";
        }
        if (scene.name == "Sc3BStreet")
        {
            recorder.customPath = $"{Application.dataPath}/Data/{UserGroup}/{UserName + "_" + date}/Sc5Street/EyeTracking";
        }

        bool connected = recorder.requestCtrl.IsConnected;

        camera.clearFlags = CameraClearFlags.Skybox;
    }

    void OnDestroy()
    {
        recorder.StopRecording();
    }

    new public void StartTask()
    {
        base.StartTask();
        StartCoroutine(ClearData(Correspond ? "sc3a_data" : "sc3b_data"));
        StartCoroutine(ShowCar());

        if (SceneManager.GetActiveScene().name == "Sc3AStreet")
        {
            EEG.Instance.Init("Sc4Street");
        }
        if (SceneManager.GetActiveScene().name == "Sc3BStreet")
        {
            EEG.Instance.Init("Sc5Street");
        }
    }
    private void Update()
    {
        StartBTN.onClick.AddListener(buttonIsClicked);

        if (!isStarted && btnIsClicked)
        {

            StartTask();
            recorder.StartRecording();
            Pointer.SetActive(false);

        }

        Vector2 touchpadValue = touchPadAction.GetAxis(handType);
        bool touchpadClicked = touchPadClick.GetStateDown(handType);

        if (!isPressed)
        {

            if (touchpadValue.x < 0 && touchpadClicked)
            {
                //Debug.Log("Pressed Left");
                StartCoroutine(Post(true));
            }
            if (touchpadValue.x > 0 && touchpadClicked)
            {
                //Debug.Log("Pressed Right");
                StartCoroutine(Post(false));
            }

            
            //if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            //StartCoroutine(Post(true));
            //if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) 
            // StartCoroutine(Post(false));
        }
    }

    void buttonIsClicked()
    {
        btnIsClicked = true;
    }

    IEnumerator Post(bool IsLeft)
    {
        
        isPressed = true;
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("username", UserName));
        formData.Add(new MultipartFormDataSection("car_shown", (SpawnPosIndex == 0) ? "Left": "Right"));
        formData.Add(new MultipartFormDataSection("arrow_pressed", (IsLeft) ? "Left": "Right"));
        formData.Add(new MultipartFormDataSection("accuracy", (SpawnPosIndex == 0 == IsLeft == Correspond) ? "Correct": "Wrong"));
        formData.Add(new MultipartFormDataSection("reaction_time", ((Time.time - startTime) * 1000).ToString("0.0")));

        string url = Constant.DOMAIN + ((Correspond) ? Constant.SC3AData : Constant.SC3BData);
        //Debug.Log(url);
        UnityWebRequest www = UnityWebRequest.Post(url, formData);
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
        }
    }
    IEnumerator ShowCar()
    {
        isPressed = false;
        yield return new WaitForSeconds(2);
        SpawnPosIndex = Random.Range(0, 2);
        int _carIndex = Random.Range(0, SpawnPrefabs.Length);
        Instantiate(SpawnPrefabs[_carIndex], SpawnPoses[SpawnPosIndex]).AddComponent<AutoCar>().Set(CarShowTime, CarSpeed);
        startTime = Time.time;
        yield return new WaitForSeconds(CarShowTime + Delay);
        count++;
        if(count < TotalCount)
        {
            StartCoroutine(ShowCar());
        }
        else
        {
            recorder.StopRecording();
            StartCoroutine(SetLevel((Correspond) ? SceneType.Sc3BStreet : SceneType.Sc3Questionnaire));
            NextScene();
        }
    }
}