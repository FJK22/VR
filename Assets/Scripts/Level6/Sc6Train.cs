using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Networking;
using System.Collections.Generic;

public class Sc6Train : LevelScript
{
    int CorrectIndex = 3; // 19 Gloucester Road
    public static string[] trainDestinations = {
        "<color=\"red\">5</color> Oxford Circus",
        "<color=\"red\">7</color> Great Portland Street",
        "<color=\"red\">11</color> Baker Street",
        "<color=\"red\">19</color> Gloucester Road",
        "<color=\"red\">29</color> Tottenham Court Road",
        "<color=\"red\">45</color> Bond Street",
        "<color=\"red\">58</color> Marble Arch",
        "<color=\"red\">73</color> Edgware Road",
        "<color=\"red\">95</color> Piccadilly Circus"};
    [SerializeField] Transform[] spawnPos = null;
    [SerializeField] GameObject trainPrefab = null;
    [SerializeField] float GeneratingDelay = 20f;
    [SerializeField] float WaitingTime = 10f;
    [SerializeField] int TimeLimit = 300;
    [SerializeField] float InTrainDelay = 2f;
    public int buttonClickCount = 0;
    Transform[] arrivePos;
    Transform[] targetPos;
    int roadIndex = 0;
    float startTime = 0;
    bool isInTrain = false;
    public bool IsInTrain
    {
        get { return isInTrain; }
        set
        {
            isInTrain = value;
            if (isInTrain)
            {
                DOTween.PauseAll();
            }
            else
            {
                DOTween.PlayAll();
            }
        }
    }
    new void StartTask()
    {
        PlayerFreeze = true;
        base.StartTask();
        arrivePos = new Transform[2];
        targetPos = new Transform[2];
        for (int i = 0; i < 2; i++)
        {
            arrivePos[i] = spawnPos[i].GetChild(0);
            targetPos[i] = spawnPos[i].GetChild(1);
        }
        StartCoroutine(GernerateTrain());
        StartCoroutine(PlayerRelease());
    }
    IEnumerator PlayerRelease()
    {
        yield return new WaitForSeconds(59);
        startTime = Time.deltaTime;
        PlayerFreeze = false;
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            StartTask();
        }
    }
    IEnumerator Post()
    {
        float time = Time.time - startTime;
        int marks = Mathf.Clamp((int)(20 - time * 2 / TimeLimit) - buttonClickCount, 0, 10);
        string accuracy = "High";
        if (marks < 7) accuracy = "Medium";
        if (marks < 5) accuracy = "Low";
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("username", UserName));
        formData.Add(new MultipartFormDataSection("reaction_time", (time * 1000).ToString("0.0")));
        formData.Add(new MultipartFormDataSection("accuracy", accuracy));
        formData.Add(new MultipartFormDataSection("button_pressed", buttonClickCount.ToString()));
        UnityWebRequest www = UnityWebRequest.Post(Constant.DOMAIN + Constant.SC6Data, formData);
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
        }
        NextScene();
    }
    IEnumerator GernerateTrain()
    {
        if (!isInTrain)
        {
            roadIndex = (roadIndex == 0) ? 1 : 0;
            Transform train = Instantiate(trainPrefab, spawnPos[roadIndex]).transform;
            train.GetComponent<Train>().TrainIndex = Random.Range(0, trainDestinations.Length);
            Sequence s = DOTween.Sequence();
            s.Append(
                train.DOMove(arrivePos[roadIndex].position, 3, true).SetEase(Ease.OutQuad)
                .OnStart(() =>
                {
                    train.GetComponent<AudioSource>().Play();
                })
                .OnComplete(() =>
                {
                    train.GetComponent<Animator>().SetBool("Open", true);
                }))
            .AppendInterval(WaitingTime)
            .AppendCallback(() => { train.GetComponent<Animator>().SetBool("Open", false); })
            .AppendInterval(2)
            .Append(
                train.DOMove(targetPos[roadIndex].position, 3, true).SetEase(Ease.InQuad)
                .OnComplete(() =>
                {
                    Destroy(train.gameObject);
                })
            );
        }
        yield return new WaitForSeconds(GeneratingDelay);
        StartCoroutine(GernerateTrain());
    }
    IEnumerator LimitTimeCounter()
    {
        yield return new WaitForSeconds(TimeLimit);
        StartCoroutine(Post());
    }
    public IEnumerator TrainTrigger(bool isIn, int index)
    {
        IsInTrain = isIn;
        if (!isIn)
        {
            MessageManager.Instance.MessageOff();
        }
        else
        {
            yield return new WaitForSeconds(InTrainDelay);
            if (isIn)
            {
                if (index == CorrectIndex)
                {
                    StartCoroutine(Post());
                }
                else
                {
                    MessageManager.Instance.Messge("You are in wrong train.");
                }
            }
        }
    }
}
