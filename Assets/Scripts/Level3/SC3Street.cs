using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SC3Street : LevelScript
{
    [SerializeField] Transform[] SpawnPoses = null;
    [SerializeField] GameObject[] SpawnPrefabs = null;
    [SerializeField] float CarShowTime = 5f;
    [SerializeField] float CarSpeed = 10f;
    [SerializeField] float Delay = 2f;
    [SerializeField] int TotalCount = 10;
    [SerializeField] bool Correspond = false;

    int SpawnPosIndex;
    int count;
    float startTime;
    bool isPressed;
    new public void StartTask()
    {
        base.StartTask();
        StartCoroutine(ShowCar());
    }
    private void Update()
    {
        StartBTN.onClick.AddListener(buttonIsClicked);

        if (!isStarted && btnIsClicked) StartTask();
        if (!isPressed)
        {
            //TO BE FIXED FOR THE CONTROLLER
            //if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                //StartCoroutine(Post(true));
            //if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) 
               // StartCoroutine(Post(false));
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
        formData.Add(new MultipartFormDataSection("accuracy", (SpawnPosIndex == 0 == IsLeft == Correspond) ? "Correct": "Wrong"));
        formData.Add(new MultipartFormDataSection("reaction_time", ((Time.time - startTime) * 1000).ToString("0.0")));

        string url = Constant.DOMAIN + ((Correspond) ? Constant.SC3AData : Constant.SC3BData);
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
        SpawnPosIndex = Random.Range(0, 2);
        int _carIndex = Random.Range(0, SpawnPrefabs.Length);
        Instantiate(SpawnPrefabs[_carIndex], SpawnPoses[SpawnPosIndex]).AddComponent<AutoCar>().Set(CarShowTime, CarSpeed);
        startTime = Time.time;
        yield return new WaitForSeconds(CarShowTime + Delay);
        count++;
        if(count < TotalCount)
        {
            StartCoroutine(ShowCar());
        }
        else
        {
            NextScene();
        }
    }
}