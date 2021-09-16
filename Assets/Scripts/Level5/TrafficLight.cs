using System.Collections;
using Unity.Collections;
using UnityEngine;

public class TrafficLight : MonoBehaviour
{
    bool isFirstRoadRed = false;
    [HideInInspector] public bool isEnter1 = false;
    [HideInInspector] public bool isEnter2 = false;

    [SerializeField] PersonBlock playerBlock1 = null;
    [SerializeField] PersonBlock playerBlock2 = null;
    [SerializeField] MeshRenderer render = null;
    [SerializeField] MeshRenderer reverseRender = null;
    [SerializeField] float TrafficCrossTime = 20f;
    [SerializeField] float CrossDelay = 1f;
    [SerializeField] Color GreenOn = Color.green;
    [SerializeField] Color GreenOff = Color.black;
    [SerializeField] Color RedOn = Color.red;
    [SerializeField] Color RedOff = Color.black;

    [SerializeField] Obstacle[] obstacle1 = null;
    [SerializeField] Obstacle[] obstacle2 = null;

    AudioSource audioSource;

    [ReadOnly] Material green1;
    [ReadOnly] Material green2;
    [ReadOnly] Material red1;
    [ReadOnly] Material red2;

    [ReadOnly] Material rGreen1;
    [ReadOnly] Material rGreen2;
    [ReadOnly] Material rRed1;
    [ReadOnly] Material rRed2;

    
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        green1 = render.materials[6];
        green2 = render.materials[3];
        red1 = render.materials[4];
        red2 = render.materials[1];

        if (reverseRender)
        {
            rGreen1 = reverseRender.materials[6];
            rGreen2 = reverseRender.materials[3];
            rRed1 = reverseRender.materials[4];
            rRed2 = reverseRender.materials[1];
        }
        IsFirstRoadRed = true;
        StartCoroutine(Replace());
        if (playerBlock2)
        {
            playerBlock2.isFirstRoad = false;
        }
    }

    public bool IsFirstRoadRed {
        get { return isFirstRoadRed; }
        set
        {
            isFirstRoadRed = value;
            if (!isFirstRoadRed && Sc5Street.Instance.startTime == 0) StartCoroutine(Sc5Street.Instance.LimitTimer());
            green1.SetColor("_EmissionColor", (isFirstRoadRed) ? GreenOff : GreenOn);
            green2.SetColor("_EmissionColor", (isFirstRoadRed) ? GreenOn : GreenOff);
            red1.SetColor("_EmissionColor", (isFirstRoadRed) ? RedOn: RedOff);
            red2.SetColor("_EmissionColor", (isFirstRoadRed) ? RedOff : RedOn);

            if (reverseRender)
            {
                rGreen2.SetColor("_EmissionColor", (isFirstRoadRed) ? GreenOff : GreenOn);
                rGreen1.SetColor("_EmissionColor", (isFirstRoadRed) ? GreenOn : GreenOff);
                rRed2.SetColor("_EmissionColor", (isFirstRoadRed) ? RedOn : RedOff);
                rRed1.SetColor("_EmissionColor", (isFirstRoadRed) ? RedOff : RedOn);
            }


            if(!isEnter1)
            {
                playerBlock1.SetCollider(IsFirstRoadRed);
            }
            if (!isEnter2 && playerBlock2)
            {
                playerBlock2.SetCollider(!isFirstRoadRed);
            }
            StartCoroutine(CarPass());
            if((isEnter1 && value) || (isEnter2 && !value))
            {
                audioSource.Play();
            }
            else
            {
                audioSource.Stop();
            }
        }
    }
    
    IEnumerator CarPass()
    {
        StopCoroutine(CarPass());
        if (isFirstRoadRed)
        {
            foreach (var o2 in obstacle2)
            {
                o2.IsBlock = true;
            }
            yield return new WaitForSeconds(CrossDelay);
            foreach (var o1 in obstacle1)
            {
                o1.IsBlock = isEnter1;
            }
        }
        else
        {
            foreach (var o1 in obstacle1)
            {
                o1.IsBlock = true;
            }
            yield return new WaitForSeconds(CrossDelay);
            foreach (var o2 in obstacle2)
            {
                o2.IsBlock = isEnter2;
            }
            
            
        }
    }
    IEnumerator Replace()
    {
        yield return new WaitForSeconds(TrafficCrossTime);
        IsFirstRoadRed = !IsFirstRoadRed;
        StartCoroutine(Replace());
    }
}
