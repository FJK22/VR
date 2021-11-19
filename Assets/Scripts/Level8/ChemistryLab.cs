using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;
using Valve.VR.InteractionSystem.Sample;
using UnitySimpleLiquid;
using SimpleJSON;
using UnityEngine.Networking;
using PupilLabs;

public class ChemistryLab : MonoBehaviour
{
    public Button startExp;
    bool btnIsClicked = false;
    public GameObject burnFlame;

    [Space]
    [Header("Eye Tracker")]
    public RecordingController recorder;
    public Text statusText;

    void Awake()
    {
        string date = System.DateTime.Now.ToString("yyyy_MM_dd");
        recorder.customPath = $"{Application.dataPath}/Data/{LevelScript.UserGroup}/{LevelScript.UserName + "_" + date}/Sc10ChemistryLab/EyeTracking";
        bool connected = recorder.requestCtrl.IsConnected;
    }

    void Update()
    {
        startExp.onClick.AddListener(buttonIsClicked);

        if (btnIsClicked)
        {
            recorder.StartRecording();
        }
        if (burnFlame.activeSelf)
        {
            recorder.StopRecording();
        }
    }

    public void buttonIsClicked()
    {
        btnIsClicked = true;
    }
}
