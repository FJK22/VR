using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Sc5Street : LevelScript
{
    [SerializeField] float MaxLimitTime = 600f; 
    [SerializeField] GameObject phone = null;
    [SerializeField] GameObject mapCanvas = null;
    [SerializeField] GameObject mapPan = null;
    [SerializeField] GameObject missedCallPan = null;
    [SerializeField] GameObject callingPan = null;
    [SerializeField] GameObject messagePan = null;
    [SerializeField] AudioSource missedSound = null;
    [SerializeField] AudioSource callIncomeSound = null;
    [SerializeField] AudioSource messageSound = null;
    [SerializeField] float missedCallDelay = 3;
    [SerializeField] float messageDelay = 3;
    [SerializeField] float mapDelay = 3;

    public static Sc5Street Instance;
    private void Awake()
    {
        Instance = this;
    }
    public GameObject[] CarPrefabs = null;
    int mapOpenCount = 0;
    public float startTime = 0;
    bool isMapOpened = false;
    int marks = 10;
    int currentPointIndex = 0;
    void Update()
    {
        if (!isStarted && Input.GetKey(KeyCode.Space))
        {
            StartTask();
        }
    }
    new public void StartTask()
    {
        base.StartTask();
        if (IsVR)
        {
            Vector3 phonePos = phone.transform.localPosition;
            Quaternion phoneRot = phone.transform.localRotation;

            phone.transform.SetParent(VRCamera.transform.Find("Camera"));
            phone.transform.localRotation = phoneRot;
            phone.transform.localPosition = phonePos;
        }
        mapCanvas.SetActive(true);
        StartCoroutine(LimitTimer());
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
        //yield return new WaitForSeconds(0.01f);
        phone.SetActive(true);
        missedCallPan.SetActive(true);
        yield return new WaitForSeconds(missedCallDelay);
        phone.SetActive(false);
        missedCallPan.SetActive(false);
    }
    IEnumerator Message()
    {
        messageSound.Play();
       // yield return new WaitForSeconds(1);
        phone.SetActive(true);
        messagePan.SetActive(true);
        yield return new WaitForSeconds(messageDelay);
        messagePan.SetActive(false);
        phone.SetActive(false);
    }
    IEnumerator LimitTimer()
    {
        yield return new WaitForSeconds(MaxLimitTime);
        marks = 0;
        StartCoroutine(Post());
    }
}
