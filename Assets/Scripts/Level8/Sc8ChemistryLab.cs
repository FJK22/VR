using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Sc8ChemistryLab : LevelScript
{
    public float startTime = 0f;
    public Button startExperimentBTN;
    [SerializeField] float MaxLimitTime = 600f;

    private void Update()
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

    new void StartTask()
    {
        base.StartTask();
        // StartCoroutine(ShowNumber(true));
    }
}
