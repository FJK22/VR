using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Valve.VR;
using UnityEngine.UI;
using PupilLabs;


public class Sc5Street : LevelScript
{
   
    [SerializeField] float MaxLimitTime = 600f;
    [SerializeField] GameObject phone = null;
    //[SerializeField] GameObject mapCanvas = null;
    [SerializeField] GameObject mapPan = null;
    [SerializeField] GameObject missedCallPan = null;
    [SerializeField] GameObject callingPan = null;
    [SerializeField] GameObject messagePan = null;
    [SerializeField] AudioSource missedSound = null;
    [SerializeField] AudioSource callIncomeSound = null;
    [SerializeField] AudioSource messageSound = null;
    [SerializeField] float missedCallDelay = 3;
    [SerializeField] float messageDelay = 3;
    [SerializeField] float mapDelay = 4;

    public static Sc5Street Instance;
    
    public GameObject[] CarPrefabs = null;
    int mapOpenCount = 0;
    public float startTime = 0;
    bool isMapOpened = false;
    int marks = 10;
    int currentPointIndex = 0;
    public GameObject VRController;
    public GameObject Pointer;
    public Button btn;

    [Space]
    [Header("VR Trigger")]
    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Boolean grabPinchAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabPinch");

    [Space]
    [Header("IncomingCall")]
    public GameObject IncomingCall;
    public GameObject Calling;
    [SerializeField] AudioSource IncomingCallAudio = null;
    [SerializeField] AudioSource CallingAudio = null;

    private bool callingPanBool = true;

    [Space]
    [Header("Eye Tracker")]
    public RecordingController recorder;
    public Text statusText;
    public Camera camera;

    void Awake()
    {
        Pointer.SetActive(true);
        Instance = this;
        string date = System.DateTime.Now.ToString("yyyy_MM_dd");
        recorder.customPath = $"{Application.dataPath}/Data/{UserGroup}/{UserName + "_" + date}/Sc7StreetPedestrian/EyeTracking";
        camera.clearFlags = CameraClearFlags.Skybox;
    }

    void OnDestroy()
    {
        recorder.StopRecording();
    }

    void Start()
    {
        
        btn.onClick.AddListener(ButtonOnClick);
    }
    void ButtonOnClick()
    {
        StartTask();
    }
    new public void StartTask()
    {

        base.StartTask();
        EEG.Instance.Init("Sc7StreetPedestrian");
        VRController.GetComponent<VRController>().enabled = true;
        recorder.StartRecording();
        TaskCanvas.GetComponent<Canvas>().enabled = false;
        TaskCanvas.GetComponent<GraphicRaycaster>().enabled = false;
        Pointer.SetActive(false);
        StartCoroutine(LimitTimer());
    }

    void Update()
    {


        if (isStarted && !missedCallPan.activeSelf && !callingPan.activeSelf && !messagePan.activeSelf && grabPinchAction.GetStateDown(handType))
        {
            MapOpen();
        }

        if (callingPan.activeSelf)
        {
            StartCall();
        }



    }

    void StartCall()
    {
         if (callingPanBool && grabPinchAction.GetStateDown(handType))
         {
             IncomingCallAudio.Stop();
             IncomingCall.SetActive(false);
             Calling.SetActive(true);
             CallingAudio.Play();
             callingPanBool = false;
             ReceiveCall();
         }
    }

  

   
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            StartCoroutine(Post());
        }
    }
    private void OnEventTrigger(int index)
    {
        mapPan.SetActive(false);
        
        switch (index)
        {
            case 0:
                StartCoroutine(MissedCall());
                break;
            case 1:
                phone.SetActive(true);
                callingPan.SetActive(true);
                callIncomeSound.Play();
                break;
            case 2:
                StartCoroutine(Message());
                break;
        }
    }
    private void PathPass(int index)
    {
        if(index < 0)
        {
            marks += index;
        }
        else if(currentPointIndex > index)
        {
            marks += index - currentPointIndex;
        }
        else
        {
            currentPointIndex = index;
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
    public void ReceiveCall()
    {
        callingPanBool = false;
        StartCoroutine(AudioCalling());
    }
    IEnumerator AudioCalling() {
        yield return new WaitForSeconds(6.5f);
        callingPan.SetActive(false);
        phone.SetActive(false);

    }
    IEnumerator Post()
    {
        string accuracy = "High";
        if (marks < 8) accuracy = "Medium";
        if (marks < 5) accuracy = "Low";
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("username", UserName));
        formData.Add(new MultipartFormDataSection("reaction_time", ((Time.time - startTime) * 1000).ToString("0.0")));
        formData.Add(new MultipartFormDataSection("map_pressed", mapOpenCount.ToString()));
        formData.Add(new MultipartFormDataSection("accuracy", accuracy));
        UnityWebRequest www = UnityWebRequest.Post(Constant.DOMAIN + Constant.SC5Data, formData);
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
        }
        recorder.StopRecording();
        StartCoroutine(SetLevel(SceneType.Sc5Questionnaire));
        NextScene();
    }
    IEnumerator MapClose()
    {
        yield return new WaitForSeconds(mapDelay);
        isMapOpened = false;
        phone.SetActive(false);
        mapPan.SetActive(false);
    }
    IEnumerator MissedCall()
    {
        missedSound.Play();
        phone.SetActive(true);
        missedCallPan.SetActive(true);
        yield return new WaitForSeconds(missedCallDelay);
        phone.SetActive(false);
        missedCallPan.SetActive(false);
    }
    IEnumerator Message()
    {
        messageSound.Play();
        phone.SetActive(true);
        messagePan.SetActive(true);
        yield return new WaitForSeconds(messageDelay);
        messagePan.SetActive(false);
        phone.SetActive(false);
    }
    public IEnumerator LimitTimer()
    {
        startTime = Time.time;
        yield return new WaitForSeconds(MaxLimitTime);
        marks = 0;
        StartCoroutine(Post());
    }
}
