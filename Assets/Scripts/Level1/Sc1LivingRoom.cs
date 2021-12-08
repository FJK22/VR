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

public class Sc1LivingRoom : LevelScript
{
    [SerializeField] VideoPlayer video = null;
    [SerializeField] AudioSource[] audios = null;
    public GameObject Pointer;


    [Space]
    [Header("Eye Tracker")]
    public RecordingController recorder;
    public Text statusText;

    void Awake()
    {
        string date = System.DateTime.Now.ToString("yyyy_MM_dd");
        recorder.customPath = $"{Application.dataPath}/Data/{UserGroup}/{UserName + "_" + date}/Sc1LivingRoom/EyeTracking";
        bool connected = recorder.requestCtrl.IsConnected;
    }
    
    void OnDestroy()
    {
        recorder.StopRecording();
    }

    void Update()
    {
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

    
    IEnumerator EndTask()
    {
        recorder.StopRecording();
        StartCoroutine(SetLevel(SceneType.Sc1Questionnaire)); 
        yield return new WaitForSeconds(1);
        NextScene();
    }
}
