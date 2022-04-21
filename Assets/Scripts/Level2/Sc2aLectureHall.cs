using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using Valve.VR;
using UnityEngine.UI;
using PupilLabs;
using UnityEngine.SceneManagement;


public class Sc2aLectureHall : LevelScript
{
    [SerializeField] TextMeshPro text = null;
    [SerializeField] float delay = 1.15f;
    [SerializeField] bool isReverse = false;
    int count = 0;
    int currentNumber = 0;
    bool posted = false;
    float startTime = 0f;
    public Camera camera;
    List<int> mylist = new List<int>();


    [Space]
    [Header("VR Trigger")]
    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Boolean grabPinchAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabPinch");
    public GameObject Pointer;

    [Space]
    [Header("Eye Tracker")]
    public RecordingController recorder;
    public Text statusText;

    void Awake()
    {
        
        camera.clearFlags = CameraClearFlags.Skybox;
        Pointer.SetActive(true);

        Scene scene = SceneManager.GetActiveScene();

        string date = System.DateTime.Now.ToString("yyyy_MM_dd");

        recorder.customPath = $"{Application.dataPath}/Data/{UserGroup}/{UserName + "_" + date}/Sc2LectureHall/EyeTracking";

        bool connected = recorder.requestCtrl.IsConnected;
        
        mylist.Add(1);
        mylist.Add(2);
        mylist.Add(3);
        mylist.Add(4);
        mylist.Add(5);
        mylist.Add(6);
        mylist.Add(7);
        mylist.Add(8);
        mylist.Add(9);
        mylist.Add(1);
        mylist.Add(2);
        mylist.Add(3);
        mylist.Add(4);
        mylist.Add(5);
        mylist.Add(6);
        mylist.Add(7);
        mylist.Add(8);
        mylist.Add(9);
        mylist.Add(1);
        mylist.Add(2);
        mylist.Add(3);
        mylist.Add(4);
        mylist.Add(5);
        mylist.Add(6);
        mylist.Add(7);
        mylist.Add(8);
        mylist.Add(9);
        mylist.Add(1);
        mylist.Add(2);
        mylist.Add(3);
        mylist.Add(4);
        mylist.Add(5);
        mylist.Add(6);
        mylist.Add(7);
        mylist.Add(8);
        mylist.Add(9);
        mylist.Add(1);
        mylist.Add(2);
        mylist.Add(3);
        mylist.Add(4);
        mylist.Add(5);
        mylist.Add(6);
        mylist.Add(7);
        mylist.Add(8);
        mylist.Add(9);
        mylist.Add(1);
        mylist.Add(2);
        mylist.Add(3);
        mylist.Add(4);
        mylist.Add(5);
        mylist.Add(6);
        mylist.Add(7);
        mylist.Add(8);
        mylist.Add(9);
        mylist.Add(1);
        mylist.Add(2);
        mylist.Add(3);
        mylist.Add(4);
        mylist.Add(5);
        mylist.Add(6);
        mylist.Add(7);
        mylist.Add(8);
        mylist.Add(9);
        mylist.Add(1);
        mylist.Add(2);
        mylist.Add(3);
        mylist.Add(4);
        mylist.Add(5);
        mylist.Add(6);
        mylist.Add(7);
        mylist.Add(8);
        mylist.Add(9);
        mylist.Add(1);
        mylist.Add(2);
        mylist.Add(3);
        mylist.Add(4);
        mylist.Add(5);
        mylist.Add(6);
        mylist.Add(7);
        mylist.Add(8);
        mylist.Add(9);
        mylist.Add(1);
        mylist.Add(2);
        mylist.Add(3);
        mylist.Add(4);
        mylist.Add(5);
        mylist.Add(6);
        mylist.Add(7);
        mylist.Add(8);
        mylist.Add(9);
        mylist.Add(1);
        mylist.Add(2);
        mylist.Add(3);
        mylist.Add(4);
        mylist.Add(5);
        mylist.Add(6);
        mylist.Add(7);
        mylist.Add(8);
        mylist.Add(9);
        mylist.Add(1);
        mylist.Add(2);
        mylist.Add(3);
        mylist.Add(4);
        mylist.Add(5);
        mylist.Add(6);
        mylist.Add(7);
        mylist.Add(8);
        mylist.Add(9);
        mylist.Add(1);
        mylist.Add(2);
        mylist.Add(3);
        mylist.Add(4);
        mylist.Add(5);
        mylist.Add(6);
        mylist.Add(7);
        mylist.Add(8);
        mylist.Add(9);
        mylist.Add(1);
        mylist.Add(2);
        mylist.Add(3);
        mylist.Add(4);
        mylist.Add(5);
        mylist.Add(6);
        mylist.Add(7);
        mylist.Add(8);
        mylist.Add(9);
        mylist.Add(1);
        mylist.Add(2);
        mylist.Add(3);
        mylist.Add(4);
        mylist.Add(5);
        mylist.Add(6);
        mylist.Add(7);
        mylist.Add(8);
        mylist.Add(9);
        mylist.Add(1);
        mylist.Add(2);
        mylist.Add(3);
        mylist.Add(4);
        mylist.Add(5);
        mylist.Add(6);
        mylist.Add(7);
        mylist.Add(8);
        mylist.Add(9);
        mylist.Add(1);
        mylist.Add(2);
        mylist.Add(3);
        mylist.Add(4);
        mylist.Add(5);
        mylist.Add(6);
        mylist.Add(7);
        mylist.Add(8);
        mylist.Add(9);
        mylist.Add(1);
        mylist.Add(2);
        mylist.Add(3);
        mylist.Add(4);
        mylist.Add(5);
        mylist.Add(6);
        mylist.Add(7);
        mylist.Add(8);
        mylist.Add(9);
        mylist.Add(1);
        mylist.Add(2);
        mylist.Add(3);
        mylist.Add(4);
        mylist.Add(5);
        mylist.Add(6);
        mylist.Add(7);
        mylist.Add(8);
        mylist.Add(9);
        mylist.Add(1);
        mylist.Add(2);
        mylist.Add(3);
        mylist.Add(4);
        mylist.Add(5);
        mylist.Add(6);
        mylist.Add(7);
        mylist.Add(8);
        mylist.Add(9);
        mylist.Add(1);
        mylist.Add(2);
        mylist.Add(3);
        mylist.Add(4);
        mylist.Add(5);
        mylist.Add(6);
        mylist.Add(7);
        mylist.Add(8);
        mylist.Add(9);
        mylist.Add(1);
        mylist.Add(2);
        mylist.Add(3);
        mylist.Add(4);
        mylist.Add(5);
        mylist.Add(6);
        mylist.Add(7);
        mylist.Add(8);
        mylist.Add(9);
        mylist.Add(1);
        mylist.Add(2);
        mylist.Add(3);
        mylist.Add(4);
        mylist.Add(5);
        mylist.Add(6);
        mylist.Add(7);
        mylist.Add(8);
        mylist.Add(9);
        mylist.Add(1);
        mylist.Add(2);
        mylist.Add(3);
        mylist.Add(4);
        mylist.Add(5);
        mylist.Add(6);
        mylist.Add(7);
        mylist.Add(8);
        mylist.Add(9);
        mylist.Add(1);
        mylist.Add(2);
        mylist.Add(3);
        mylist.Add(4);
        mylist.Add(5);
        mylist.Add(6);
        mylist.Add(7);
        mylist.Add(8);
        mylist.Add(9);








    }

    void OnDestroy()
    {
        recorder.StopRecording();
    }
    void Update()
    {
        if (btnIsClicked && !isStarted)
        {
            StartTask();
            recorder.StartRecording();
            Pointer.SetActive(false);

            
        }

        if (!posted)
        {
            if (grabPinchAction.GetStateDown(handType))
            {
                StartCoroutine(Post(true));
            }
        }

        
    }

    new public void StartTask()
    {
        base.StartTask();
        StartCoroutine(ClearData("sc2_data"));
        StartCoroutine(ShowNumber(true));
        EEG.Instance.Init("Sc2LectureHall");

    }

    IEnumerator Post(bool pressed)
    {

        posted = true;
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("username", UserName));
        formData.Add(new MultipartFormDataSection("digit", currentNumber.ToString()));
        formData.Add(new MultipartFormDataSection("spacebar_trigger_pressed", (pressed) ? "YES" : "NO"));
        if (pressed)
        {
            formData.Add(new MultipartFormDataSection("accuracy", (currentNumber == 3) ? "Correct" : "Wrong"));
            formData.Add(new MultipartFormDataSection("reaction_time", ((Time.time - startTime) * 1000).ToString("0.0")));
        }
        UnityWebRequest www = UnityWebRequest.Post(Constant.DOMAIN + (Constant.SC2Data), formData);
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
        }
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
            int newNumber = mylist[Random.Range(0, mylist.Count)];

            if (newNumber != currentNumber)
            {
                currentNumber = newNumber;
                break;


            }

        }

        text.text = currentNumber.ToString();
        mylist.Remove(currentNumber);
        startTime = Time.time;
        yield return new WaitForSeconds(delay);
        posted = false;
        count++;

        if (count < 225)
        {
            StartCoroutine(ShowNumber());
        }
        else
        {
            recorder.StopRecording();
            StartCoroutine(SetLevel(SceneType.Sc2aQuestionnaire));
            yield return new WaitForSeconds(2f);
            NextScene();
        }



    }
}

