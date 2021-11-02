using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Sc1LivingRoom : LevelScript
{
    [SerializeField] VideoPlayer video = null;
    [SerializeField] AudioSource[] audios = null;
    void Update()
    {
        StartBTN.onClick.AddListener(buttonIsClicked);

        if (!isStarted && btnIsClicked)
        {
             StartTask();
            
        }
        if (isStarted && video.isPaused)
        {
            isStarted = false;
            StartCoroutine(EndTask());
        }
    }

    new public void StartTask()
    {

        base.StartTask();
        video.Play();
        foreach (var a in audios)
            a.Play();
        
        
    }

    void buttonIsClicked()
    {
        btnIsClicked = true;
    }

    IEnumerator EndTask()
    {
        yield return new WaitForSeconds(1);
        NextScene();
    }
}
