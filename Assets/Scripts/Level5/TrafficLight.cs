using System.Collections;
using Unity.Collections;
using UnityEngine;

public class TrafficLight : MonoBehaviour
{
    bool isFirstRoadRed = false;
    public bool isEnter;
    [SerializeField] GameObject block;
    [SerializeField] MeshRenderer render;
    [SerializeField] float Delay = 20f;
    [SerializeField] Color GreenOn;
    [SerializeField] Color GreenOff;
    [SerializeField] Color RedOn;
    [SerializeField] Color RedOff;

    [SerializeField] Obstacle[] obstacle1;
    [SerializeField] Obstacle[] obstacle2;


    [ReadOnly] Material green1;
    [ReadOnly] Material green2;
    [ReadOnly] Material red1;
    [ReadOnly] Material red2;
    
    private void Start()
    {
        green1 = render.materials[6];
        green2 = render.materials[3];
        red1 = render.materials[4];
        red2 = render.materials[1];
        IsFirstRoadRed = true;
        StartCoroutine(Replace());
    }

    bool IsFirstRoadRed {
        get { return isFirstRoadRed; }
        set
        {
            isFirstRoadRed = value;
            green1.SetColor("_EmissionColor", (isFirstRoadRed) ? GreenOff : GreenOn);
            green2.SetColor("_EmissionColor", (isFirstRoadRed) ? GreenOn : GreenOff);
            red1.SetColor("_EmissionColor", (isFirstRoadRed) ? RedOn: RedOff);
            red2.SetColor("_EmissionColor", (isFirstRoadRed) ? RedOff : RedOn);
            if(!isEnter)
            {
                block.SetActive(IsFirstRoadRed);
            }
            foreach(var o1 in obstacle1)
            {
                o1.IsBlock = !value || isEnter;
                if (isEnter && value) o1.Alarm?.Invoke();
            }
            foreach(var o2 in obstacle2)
            {
                o2.IsBlock = value || isEnter;
            }
        }
    }
    
    IEnumerator Replace()
    {
        yield return new WaitForSeconds(Delay);
        IsFirstRoadRed = !IsFirstRoadRed;
        StartCoroutine(Replace());
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            isEnter = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            isEnter = false;
            if (IsFirstRoadRed) block.SetActive(true);
        }
    }
}
