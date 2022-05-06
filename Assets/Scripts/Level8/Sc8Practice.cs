using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnitySimpleLiquid;

public class Sc8Practice : MonoBehaviour
{
    public GameObject Hand;
    public GameObject EEG;
    public bool ptcompleted;
    public Button btn;
    public GameObject PTButton;
    public GameObject CanvasInstructions;

    public GameObject Beaker;
    public GameObject BrownBottle;

    public TextMeshProUGUI robotInstructions;

    void Start()
    {
        ptcompleted = true;
        Beaker.SetActive(false);
        BrownBottle.SetActive(false);

        btn.onClick.AddListener(PTButtonOnClick);
    }
    void Update()
    {
        if (ptcompleted == true && Beaker.GetComponent<LiquidContainer>().fillAmountPercent > 0.5f)
        {
            StartCoroutine(PracticeCompleted());

            ptcompleted = false;
        }

    }


    void PTButtonOnClick() 
    {
        PTButton.SetActive(false);
        CanvasInstructions.SetActive(false);
        Beaker.SetActive(true);
        BrownBottle.SetActive(true);
        robotInstructions.text = "Grab the brown bottle with bromine with the controller and pour the bromine into the beaker.";

        
    }

    IEnumerator PracticeCompleted()
    {
        robotInstructions.text = "Practice completed, you will now start a calibration process before conducting the study.";
        yield return new WaitForSeconds(10.0f);
        Destroy(Beaker);
        Destroy(BrownBottle);
        Hand.SetActive(false);
        EEG.SetActive(true);
        this.gameObject.SetActive(false);
       
    }
    
}
