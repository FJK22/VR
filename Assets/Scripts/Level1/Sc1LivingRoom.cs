using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Sc1LivingRoom : LevelScript
{
    [SerializeField] VideoPlayer video;
    [SerializeField] AudioSource[] audios;
    void Update()
    {
        if (!isStarted && Input.GetKey(KeyCode.Space))
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

    IEnumerator EndTask()
    {
        yield return new WaitForSeconds(2);
        NextScene();
    }
}
