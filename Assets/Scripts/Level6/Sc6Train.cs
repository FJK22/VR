using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Networking;
using System.Collections.Generic;

public class Sc6Train : LevelScript
{
    int CorrectIndex = 3; // 19 Gloucester Road
    public static string[] trainDestinations = {
        "5 Oxford Circus",
        "7 Great Portland Street",
        "11 Baker Street",
        "19 Gloucester Road",
        "29 Tottenham Court Road",
        "45 Bond Street",
        "58 Marble Arch",
        "73 Edgware Road",
        "95 Piccadilly Circus"};
    [SerializeField] Transform[] spawnPos = null;
    [SerializeField] GameObject trainPrefab = null;
    [SerializeField] AudioClip arriveAudio = null;
    [SerializeField] AudioClip leaveAudio = null;
    [SerializeField] float GeneratingDelay = 20f;
    [SerializeField] float WaitingTime = 10f;
    [SerializeField] int TimeLimit = 300;
    [SerializeField] float InTrainDelay = 2f;
    [SerializeField] Animator TalkAnimator = null;
    [SerializeField] AudioSource TalkAudio = null;
    public int buttonClickCount = 0;
    Transform[] arrivePos;
    Transform[] targetPos;
    int roadIndex = 0;
    float startTime = 0;
    int trainState = 0; // bit state => 00 : two road empty, 01 10 : one road fill, 11 : two road fill
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
        StartCoroutine(talk());
    }

    IEnumerator talk()
    {
        yield return new WaitForSeconds(3);
        TalkAnimator.enabled = true;
        TalkAudio.Play();
        StartCoroutine(PlayerRelease());
    }
    IEnumerator PlayerRelease()
    {
        yield return new WaitForSeconds(1);
        startTime = Time.deltaTime;
        PlayerFreeze = false;
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space) && !isStarted)
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
        if (trainState < 3)
        {
            if (trainState == 0)
            {
                roadIndex ^= 1;
            }
            else
            {
                roadIndex = (trainState == 1) ? 1 : 0;
            }
            Transform train = Instantiate(trainPrefab, spawnPos[roadIndex]).transform;
            Train trainScript = train.GetComponent<Train>();
            trainScript.DestinationIndex = Random.Range(0, trainDestinations.Length);
            trainScript.roadIndex = roadIndex;
            Sequence s = DOTween.Sequence();
            trainScript.sequence = s;
            s.Append(
                train.DOMove(arrivePos[roadIndex].position, 3, true).SetEase(Ease.OutQuad)
                .OnStart(() =>
                {
                    train.GetComponent<AudioSource>().PlayOneShot(arriveAudio);
                })
                .OnComplete(() =>
                {
                    trainState |= 1 << trainScript.roadIndex; // train state set
                    train.GetComponent<Animator>().SetBool("Open", true);
                }))
            .AppendInterval(WaitingTime)
            .AppendCallback(() =>
            {
                train.GetComponent<Animator>().SetBool("Open", false);
                train.GetComponent<AudioSource>().PlayOneShot(leaveAudio);
                train.GetComponent<Train>().DoorBlock.enabled = true;
            })
            .AppendInterval(2)
            .Append(
                train.DOMove(targetPos[roadIndex].position, 3, true).SetEase(Ease.InQuad)
                .OnComplete(() =>
                {
                    trainState &= ~(1 << trainScript.roadIndex);
                    //trainState ^= 1 << trainScript.roadIndex; // train state toggle
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
    public IEnumerator TrainTrigger(int index)
    {
        if (index < 0)
        {
            MessageManager.Instance.MessageOff();
        }
        else
        {
            yield return new WaitForSeconds(InTrainDelay);
            if (index > 0)
            {
                if (index == CorrectIndex)
                {
                    StartCoroutine(Post());
                }
                else
                {
                    MessageManager.Instance.Messge("You are in the wrong train.");
                }
            }
        }
    }
}
