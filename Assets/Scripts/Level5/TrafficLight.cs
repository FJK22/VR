using System.Collections;
using Unity.Collections;
using UnityEngine;

public class TrafficLight : MonoBehaviour
{
    bool isRed = false;
    public bool IsEnter;
    [SerializeField] GameObject block;
    [SerializeField] MeshRenderer render;
    [SerializeField] float Delay = 20f;
    [SerializeField] Color GreenOn;
    [SerializeField] Color GreenOff;
    [SerializeField] Color RedOn;
    [SerializeField] Color RedOff;
    [SerializeField] AudioSource Alarm;

    [ReadOnly] Material green1;
    [ReadOnly] Material green2;
    [ReadOnly] Material red1;
    [ReadOnly] Material red2;

    private void Start()
    {
        green1 = render.materials[6];
        green2 = render.materials[3];
        red1 = render.materials[1];
        red2 = render.materials[4];
        IsRed = false;
        StartCoroutine(Replace());
    }

    bool IsRed {
        get { return isRed; }
        set
        {
            isRed = value;
            green1.SetColor("_EmissionColor", (isRed) ? GreenOff : GreenOn);
            green2.SetColor("_EmissionColor", (isRed) ? GreenOff : GreenOn);
            red1.SetColor("_EmissionColor", (isRed) ? RedOn: RedOff);
            red2.SetColor("_EmissionColor", (isRed) ? RedOn : RedOff);
            if (IsEnter)
            {
                if(Alarm)Alarm.Play();
            }
            else
            {
                block.SetActive(IsRed);
                if (Alarm) Alarm.Stop();
            }
        }
    }
    IEnumerator Replace()
    {
        yield return new WaitForSeconds(Delay);
        IsRed = !IsRed;
        StartCoroutine(Replace());
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            IsEnter = true;
            if (isRed && Alarm) Alarm.Play();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            IsEnter = false;
            if (Alarm) Alarm.Stop();
        }
    }
}
