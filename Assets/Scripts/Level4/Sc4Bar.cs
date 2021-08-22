using System.Collections;
using UnityEngine;

public class Sc4Bar : LevelScript
{
    [SerializeField] AudioSource[] audios;
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
        foreach (var a in audios)
            a.Play();
    }

    IEnumerator EndTask()
    {
        yield return new WaitForSeconds(2);
        NextScene();
    }
}
