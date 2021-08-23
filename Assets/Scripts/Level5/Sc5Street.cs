using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Sc5Street : LevelScript
{
    [SerializeField] GameObject phone;
    [SerializeField] GameObject mapCanvas;
    [SerializeField] GameObject mapPan;
    [SerializeField] GameObject missedCallPan;
    [SerializeField] GameObject callingPan;
    [SerializeField] GameObject messagePan;
    [SerializeField] AudioSource missedSound;
    [SerializeField] AudioSource callIncomeSound;
    [SerializeField] AudioSource messageSound;
    [SerializeField] float missedCallDelay = 3;
    [SerializeField] float messageDelay = 3;
    [SerializeField] float mapDelay = 3;

    int mapOpenCount = 0;
    float startTime = 0;
    bool isMapOpened = false;
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
        startTime = Time.time;
        mapCanvas.SetActive(true);
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
        phone.SetActive(true);
        switch (index)
        {
            case 0:
                StartCoroutine(MissedCall());
                break;
            case 1:
                callingPan.SetActive(true);
                callIncomeSound.Play();
                break;
            case 2:
                StartCoroutine(Message());
                break;
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
    IEnumerator Post()
    {

        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("username", UserName));
        formData.Add(new MultipartFormDataSection("reaction_time", ((Time.time - startTime) * 1000).ToString("0.0")));
        formData.Add(new MultipartFormDataSection("map_pressed", mapOpenCount.ToString()));
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
        missedCallPan.SetActive(true);
        yield return new WaitForSeconds(missedCallDelay);
        phone.SetActive(false);
        missedCallPan.SetActive(false);
    }
    IEnumerator Message()
    {
        messageSound.Play();
        messagePan.SetActive(true);
        yield return new WaitForSeconds(messageDelay);
        messagePan.SetActive(false);
        phone.SetActive(false);
    }
}
