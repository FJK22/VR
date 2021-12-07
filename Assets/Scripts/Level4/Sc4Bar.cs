using System.Collections;
using UnityEngine;
using PupilLabs;
using UnityEngine.UI;

public class Sc4Bar : LevelScript
{
    [SerializeField] AudioSource[] audios = null;
    public GameObject Pointer;

    [Space]
    [Header("Eye Tracker")]
    public RecordingController recorder;
    public Text statusText;

    void Awake()
    {
        string date = System.DateTime.Now.ToString("yyyy_MM_dd");
        recorder.customPath = $"{Application.dataPath}/Data/{UserGroup}/{UserName + "_" + date}/Sc6Club/EyeTracking";
        bool connected = recorder.requestCtrl.IsConnected;
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
    }

    void buttonIsClicked()
    {
        btnIsClicked = true;
    }

    new public void StartTask()
    {
        base.StartTask();
        foreach (var a in audios)
            a.Play();
    }

    IEnumerator EndTask()
    {
        recorder.StopRecording();
        yield return new WaitForSeconds(2);
        StartCoroutine(SetLevel(SceneType.Sc4Questionnaire));
        NextScene();
    }
}
