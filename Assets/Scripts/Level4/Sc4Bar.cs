using System.Collections;
using UnityEngine;

public class Sc4Bar : LevelScript
{
    [SerializeField] AudioSource[] audios = null;
    public GameObject Pointer;
    void Update()
    {
        StartBTN.onClick.AddListener(buttonIsClicked);

        if (!isStarted && btnIsClicked)
        {
            StartTask();
            Pointer.SetActive(false);
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
