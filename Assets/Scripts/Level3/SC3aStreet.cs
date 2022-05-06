using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Valve.VR;
using UnityEngine.UI;
using PupilLabs;
using UnityEngine.SceneManagement;

public class SC3aStreet : LevelScript
{
    [SerializeField] Transform[] SpawnPoses = null;
    [SerializeField] GameObject[] SpawnPrefabs = null;
    [SerializeField] float CarShowTime = 1f;
    [SerializeField] float CarSpeed = 55f;
    [SerializeField] float Delay = 1f;
    [SerializeField] int TotalCount = 70;
    List<int> mylist = new List<int>();

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
    public GazeVisualizer gazeVisualizer;
    public GazeData gazeData;
    public Transform gazeOriginCamera;
    public GazeController gazeController;



   

    void Awake()
    {
        Pointer.SetActive(true);

        Scene scene = SceneManager.GetActiveScene();

        string date = System.DateTime.Now.ToString("yyyy_MM_dd");

        recorder.customPath = $"{Application.dataPath}/Data/{UserGroup}/{UserName + "_" + date}/Sc4Street/EyeTracking";

        bool connected = recorder.requestCtrl.IsConnected;

        camera.clearFlags = CameraClearFlags.Skybox;

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

    new public void StartTask()
    {
        base.StartTask();
        StartCoroutine(ClearData("sc3a_data"));
        StartCoroutine(ShowCar());

        EEG.Instance.Init("Sc4Street");

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
        formData.Add(new MultipartFormDataSection("accuracy", (SpawnPosIndex == 0 == IsLeft) ? "Correct": "Wrong"));
        formData.Add(new MultipartFormDataSection("reaction_time", ((Time.time - startTime) * 1000).ToString("0.0")));
        

        if (gazeData != null)
        {
            Vector3 origin = gazeOriginCamera.position;
            Vector3 direction = gazeOriginCamera.TransformDirection(gazeData.GazeDirection);

            if (Physics.SphereCast(origin, 0.05f, direction, out RaycastHit hit, Mathf.Infinity))
            {
                if (hit.collider.CompareTag("Left"))
                {
                    formData.Add(new MultipartFormDataSection("looked", "Left"));
                }
                else if (hit.collider.CompareTag("Right"))
                {
                    formData.Add(new MultipartFormDataSection("looked", "Right"));
                }
                else
                {
                    formData.Add(new MultipartFormDataSection("looked", "Else"));
                }

            }
        }

        string url = Constant.DOMAIN + (Constant.SC3AData);

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

        do
        {

            SpawnPosIndex = mylist[Random.Range(0, mylist.Count)];


        } while (mylist.Count > 0);

        
        int _carIndex = Random.Range(0, SpawnPrefabs.Length);
        Instantiate(SpawnPrefabs[_carIndex], SpawnPoses[SpawnPosIndex]).AddComponent<AutoCar>().Set(CarShowTime, CarSpeed);
        mylist.Remove(SpawnPosIndex);
        startTime = Time.time;
        yield return new WaitForSeconds(CarShowTime + Delay);
        count++;
        if (count < TotalCount)
        {
            StartCoroutine(ShowCar());
        }
        else
        {
            recorder.StopRecording();
            StartCoroutine(SetLevel(SceneType.Sc3aQuestionnaire));
            yield return new WaitForSeconds(2f);
            NextScene();
        }
    }
}