using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Looxid.Link;


public class Sc8ChemistryLab : LevelScript
{
    float startTime = 0;
    public Button startExperimentBTN;
    [SerializeField] int TimeLimit = 600;
    bool startExButtonClicked = false;
    public GameObject BurnerFlame;
    private int count;
    private float attentionAverage;
    bool burnFLameIsActive = false;
    EEGSensor sensorStatusData;
    private EEGRawSignal rawSignalData;
    public GameObject Visualizer;

    private LinkDataValue attention;



    void Start()
    {

        //LooxidLinkData.OnReceiveEEGSensorStatus += LooxidLinkData.OnReceiveEEGSensorStatus;
        //LooxidLinkData.OnReceiveEEGRawSignals += LooxidLinkData.OnReceiveEEGRawSignals;
        //LooxidLinkData.OnReceiveMindIndexes += LooxidLinkData.OnReceiveMindIndexes;

        //bool isIntialized = LooxidLinkManager.Instance.Initialize();
        //LooxidLinkManager.OnLinkCoreConnected += LooxidLinkManager.OnLinkCoreConnected;
        //LooxidLinkManager.OnLinkCoreDisconnected += LooxidLinkManager.OnLinkCoreDisconnected;
       // LooxidLinkManager.OnLinkHubConnected += LooxidLinkManager.OnLinkHubConnected;
        //LooxidLinkManager.OnLinkHubDisconnected += LooxidLinkManager.OnLinkHubDisconnected;

        LooxidLinkManager.Instance.SetDebug(true);
        LooxidLinkManager.Instance.Initialize();
        attention = new LinkDataValue();

      

        
        //sensorStatusData = new EEGSensor();
       

        startExperimentBTN.interactable = false;
    }
    void OnEnable()
    {
        LooxidLinkData.OnReceiveEEGSensorStatus += OnReceiveEEGSensorStatus;
        //LooxidLinkData.OnReceiveEEGRawSignals += OnReceiveEEGRawSignals;
        //LooxidLinkData.OnReceiveMindIndexes += OnReceiveMindIndexes;

        //StartCoroutine(DisplayData());
    }
    void OnDisable()
    {
        LooxidLinkData.OnReceiveEEGSensorStatus -= OnReceiveEEGSensorStatus;
        //LooxidLinkData.OnReceiveEEGRawSignals -= OnReceiveEEGRawSignals;
        //LooxidLinkData.OnReceiveMindIndexes -= OnReceiveMindIndexes;
    }
    void OnReceiveEEGSensorStatus(EEGSensor sensorStatusData)
    {
        this.sensorStatusData = sensorStatusData;
    }
    void OnReceiveMindIndexes(MindIndex mindIndexData)
    {
        attention.target = double.IsNaN(mindIndexData.attention) ? 0.0f : (float)LooxidLinkUtility.Scale(LooxidLink.MIND_INDEX_SCALE_MIN, LooxidLink.MIND_INDEX_SCALE_MAX, 0.0f, 1.0f, mindIndexData.attention);
    }

    void Update()
    {
        //att = LooxidLinkUtlity.Scale(LooxidLink.MIND_INDEX_SCALE_MIN, LooxidLink.MIND_INDEX_SCALE_MAX, 0.0, 1.0, mindIndex.attention);

        StartBTN.onClick.AddListener(buttonIsClicked);
        startExperimentBTN.onClick.AddListener(buttonExpIsClicked);
        attention.value = Mathf.Lerp((float)attention.value, (float)attention.target, 0.2f);
        attentionAverage = (float)(attentionAverage * count + attention.value) / (count + 1);
        count++;
        Debug.Log(attention.value + " && " + attentionAverage);

        if (!isStarted && btnIsClicked)
        {
            if (sensorStatusData != null)
            {
                startExperimentBTN.interactable = true;
            }

           


            if (startExButtonClicked) { 

                StartTask();
                
            }
        }
        
    }



    void buttonIsClicked()
    {
        btnIsClicked = true;
        TaskCanvas.GetComponent<Canvas>().enabled = false;
        TaskCanvas.GetComponent<GraphicRaycaster>().enabled = false;
    }
    void buttonExpIsClicked()
    {
        startExButtonClicked = true;
    }

    new void StartTask()
    {
        base.StartTask();
        StartCoroutine(Experiments());
    }

    public IEnumerator Post()
    {
        float time = Time.time - startTime;
        int score = Mathf.Clamp((int)(20 - time * 2 / TimeLimit), 0, 10); //here in score we need to include average attention value
        string accuracy = "High"; //accuracy depends on score
        if (score < 7) accuracy = "Medium";
        if (score < 5) accuracy = "Low";
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("username", UserName));
        formData.Add(new MultipartFormDataSection("attention_average", attentionAverage.ToString())); //here we are posting the average?
        formData.Add(new MultipartFormDataSection("score", score.ToString()));
        formData.Add(new MultipartFormDataSection("accuracy", accuracy));
        formData.Add(new MultipartFormDataSection("reaction_time", (time * 1000).ToString("0.0")));
        UnityWebRequest www = UnityWebRequest.Post(Constant.DOMAIN + Constant.SC8Data, formData);
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
        }
        yield return new WaitForSeconds(5);
        NextScene();
    }

    IEnumerator Experiments()
    {
        yield return new WaitForSeconds(0.1f);
        startTime = Time.time;
        StartCoroutine(LimitTimeCounter());

        if (BurnerFlame.activeSelf == true)
        {
            // burnFLameIsActive = true;
            StartCoroutine(Post());
        }
    }
    IEnumerator LimitTimeCounter()
    {
        yield return new WaitForSeconds(TimeLimit);
        StartCoroutine(Post());
    }

   
}
