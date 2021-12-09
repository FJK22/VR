using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;
using Valve.VR.InteractionSystem.Sample;
using UnitySimpleLiquid;
using SimpleJSON;
using UnityEngine.Networking;
using PupilLabs;
using Looxid.Link;

public class Sc8ChemistryLab : LevelScript
{

    //private LinkDataValue attention;

    [Space]
    [Header("Eye Tracker")]
    public RecordingController recorder;
    public Text statusText;

    [Space]
    [Header("EXP")]
    [SerializeField] TextMeshProUGUI Txt_Instruction;
    public GameObject ExperimentManager;
    private bool exp1finished = false;
    private bool attentionChecking = false;

    float startTime = 0;
    public Button startExperimentBTN;
    [SerializeField] int TimeLimit = 600;
    //bool startExButtonClicked;
    [HideInInspector] public bool startExButtonClicked = false;
    private int count;
    private float attentionAverage;
    bool burnFLameIsActive;
    int countBool = 0;
    int countBool2 = 0;

    bool btnIsClicked = false;
    public GameObject burnFlame;
    //public Button startButton;

    [Space]
    [Header("Experiment 1")]
    // public GameObject Plate;
    public GameObject PotassiumStone;
    public GameObject MiniPotassiumStone;
    //public Renderer plate_renderer;
    public MeshCollider meshColliderPotassium;
    int Exp1 = 0;

    [Space]
    [Header("Experiment 2")]
    public GameObject Beaker;
    public GameObject Aluminum;
    public GameObject MiniAluminum;
    public MeshCollider meshColliderAluminum;
    public MeshCollider meshColliderBromine;
    public MeshCollider meshColliderBromineFluid;
    public CapsuleCollider capsuleColliderBromine;
    bool Exp2 = false;

    [Space]
    [Header("Experiment 3")]
    public GameObject FluidPlate;
    public MeshCollider meshColliderNitromethane;
    public MeshCollider meshColliderNitromethaneFluid;
    public CapsuleCollider capsuleColliderNitromethane;
    public MeshCollider meshColliderMethanol;
    public MeshCollider meshColliderMethanolFluid;
    public CapsuleCollider capsuleColliderMethanol;
    public MeshCollider meshColliderLighter;
    public BoxCollider boxColliderLighter;
    public GameObject BurnerFlame;
    bool Exp3 = false;

    void Awake()
    {
        string date = System.DateTime.Now.ToString("yyyy_MM_dd");
        recorder.customPath = $"{Application.dataPath}/Data/{UserGroup}/{UserName + "_" + date}/Sc10ChemistryLab/EyeTracking";
        bool connected = recorder.requestCtrl.IsConnected;
    }
    void OnDestroy()
    {
        recorder.StopRecording();
    }
    void Start()
    {
        //attention = new LinkDataValue();

        startExperimentBTN.interactable = false;

        //plate_renderer = Plate.GetComponent<Renderer>();
        meshColliderPotassium = PotassiumStone.GetComponent<MeshCollider>();
        meshColliderAluminum = Aluminum.GetComponent<MeshCollider>();

        burnFLameIsActive = false;
    }

    void buttonIsClicked()
    {
        btnIsClicked = true;
        TaskCanvas.GetComponent<Canvas>().enabled = false;
        TaskCanvas.GetComponent<GraphicRaycaster>().enabled = false;
    }
    public void buttonExpIsClicked()
    {
        StartTask();

    }

    new public void StartTask()
    {
        base.StartTask();
        EEG.Instance.Init("Sc10ChemistryLab");
        StartCoroutine(Experiments());
        StartCoroutine(LimitTimeCounter());
    }

    //void OnEnable()
    //{
    //    LooxidLinkData.OnReceiveMindIndexes += OnReceiveMindIndexes;
    //}

    //void OnDisable()
    //{

    //    LooxidLinkData.OnReceiveMindIndexes -= OnReceiveMindIndexes;
    //}

    void Update()
    {
        StartBTN.onClick.AddListener(buttonIsClicked);

        EEG.Instance.attention.value = Mathf.Lerp((float)EEG.Instance.attention.value, (float)EEG.Instance.attention.target, 0.2f);
        attentionAverage = (float)(attentionAverage * count + EEG.Instance.attention.value) / (count + 1);
        count++;

        if (!isStarted && btnIsClicked)
        {
            recorder.StartRecording();
            startExperimentBTN.interactable = true;
            startExperimentBTN.onClick.AddListener(buttonExpIsClicked);
        }
        if (ExperimentManager.GetComponent<ExperimentManager>().CurrentExperiment1 == true)
        {
            StartCoroutine(CurrentExpt1());
        }
        if (ExperimentManager.GetComponent<ExperimentManager>().CurrentExperiment2 == true)
        {
            StartCoroutine(CurrentExpt2());
        }
        if (ExperimentManager.GetComponent<ExperimentManager>().CurrentExperiment3 == true)
        {
            StartCoroutine(CurrentExpt3());
        }
    }

    IEnumerator CurrentExpt1()
    {
        yield return new WaitForSeconds(0.1f);

        if (MiniPotassiumStone.activeSelf == false)
        {
           

            if (EEG.Instance.attention.value < 0.4)
            {
                meshColliderPotassium.enabled = false;
                Txt_Instruction.text = "Your attention level is low. You can only complete this experiment with higher attention levels.";
                attentionChecking = true;
            }
            else
            {
                meshColliderPotassium.enabled = true;
                Txt_Instruction.text = "Grab the Potassium and add it into the water of the beaker.";
            }

        }
        else
        {
            attentionChecking = false;
            Txt_Instruction.text = "This is a chemical reaction of potassium in the water. Well done for completing this experiment.";
            Destroy(PotassiumStone);
            StartCoroutine("Experiment2");
        }
    }
    IEnumerator CurrentExpt2()
    {
        yield return new WaitForSeconds(0.1f);

        if (Beaker.GetComponent<LiquidContainer>().fillAmountPercent > 0.5f) meshColliderAluminum.enabled = (EEG.Instance.attention.value >= 0.4f);


        if (EEG.Instance.attention.value < 0.4)
        {
            if (attentionChecking)
            {
                meshColliderBromine.enabled = false;
                meshColliderBromineFluid.enabled = false;
                capsuleColliderBromine.enabled = false;
                Txt_Instruction.text = "Your attention level is low. You can only complete this experiment with higher attention levels.";
            }
        }
        else if (MiniAluminum.activeSelf == false)
        {
            if (Beaker.GetComponent<LiquidContainer>().fillAmountPercent < 0.5f)
            {
                meshColliderBromine.enabled = true;
                meshColliderBromineFluid.enabled = true;
                capsuleColliderBromine.enabled = true;
                Txt_Instruction.text = "Pour Bromine to the beaker container.";
            }
            else
            {
                Txt_Instruction.text = "Put the alimunium into the beaker with bromine.";
            }
        }
        else
        {
            attentionChecking = false;
            Destroy(Aluminum);
            Txt_Instruction.text = "This is a chemical reaction of aliminum and bromine. Well done for completing this experiment.";
            StartCoroutine("Experiment3");
        }
    }

    IEnumerator CurrentExpt3()
    {
        yield return new WaitForSeconds(0.1f);
        if (FluidPlate.GetComponent<LiquidContainer>().fillAmountPercent > 0.9f) meshColliderLighter.enabled = (EEG.Instance.attention.value >= 0.4f);
        if (FluidPlate.GetComponent<LiquidContainer>().fillAmountPercent > 0.9f) boxColliderLighter.enabled = (EEG.Instance.attention.value >= 0.4f);

        if (EEG.Instance.attention.value < 0.4)
        {
            Debug.Log("attention < 0.4");
            if (attentionChecking)
            {
                meshColliderNitromethane.enabled = false;
                meshColliderNitromethaneFluid.enabled = false;
                capsuleColliderNitromethane.enabled = false;

                meshColliderMethanol.enabled = false;
                meshColliderMethanolFluid.enabled = false;
                capsuleColliderMethanol.enabled = false;

                meshColliderLighter.enabled = false;
                boxColliderLighter.enabled = false;
                Txt_Instruction.text = "Your attention level is low. You can only complete this experiment with higher attention levels.";
            }
        }
        else if (!BurnerFlame.activeSelf) 
        {
            if (FluidPlate.GetComponent<LiquidContainer>().fillAmountPercent == 0.0f)
            {
                meshColliderNitromethane.enabled = true; //we enable here first liquid
                meshColliderNitromethaneFluid.enabled = true;
                capsuleColliderNitromethane.enabled = true;

                meshColliderMethanol.enabled = false;
                meshColliderMethanolFluid.enabled = false;
                capsuleColliderMethanol.enabled = false;

                meshColliderLighter.enabled = false;
                boxColliderLighter.enabled = false;
                Txt_Instruction.text = "Pour the Nitromethane into the glass plate.";
            }
            else if (FluidPlate.GetComponent<LiquidContainer>().fillAmountPercent <= 0.3f) //if we pour first liquid into plate
            {
                meshColliderNitromethane.enabled = false;
                meshColliderNitromethaneFluid.enabled = false;
                capsuleColliderNitromethane.enabled = false;

                meshColliderMethanol.enabled = true; //we enable second liquid
                meshColliderMethanolFluid.enabled = true;
                capsuleColliderMethanol.enabled = true;

                meshColliderLighter.enabled = false;
                boxColliderLighter.enabled = false;
                Txt_Instruction.text = "Pour the Methanol that is in the tube into the glass plate.";
            }
            else
            {
                Txt_Instruction.text = "Add the lighter into the glass plate."; 
            }
        }
        else
        {
            Debug.Log("exp3 finish");
            attentionChecking = false;
            Txt_Instruction.text = "This is a chemical reaction of Nitromethane combnined with Methanol. Well done for completing this experiment.";
            burnFLameIsActive = true; 
            countBool++;

            if (burnFLameIsActive && countBool == 1)
            {
                burnFLameIsActive = false;
                recorder.StopRecording();
                StartCoroutine(PostData());
                Debug.Log("posted");
            }
        }
    }


    IEnumerator Experiment2()
    {
        if (!exp1finished)
        {
            exp1finished = true;
            yield return new WaitForSeconds(10);
            attentionChecking = true;
            ExperimentManager.GetComponent<ExperimentManager>().StartExperiment2();
        }
    }


    IEnumerator Experiment3()
    {
        yield return new WaitForSeconds(15);
        attentionChecking = true;
        ExperimentManager.GetComponent<ExperimentManager>().StartExperiment3();
    }

    IEnumerator Experiments()
    {
        yield return new WaitForSeconds(0.1f);
        startTime = Time.time;

    }
    public IEnumerator LimitTimeCounter()
    {
        startTime = Time.time;
        yield return new WaitForSeconds(TimeLimit);
        countBool2++;
        if (countBool2 == 1)
        {
            StartCoroutine(Post());
        }
    }

    IEnumerator PostData()
    {
        yield return new WaitForSeconds(7);
        StartCoroutine(Post());
    }



    IEnumerator Post()
    {

        float time = Time.time - startTime;
        float score = Mathf.Clamp((float)(20 - time * 2 / TimeLimit) + (attentionAverage * 100), 0, 100);
        string accuracy = "High";
        if (score < 60f) accuracy = "Medium";
        if (score < 20f) accuracy = "Low";
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("username", UserName));
        formData.Add(new MultipartFormDataSection("attention_average", attentionAverage.ToString()));
        formData.Add(new MultipartFormDataSection("score", score.ToString()));
        formData.Add(new MultipartFormDataSection("accuracy", accuracy));
        formData.Add(new MultipartFormDataSection("reaction_time", (time * 1000).ToString("0.0")));
        UnityWebRequest www = UnityWebRequest.Post(Constant.DOMAIN + Constant.SC8Data, formData);
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
        }
        StartCoroutine(SetLevel(SceneType.Sc8Questionnaire));
        NextScene();

    }

    //void OnReceiveMindIndexes(MindIndex mindIndexData)
    //{
    //    attention.target = double.IsNaN(mindIndexData.attention) ? 0.0f : (float)LooxidLinkUtility.Scale(LooxidLink.MIND_INDEX_SCALE_MIN, LooxidLink.MIND_INDEX_SCALE_MAX, 0.0f, 1.0f, mindIndexData.attention);
    //}


}
