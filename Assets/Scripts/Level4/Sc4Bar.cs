using System.Collections;
using UnityEngine;

public class Sc4Bar : LevelScript
{
    [SerializeField] AudioSource[] audios = null;
    void Update()
    {
        StartBTN.onClick.AddListener(buttonIsClicked);

        if (!isStarted && btnIsClicked)
        {
            StartTask();
        }
    }

    void buttonIsClicked()
    {
        btnIsClicked = true;
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
