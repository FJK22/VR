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

public class EEG : MonoBehaviour
{
    [Space]
    [Header("EEG")]
    private EEGSensor sensorStatusData;
    private LinkDataValue leftActivity;
    private LinkDataValue rightActivity;
    private LinkDataValue attention;
    private LinkDataValue relaxation;
    private LinkDataValue asymmetry;
    public List<string[]> EEGData = new List<string[]>();
    public EEGSensorID SelectFeatureSensorID;
    public string[] rowDataTemp;
    int count1 = 0;
    StreamWriter writer;


    [Space]
    [Header("EyeTracker")]
    public GameObject GazeTracker;
    public GameObject Recorder;

   

    void Start()
    {

        LooxidLinkManager.Instance.SetDebug(true);
        LooxidLinkManager.Instance.Initialize();

        leftActivity = new LinkDataValue();
        rightActivity = new LinkDataValue();
        attention = new LinkDataValue();
        relaxation = new LinkDataValue();
        asymmetry = new LinkDataValue();

        SaveData();
    }

    void SaveData()
    {
        var path = GetRecordingPath();
        writer = new StreamWriter(path);
        writer.WriteLine("AF3,AF4,FP1,FP2,AF7,AF8");
        


    }
    void OnEnable()
    {
        LooxidLinkData.OnReceiveEEGSensorStatus += OnReceiveEEGSensorStatus;
        LooxidLinkData.OnReceiveEEGRawSignals += OnReceiveEEGRawSignals;
        LooxidLinkData.OnReceiveMindIndexes += OnReceiveMindIndexes;

        // StartCoroutine(DisplayData());
    }
    void OnDisable()
    {
        LooxidLinkData.OnReceiveEEGSensorStatus -= OnReceiveEEGSensorStatus;
        LooxidLinkData.OnReceiveEEGRawSignals -= OnReceiveEEGRawSignals;
        LooxidLinkData.OnReceiveMindIndexes -= OnReceiveMindIndexes;
    }
    void Update()
    {
        if (LooxidLinkManager.Instance.isLinkHubConnected && LooxidLinkManager.Instance.isLinkCoreConnected)
        {
            if (sensorStatusData != null)
            {
                int numChannel = System.Enum.GetValues(typeof(EEGSensorID)).Length;

                for (int i = 0; i < numChannel; i++)
                {
                    bool isSensorOn = sensorStatusData.IsSensorOn((EEGSensorID)i);
                    
                    if (isSensorOn)
                    {
                        Debug.Log(i + " " + rowDataTemp[i]);

                        count1++;

                        //Open the program
                        if (count1 == 1) {

                            StartCoroutine(StartPupilCapture());

                            
                        }
                       
                    }
                }
            }
        }
    }

    void OnReceiveEEGSensorStatus(EEGSensor sensorStatusData)
    {
        this.sensorStatusData = sensorStatusData;
    }
    void OnReceiveMindIndexes(MindIndex mindIndexData)
    {
        leftActivity.target = double.IsNaN(mindIndexData.leftActivity) ? 0.0f : (float)LooxidLinkUtility.Scale(LooxidLink.MIND_INDEX_SCALE_MIN, LooxidLink.MIND_INDEX_SCALE_MAX, 0.0f, 1.0f, mindIndexData.leftActivity);
        rightActivity.target = double.IsNaN(mindIndexData.rightActivity) ? 0.0f : (float)LooxidLinkUtility.Scale(LooxidLink.MIND_INDEX_SCALE_MIN, LooxidLink.MIND_INDEX_SCALE_MAX, 0.0f, 1.0f, mindIndexData.rightActivity);
        attention.target = double.IsNaN(mindIndexData.attention) ? 0.0f : (float)LooxidLinkUtility.Scale(LooxidLink.MIND_INDEX_SCALE_MIN, LooxidLink.MIND_INDEX_SCALE_MAX, 0.0f, 1.0f, mindIndexData.attention);
        relaxation.target = double.IsNaN(mindIndexData.relaxation) ? 0.0f : (float)LooxidLinkUtility.Scale(LooxidLink.MIND_INDEX_SCALE_MIN, LooxidLink.MIND_INDEX_SCALE_MAX, 0.0f, 1.0f, mindIndexData.relaxation);
        asymmetry.target = double.IsNaN(mindIndexData.asymmetry) ? 0.0f : (float)LooxidLinkUtility.Scale(LooxidLink.MIND_INDEX_SCALE_MIN, LooxidLink.MIND_INDEX_SCALE_MAX, 0.0f, 1.0f, mindIndexData.asymmetry);
    }
    void OnReceiveEEGRawSignals(EEGRawSignal rawSignalData)
    {
        var path = GetRecordingPath();

        int numChannel = System.Enum.GetValues(typeof(EEGSensorID)).Length;

        for (int i = 0; i < numChannel; i++)
        {
           
           // rowDataTemp = new string[6];
            rowDataTemp[i] = rawSignalData.FilteredRawSignal((EEGSensorID)i).ToString();
            writer.WriteLine(rowDataTemp[i]);
           
            // EEGData.Add(rowDataTemp);
        }

        writer.Flush();
        writer.Close();

        

        
    }

    IEnumerator StartPupilCapture()
    {
        yield return new WaitForSeconds(3);
        GazeTracker.SetActive(true);
        Recorder.SetActive(true);
    }
    private string GetRecordingPath()
    {
        string path = "";

        string date = System.DateTime.Now.ToString("yyyy_MM_dd");
        //path = $"{Application.dataPath}/Data/{LevelScript.UserGroup}/{LevelScript.UserName + "_" + date}/Sc1LivingRoom/EEG/" + LevelScript.UserName + "_EEGData.csv";
        path = Application.dataPath + "test.csv";
        
        if (!System.IO.Directory.Exists(path))
        {
            System.IO.Directory.CreateDirectory(path);
        }

        return path;
    }
}
