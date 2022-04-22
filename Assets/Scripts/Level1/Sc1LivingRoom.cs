using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using Valve.VR;
using PupilLabs;
using UnityEngine.UI;
using Looxid.Link;
using System.IO;
using System.Text;
using UnityEngine.Networking;
using SimpleJSON;
using System;

public class Sc1LivingRoom : LevelScript
{
    [SerializeField] VideoPlayer video = null;
    [SerializeField] AudioSource[] audios = null;
    public GameObject Pointer;
    string looked;


    [Space]
    [Header("Eye Tracker")]
    public RecordingController recorder;
    public Text statusText;
    public GazeVisualizer gazeVisualizer;
    public GazeData gazeData;
    public Transform gazeOriginCamera;
    public GazeController gazeController;
    TimeSync timeSync;


    void Awake()
    {
        string date = System.DateTime.Now.ToString("yyyy_MM_dd");
        recorder.customPath = $"{Application.dataPath}/Data/{UserGroup}/{UserName + "_" + date}/Sc1LivingRoom/EyeTracking";
        bool connected = recorder.requestCtrl.IsConnected;
    }

    private void OnEnable()
    {
        if (gazeController)
        {
            gazeController.OnReceive3dGaze += OnReceive;
        }
    }
    
   
    private void OnReceive(GazeData obj)
    {
        gazeData = obj;
    }

    void OnDestroy()
    {
        recorder.StopRecording();
    }

    void Update()
    {

        if (gazeData != null)
        {
            Vector3 origin = gazeOriginCamera.position;
            Vector3 direction = gazeOriginCamera.TransformDirection(gazeData.GazeDirection);

            if (Physics.SphereCast(origin, 0.05f, direction, out RaycastHit hit, Mathf.Infinity))
            {
                if (hit.collider.tag == "TV")
                {
                    looked = "TV";
                    LookedAt(looked);

                }
                else
                {
                    looked = "Else";
                    LookedAt(looked);

                }


            }
        }

        StartBTN.onClick.AddListener(buttonIsClicked);
        

        if (!isStarted && btnIsClicked)
        {
            StartTask();
            recorder.StartRecording(); 
            Pointer.SetActive(false);

            

        }


        if (isStarted && video.isPaused)
        {
            isStarted = false; 
            btnIsClicked = false;
            StartCoroutine(EndTask()); 
        }
    }


    new public void StartTask()
    {
        
        base.StartTask();
        EEG.Instance.Init("Sc1LivingRoom");
        video.Play();
        foreach (var a in audios)
            a.Play();
        
        
    }

    void buttonIsClicked()
    {
        btnIsClicked = true;
    }

    void LookedAt(string lookedAtWhat)
    {
        string date = System.DateTime.Now.ToString("yyyy_MM_dd");
        string path = $"{Application.dataPath}/Data/{UserGroup}/{UserName + "_" + date}/Sc1LivingRoom/EyeTracking/" + UserName + "_" + "LookedAt.csv";

        double pupilTime = timeSync.GetPupilTimestamp();
        double unityTime = Time.realtimeSinceStartup;

        if (!File.Exists(path))
        {
            string header = "Pupil Timestamp,Unity Time,Looked At" + Environment.NewLine;

            File.AppendAllText(path, header);
        }

        string values = $"{pupilTime}, {unityTime}, {lookedAtWhat}" + Environment.NewLine;

        File.AppendAllText(path, values);
    }

    IEnumerator EndTask()
    {
        recorder.StopRecording();
        StartCoroutine(SetLevel(SceneType.Sc1Questionnaire)); 
        yield return new WaitForSeconds(1);
        NextScene();
    }
}
