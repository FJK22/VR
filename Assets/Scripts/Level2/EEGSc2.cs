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
using System;

public class EEGSc2 : MonoBehaviour
{
    public Button startButton;
    public LevelScript level;
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

    [Space]
    [Header("EyeTracker")]
    public GameObject GazeTracker;
    public GameObject Recorder;

    EGGData eegData;

    void Start()
    {

        LooxidLinkManager.Instance.SetDebug(true);
        LooxidLinkManager.Instance.Initialize();

        leftActivity = new LinkDataValue();
        rightActivity = new LinkDataValue();
        attention = new LinkDataValue();
        relaxation = new LinkDataValue();
        asymmetry = new LinkDataValue();

        // string path = GetRecordingPath();
        //StreamWriter writer = new StreamWriter(path);
        //writer.WriteLine("AF3,AF4,FP1,FP2,AF7,AF8");
        string date = System.DateTime.Now.ToString("yyyy_MM_dd");
        eegData = new EGGData(
            $"{Application.dataPath}/Data/{LevelScript.UserGroup}/{LevelScript.UserName + "_" + date}/Sc2LectureHall/EEG/"
            , LevelScript.UserName + "_" + "EEGData.csv");
    }
    
   
    void OnEnable()
    {
        LooxidLinkData.OnReceiveEEGSensorStatus += OnReceiveEEGSensorStatus;
        LooxidLinkData.OnReceiveEEGRawSignals += OnReceiveEEGRawSignals;
        LooxidLinkData.OnReceiveMindIndexes += OnReceiveMindIndexes;
        LooxidLinkData.OnReceiveEEGFeatureIndexes += OnReceiveEEGFeatureIndexes;

        // StartCoroutine(DisplayData());
    }

    private void OnReceiveEEGFeatureIndexes(EEGFeatureIndex feature)
    {
        if (level.isStarted)
        {
            eegData.SetFeature(feature);
        }
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
                       
                        count1++;

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
        if (level.isStarted)
        {
            mindIndexData.leftActivity = leftActivity.target;
            mindIndexData.rightActivity = rightActivity.target;
            mindIndexData.attention = attention.target;
            mindIndexData.relaxation = relaxation.target;
            mindIndexData.asymmetry = asymmetry.target;
            eegData.SetMind(mindIndexData);
        }
    }

    void OnReceiveEEGRawSignals(EEGRawSignal rawSignalData)
    {
       // string path = GetRecordingPath();
        //StreamWriter writer = new StreamWriter(path);

        int numChannel = System.Enum.GetValues(typeof(EEGSensorID)).Length;

        for (int i = 0; i < numChannel; i++) 
        {

            // rowDataTemp = new string[6];
           // rowDataTemp[i] = rawSignalData.FilteredRawSignal((EEGSensorID)i).ToString();
           // writer.WriteLine(numChannel.GetType().ToString() +"," + rowDataTemp[i]); 

            // EEGData.Add(rowDataTemp);
        }

       // writer.Flush();
        //writer.Close();



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
        string file = "";
        string filePath = "";

        string date = System.DateTime.Now.ToString("yyyy_MM_dd");
        path = $"{Application.dataPath}/Data/{LevelScript.UserGroup}/{LevelScript.UserName + "_" + date}/Sc2LectureHall/EEG/"; 
        file = LevelScript.UserName + "_" + "EEGData.csv";
        filePath = path + file;

        if (!System.IO.Directory.Exists(path))
        {
            System.IO.Directory.CreateDirectory(path);
        }

        return filePath;
    }
}

public class EGGData2{
    public enum EGGDataStatus
    {
        NotSet,
        FeatureSet,
        MindSet
    }
    string _path;
    EEGFeatureIndex _f;
    MindIndex _m;
    string _filePath = "";
    EGGDataStatus status = EGGDataStatus.NotSet;
    public void SetFeature(EEGFeatureIndex index)
    {
        _f = index;
        if(status == EGGDataStatus.MindSet)
        {
            Save();
        }
        else
        {
            status = EGGDataStatus.FeatureSet;
        }
    }
    public void SetMind(MindIndex index)
    {
        _m = index;
        if(status == EGGDataStatus.FeatureSet)
        {
            Save();
        }
        else
        {
            status = EGGDataStatus.MindSet;
        }
    }
    public void Save()
    {
        string values =
            $"{_f.Alpha(EEGSensorID.AF3)},{_f.Beta(EEGSensorID.AF3)},{_f.Gamma(EEGSensorID.AF3)},{_f.Delta(EEGSensorID.AF3)},{_f.Theta(EEGSensorID.AF3)}," +
            $"{_f.Alpha(EEGSensorID.AF4)},{_f.Beta(EEGSensorID.AF4)},{_f.Gamma(EEGSensorID.AF4)},{_f.Delta(EEGSensorID.AF4)},{_f.Theta(EEGSensorID.AF4)}," +
            $"{_f.Alpha(EEGSensorID.Fp1)},{_f.Beta(EEGSensorID.Fp1)},{_f.Gamma(EEGSensorID.Fp1)},{_f.Delta(EEGSensorID.Fp1)},{_f.Theta(EEGSensorID.Fp1)}," +
            $"{_f.Alpha(EEGSensorID.Fp2)},{_f.Beta(EEGSensorID.Fp2)},{_f.Gamma(EEGSensorID.Fp2)},{_f.Delta(EEGSensorID.Fp2)},{_f.Theta(EEGSensorID.Fp2)}," +
            $"{_f.Alpha(EEGSensorID.AF7)},{_f.Beta(EEGSensorID.AF7)},{_f.Gamma(EEGSensorID.AF7)},{_f.Delta(EEGSensorID.AF7)},{_f.Theta(EEGSensorID.AF7)}," +
            $"{_f.Alpha(EEGSensorID.AF8)},{_f.Beta(EEGSensorID.AF8)},{_f.Gamma(EEGSensorID.AF8)},{_f.Delta(EEGSensorID.AF8)},{_f.Theta(EEGSensorID.AF8)}," +
            $"{_m.attention},{_m.asymmetry}, {_m.leftActivity},{_m.relaxation},{_m.rightActivity}{Environment.NewLine}";
        File.AppendAllText(_filePath, values);
        status = EGGDataStatus.NotSet;
    }
    public EGGData2(string path, string file)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        _filePath = Path.Combine(path, file);
        if (File.Exists(_filePath))
        {
            File.Delete(_filePath);
        }
        FileStream fs = File.Create(_filePath);
        fs.Close();
        string header = 
            "alpha_AF3,beta_AF3,gamma_AF3,delta_AF3,theta_AF3," +
            "alpha_AF4,beta_AF4,gamma_AF4,delta_AF4,theta_AF4," +
            "alpha_FP1,beta_FP1,gamma_FP1,delta_FP1,theta_FP1," +
            "alpha_FP2,beta_FP2,gamma_FP2,delta_FP2,theta_FP2," +
            "alpha_AF7,beta_AF7,gamma_AF7,delta_AF7,theta_AF7," +
            "alpha_AF8,beta_AF8,gamma_AF8,delta_AF8,theta_AF8," +
            "attention,asymetry,left_activity,relaxation,right_activity" + Environment.NewLine;
        File.AppendAllText(_filePath, header);
    }
}
