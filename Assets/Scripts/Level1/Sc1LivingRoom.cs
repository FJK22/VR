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
    
    int lookedtvcount = 0;
    float lookedtvtime = 0;

    int lookedelsecount = 0;
    float lookedelsetime = 0;

    private float nextActionTime = 0.0f;
    public float period = 0.1f;

    [Space]
    [Header("Eye Tracker")]
    public RecordingController recorder;
    public Text statusText;
    public GazeVisualizer gazeVisualizer;
    public GazeData gazeData;
    public Transform gazeOriginCamera;
    public GazeController gazeController;

    void Awake()
    {
        string date = System.DateTime.Now.ToString("yyyy_MM_dd");
        recorder.customPath = $"{Application.dataPath}/Data/{UserGroup}/{UserName + "_" + date}/Sc1LivingRoom/EyeTracking";
       
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

        if (btnIsClicked && gazeData != null) {

            Vector3 origin = gazeOriginCamera.position;
            Vector3 direction = gazeOriginCamera.TransformDirection(gazeData.GazeDirection);

            if (Physics.SphereCast(origin, 0.05f, direction, out RaycastHit hit, Mathf.Infinity))
            {

                if (hit.collider.CompareTag("TV"))
                {

                   
                    if (Time.time > nextActionTime)
                    {
                        nextActionTime += period;

                        lookedtvcount++;
                        lookedtvtime += Time.time;

                    }

                }

                else
                {
                    if (Time.time > nextActionTime)
                    {
                        nextActionTime += period;

                        lookedelsecount++;
                        lookedelsetime += Time.time;

                       
                    }

                    

                }

            }

        }






    }

    IEnumerator Post()
    {

        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("username", UserName));
        formData.Add(new MultipartFormDataSection("lookedtvcount", lookedtvcount.ToString()));
        formData.Add(new MultipartFormDataSection("lookedtvtime", lookedtvtime.ToString("0.0")));
        formData.Add(new MultipartFormDataSection("lookedelsecount", lookedelsecount.ToString()));
        formData.Add(new MultipartFormDataSection("lookedelsetime", lookedelsetime.ToString("0.0")));
       


        string url = Constant.DOMAIN + (Constant.SC1EyeTrackingData);

        
        UnityWebRequest www = UnityWebRequest.Post(url, formData);
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
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
        StartCoroutine(Post());
        StartCoroutine(SetLevel(SceneType.Sc1Questionnaire)); 
        yield return new WaitForSeconds(1);
        NextScene();

    }
}
