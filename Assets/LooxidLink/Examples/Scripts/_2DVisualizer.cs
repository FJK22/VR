using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;
using Valve.VR.InteractionSystem.Sample;
using UnitySimpleLiquid;

namespace Looxid.Link
{
    public enum Tab2DVisualizer
    {
        SENSOR_STATUS = 0,
        MIND_INDEX = 1,
        FEATURE_INDEX = 2,
        RAW_SIGNAL = 3
    }

    public class _2DVisualizer : MonoBehaviour
    {
        [Header("Tabs")]
        public Tab2DVisualizer SelectTab = Tab2DVisualizer.SENSOR_STATUS;
        public GameObject[] Tabs;
        public GameObject[] Panels;

        [Header("Message")]
        public bool displayLinkMessage;
        public CanvasGroup DisconnecetdPanel;
        public CanvasGroup SensorOffPanel;
        public CanvasGroup NoiseSignalPanel;

        [Header("Sensor Status")]
        public Image AF3SensorImage;
        public Image AF4SensorImage;
        public Image Fp1SensorImage;
        public Image Fp2SensorImage;
        public Image AF7SensorImage;
        public Image AF8SensorImage;

        [Header("Mind Index")]
        public BarIndicator LeftActivityIndicator;
        public BarIndicator RightActivityIndicator;
        public BarIndicator AttentionIndicator;
        public BarIndicator RelaxationIndicator;

        [Header("Feature Index")]
        public EEGSensorID SelectChannel;
        public BarIndicator DeltaIndicator;
        public BarIndicator ThetaIndicator;
        public BarIndicator AlphaIndicator;
        public BarIndicator BetaIndicator;
        public BarIndicator GammaIndicator;
        public Toggle[] ChannelToggles;

        [Header("Raw Signal")]
        public LineChart AF3Chart;
        public LineChart AF4Chart;
        public LineChart Fp1Chart;
        public LineChart Fp2Chart;
        public LineChart AF7Chart;
        public LineChart AF8Chart;

        private Color32 BackColor = new Color32(255, 255, 255, 255);
        private Color32 TextColor = new Color32(10, 10, 10, 255);

        private float disconnectedWindowAlpha = 0.0f;
        private float disconnectedWindowTargetAlpha = 0.0f;
        private float noiseSingalWindowAlpha = 0.0f;
        private float noiseSingalWindowTargetAlpha = 0.0f;
        private float sensorOffWindowAlpha = 0.0f;
        private float sensorOffWindowTargetAlpha = 0.0f;

        private LooxidLinkMessageType messageType;

        private EEGSensor sensorStatusData;
        private EEGRawSignal rawSignalData;

        private LinkDataValue leftActivity;
        private LinkDataValue rightActivity;
        public static LinkDataValue attention;
        private LinkDataValue relaxation;

        private LinkDataValue delta;
        private LinkDataValue theta;
        private LinkDataValue alpha;
        private LinkDataValue beta;
        private LinkDataValue gamma;

        public Button TablePan;
        public Button startButton;

        [Space]
        [Header("Experiment 1")]
       // public GameObject Plate;
        public GameObject PotassiumStone;
        public GameObject MiniPotassiumStone;
        //public Renderer plate_renderer;
        public MeshCollider meshColliderPotassium;
        
        [Space]
        [Header("Experiment 2")]
        public GameObject Beaker;
        public GameObject Aluminum;
        public GameObject MiniAluminum;
        public MeshCollider meshColliderAluminum;
        public MeshCollider meshColliderBromine;
        public MeshCollider meshColliderBromineFluid;
        public CapsuleCollider capsuleColliderBromine;

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

        [Space]
        [SerializeField] TextMeshProUGUI Txt_Instruction;
        public GameObject ExperimentManager;
        private bool exp1finished = false;
        private bool attentionChecking = false;

        void Start()
        {
            LooxidLinkManager.Instance.SetDebug(true);
            LooxidLinkManager.Instance.Initialize();

            LooxidLinkManager.Instance.SetDisplayDisconnectedMessage(displayLinkMessage);
            LooxidLinkManager.Instance.SetDisplayNoiseSignalMessage(displayLinkMessage);
            LooxidLinkManager.Instance.SetDisplaySensorOffMessage(displayLinkMessage);

            leftActivity = new LinkDataValue();
            rightActivity = new LinkDataValue();
            attention = new LinkDataValue();
            relaxation = new LinkDataValue();

            delta = new LinkDataValue();
            theta = new LinkDataValue();
            alpha = new LinkDataValue();
            beta = new LinkDataValue();
            gamma = new LinkDataValue();

             //plate_renderer = Plate.GetComponent<Renderer>();
            meshColliderPotassium = PotassiumStone.GetComponent<MeshCollider>();
            meshColliderAluminum = Aluminum.GetComponent<MeshCollider>();

        }

        void OnEnable()
        {
            LooxidLinkManager.OnLinkCoreConnected += OnLinkCoreConncetd;
            LooxidLinkManager.OnLinkHubConnected += OnLinkHubConnected;
            LooxidLinkManager.OnLinkCoreDisconnected += OnLinkCoreDisconncetd;
            LooxidLinkManager.OnLinkHubDisconnected += OnLinkHubDisconnected;
            LooxidLinkManager.OnShowSensorOffMessage += OnShowSensorOffMessage;
            LooxidLinkManager.OnHideSensorOffMessage += OnHideSensorOffMessage;
            LooxidLinkManager.OnShowNoiseSignalMessage += OnShowNoiseSignalMessage;
            LooxidLinkManager.OnHideNoiseSignalMessage += OnHideNoiseSignalMessage;

            LooxidLinkData.OnReceiveEEGSensorStatus += OnReceiveEEGSensorStatus;
            LooxidLinkData.OnReceiveEEGRawSignals += OnReceiveEEGRawSignals;
            LooxidLinkData.OnReceiveMindIndexes += OnReceiveMindIndexes;
            LooxidLinkData.OnReceiveEEGFeatureIndexes += OnReceiveEEGFeatureIndexes;

            StartCoroutine(DisplayData());
        }
        void OnDisable()
        {
            LooxidLinkManager.OnLinkCoreConnected -= OnLinkCoreConncetd;
            LooxidLinkManager.OnLinkHubConnected -= OnLinkHubConnected;
            LooxidLinkManager.OnLinkCoreDisconnected -= OnLinkCoreDisconncetd;
            LooxidLinkManager.OnLinkHubDisconnected -= OnLinkHubDisconnected;
            LooxidLinkManager.OnShowSensorOffMessage -= OnShowSensorOffMessage;
            LooxidLinkManager.OnHideSensorOffMessage -= OnHideSensorOffMessage;
            LooxidLinkManager.OnShowNoiseSignalMessage -= OnShowNoiseSignalMessage;
            LooxidLinkManager.OnHideNoiseSignalMessage -= OnHideNoiseSignalMessage;

            LooxidLinkData.OnReceiveEEGSensorStatus -= OnReceiveEEGSensorStatus;
            LooxidLinkData.OnReceiveEEGRawSignals -= OnReceiveEEGRawSignals;
            LooxidLinkData.OnReceiveMindIndexes -= OnReceiveMindIndexes;
            LooxidLinkData.OnReceiveEEGFeatureIndexes -= OnReceiveEEGFeatureIndexes;
        }

        void OnLinkCoreConncetd()
        {
            if( !displayLinkMessage )
            {
                HideMessage(LooxidLinkMessageType.CoreDisconnected);
            }
        }
        void OnLinkHubConnected()
        {
            if (!displayLinkMessage)
            {
                HideMessage(LooxidLinkMessageType.HubDisconnected);
            }
        }
        void OnLinkCoreDisconncetd()
        {
            if (!displayLinkMessage)
            {
                ShowMessage(LooxidLinkMessageType.CoreDisconnected);
            }
        }
        void OnLinkHubDisconnected()
        {
            if (!displayLinkMessage)
            {
                ShowMessage(LooxidLinkMessageType.HubDisconnected);
            }
        }
        void OnShowSensorOffMessage()
        {
            if (!displayLinkMessage)
            {
                ShowMessage(LooxidLinkMessageType.SensorOff);
            }
        }
        void OnHideSensorOffMessage()
        {
            if (!displayLinkMessage)
            {
                HideMessage(LooxidLinkMessageType.SensorOff);
            }
        }
        void OnShowNoiseSignalMessage()
        {
            if (!displayLinkMessage)
            {
                ShowMessage(LooxidLinkMessageType.NoiseSignal);
            }
        }
        void OnHideNoiseSignalMessage()
        {
            if (!displayLinkMessage)
            {
                HideMessage(LooxidLinkMessageType.NoiseSignal);
            }
        }

        // Data Subscription
        void OnReceiveEEGSensorStatus(EEGSensor sensorStatusData)
        {
            this.sensorStatusData = sensorStatusData;
        }
        void OnReceiveMindIndexes(MindIndex mindIndexData)
        {
            leftActivity.target = double.IsNaN(mindIndexData.leftActivity) ? 0.0f : (float)LooxidLinkUtility.Scale(LooxidLink.MIND_INDEX_SCALE_MIN, LooxidLink.MIND_INDEX_SCALE_MAX, 0.0f, 1.0f, mindIndexData.leftActivity);
            rightActivity.target = double.IsNaN(mindIndexData.rightActivity) ? 0.0f : (float)LooxidLinkUtility.Scale(LooxidLink.MIND_INDEX_SCALE_MIN, LooxidLink.MIND_INDEX_SCALE_MAX, 0.0f, 1.0f, mindIndexData.rightActivity);
            attention.target = double.IsNaN(mindIndexData.attention) ? 0.0f : (float)LooxidLinkUtility.Scale(LooxidLink.MIND_INDEX_SCALE_MIN, LooxidLink.MIND_INDEX_SCALE_MAX, 0.0f, 1.0f, mindIndexData.attention);
            relaxation.target = double.IsNaN(mindIndexData.relaxation) ? 0.0f : (float)LooxidLinkUtility.Scale(LooxidLink.MIND_INDEX_SCALE_MIN, LooxidLink.MIND_INDEX_SCALE_MAX, 0.0f, 1.0f, mindIndexData.relaxation);
        }
        void OnReceiveEEGRawSignals(EEGRawSignal rawSignalData)
        {
            AF3Chart.SetValue(rawSignalData.FilteredRawSignal(EEGSensorID.AF3));
            AF4Chart.SetValue(rawSignalData.FilteredRawSignal(EEGSensorID.AF4));
            Fp1Chart.SetValue(rawSignalData.FilteredRawSignal(EEGSensorID.Fp1));
            Fp2Chart.SetValue(rawSignalData.FilteredRawSignal(EEGSensorID.Fp2));
            AF7Chart.SetValue(rawSignalData.FilteredRawSignal(EEGSensorID.AF7));
            AF8Chart.SetValue(rawSignalData.FilteredRawSignal(EEGSensorID.AF8));
        }
        void OnReceiveEEGFeatureIndexes(EEGFeatureIndex featureIndexData)
        {
            double deltaValue = featureIndexData.Delta(SelectChannel);
            double thetaValue = featureIndexData.Theta(SelectChannel);
            double alphaValue = featureIndexData.Alpha(SelectChannel);
            double betaValue = featureIndexData.Beta(SelectChannel);
            double gammaValue = featureIndexData.Gamma(SelectChannel);

            delta.target = (double.IsInfinity(deltaValue) || double.IsNaN(deltaValue)) ? 0.0f : LooxidLinkUtility.Scale(delta.min, delta.max, 0.0f, 1.0f, deltaValue);
            theta.target = (double.IsInfinity(thetaValue) || double.IsNaN(thetaValue)) ? 0.0f : LooxidLinkUtility.Scale(theta.min, theta.max, 0.0f, 1.0f, thetaValue);
            alpha.target = (double.IsInfinity(alphaValue) || double.IsNaN(alphaValue)) ? 0.0f : LooxidLinkUtility.Scale(alpha.min, alpha.max, 0.0f, 1.0f, alphaValue);
            beta.target = (double.IsInfinity(betaValue) || double.IsNaN(betaValue)) ? 0.0f : LooxidLinkUtility.Scale(beta.min, beta.max, 0.0f, 1.0f, betaValue);
            gamma.target = (double.IsInfinity(gammaValue) || double.IsNaN(gammaValue)) ? 0.0f : LooxidLinkUtility.Scale(gamma.min, gamma.max, 0.0f, 1.0f, gammaValue);
        }

        IEnumerator DisplayData()
        {
            while (this.gameObject.activeSelf)
            {
                yield return new WaitForSeconds(0.1f);

                if (this.SelectTab == Tab2DVisualizer.SENSOR_STATUS)
                {
                    if (sensorStatusData != null)
                    {
                        Color32 offColor = new Color32(64, 64, 64, 255);

                        AF3SensorImage.color = sensorStatusData.IsSensorOn(EEGSensorID.AF3) ? (Color)LooxidLinkManager.linkColor : (Color)offColor;
                        AF4SensorImage.color = sensorStatusData.IsSensorOn(EEGSensorID.AF4) ? (Color)LooxidLinkManager.linkColor : (Color)offColor;
                        Fp1SensorImage.color = sensorStatusData.IsSensorOn(EEGSensorID.Fp1) ? (Color)LooxidLinkManager.linkColor : (Color)offColor;
                        Fp2SensorImage.color = sensorStatusData.IsSensorOn(EEGSensorID.Fp2) ? (Color)LooxidLinkManager.linkColor : (Color)offColor;
                        AF7SensorImage.color = sensorStatusData.IsSensorOn(EEGSensorID.AF7) ? (Color)LooxidLinkManager.linkColor : (Color)offColor;
                        AF8SensorImage.color = sensorStatusData.IsSensorOn(EEGSensorID.AF8) ? (Color)LooxidLinkManager.linkColor : (Color)offColor;
                        LeftActivityIndicator.SetValue((float)leftActivity.value);
                        RightActivityIndicator.SetValue((float)rightActivity.value);
                        AttentionIndicator.SetValue((float)attention.value);
                        RelaxationIndicator.SetValue((float)relaxation.value);


                    }
                }
                
                else if (this.SelectTab == Tab2DVisualizer.FEATURE_INDEX)
                {
                    List<EEGFeatureIndex> featureIndexList = LooxidLinkData.Instance.GetEEGFeatureIndexData(10.0f);

                    if (featureIndexList.Count > 0)
                    {
                        List<double> deltaScaleDataList = new List<double>();
                        List<double> thetaScaleDataList = new List<double>();
                        List<double> alphaScaleDataList = new List<double>();
                        List<double> betaScaleDataList = new List<double>();
                        List<double> gammaScaleDataList = new List<double>();

                        for (int i = 0; i < featureIndexList.Count; i++)
                        {
                            double deltaValue = featureIndexList[i].Delta(SelectChannel);
                            double thetaValue = featureIndexList[i].Theta(SelectChannel);
                            double alphaValue = featureIndexList[i].Alpha(SelectChannel);
                            double betaValue = featureIndexList[i].Beta(SelectChannel);
                            double gammaValue = featureIndexList[i].Gamma(SelectChannel);

                            if (!double.IsInfinity(deltaValue) && !double.IsNaN(deltaValue)) deltaScaleDataList.Add(deltaValue);
                            if (!double.IsInfinity(thetaValue) && !double.IsNaN(thetaValue)) thetaScaleDataList.Add(thetaValue);
                            if (!double.IsInfinity(alphaValue) && !double.IsNaN(alphaValue)) alphaScaleDataList.Add(alphaValue);
                            if (!double.IsInfinity(betaValue) && !double.IsNaN(betaValue)) betaScaleDataList.Add(betaValue);
                            if (!double.IsInfinity(gammaValue) && !double.IsNaN(gammaValue)) gammaScaleDataList.Add(gammaValue);
                        }

                        delta.SetScale(deltaScaleDataList);
                        theta.SetScale(thetaScaleDataList);
                        alpha.SetScale(alphaScaleDataList);
                        beta.SetScale(betaScaleDataList);
                        gamma.SetScale(gammaScaleDataList);
                    }

                    DeltaIndicator.SetValue((float)delta.value);
                    ThetaIndicator.SetValue((float)theta.value);
                    AlphaIndicator.SetValue((float)alpha.value);
                    BetaIndicator.SetValue((float)beta.value);
                    GammaIndicator.SetValue((float)gamma.value);
                }
            }
        }

        public void OnClickTabSensorStatus()
        {
            this.SelectTab = Tab2DVisualizer.SENSOR_STATUS;
            this.SelectTab = Tab2DVisualizer.MIND_INDEX;
        }
      
        public void OnClickTabFeatureIndex()
        {
            this.SelectTab = Tab2DVisualizer.FEATURE_INDEX;
        }
        public void OnClickTabRawSignal()
        {
            this.SelectTab = Tab2DVisualizer.RAW_SIGNAL;
        }
        public void OnClickLeftButton()
        {
            if (SelectTab > 0) SelectTab--;
            else SelectTab = (Tab2DVisualizer)System.Enum.GetValues(typeof(Tab2DVisualizer)).Length - 1;
        }
        public void OnClickRightButton()
        {
            if (SelectTab < (Tab2DVisualizer)System.Enum.GetValues(typeof(Tab2DVisualizer)).Length - 1) SelectTab++;
            else SelectTab = 0;
        }

        void Update()
        {
            if (Panels != null)
            {
                for (int i = 0; i < Panels.Length; i++)
                {
                    Panels[i].SetActive((Tab2DVisualizer)i == SelectTab);
                }
            }
            if (Tabs != null)
            {
                for (int i = 0; i < Tabs.Length; i++)
                {
                    Image TabImage = Tabs[i].GetComponent<Image>();
                    if (TabImage != null) TabImage.color = ((Tab2DVisualizer)i == SelectTab) ? (Color)LooxidLinkManager.linkColor : (Color)BackColor;

                    Text TabText = Tabs[i].GetComponent<Text>();
                    if (TabText != null) TabText.color = ((Tab2DVisualizer)i == SelectTab) ? BackColor : TextColor;

                    LaserPointerInputItem TabInputItem = Tabs[i].GetComponent<LaserPointerInputItem>();
                    if (TabInputItem != null)
                    {
                        TabInputItem.SetNormalColor(((Tab2DVisualizer)i == SelectTab) ? (Color)LooxidLinkManager.linkColor : (Color)BackColor);
                        TabInputItem.SetTextNormalColor(((Tab2DVisualizer)i == SelectTab) ? BackColor : TextColor);
                    }
                }
            }
            if (ChannelToggles != null)
            {
                for (int i = 0; i < ChannelToggles.Length; i++)
                {
                    ChannelToggles[i].isOn = ((EEGSensorID)i == SelectChannel);
                }
            }

            disconnectedWindowAlpha = Mathf.Lerp(disconnectedWindowAlpha, disconnectedWindowTargetAlpha, 0.2f);
            noiseSingalWindowAlpha = Mathf.Lerp(noiseSingalWindowAlpha, noiseSingalWindowTargetAlpha, 0.2f);
            sensorOffWindowAlpha = Mathf.Lerp(sensorOffWindowAlpha, sensorOffWindowTargetAlpha, 0.2f);

            if (DisconnecetdPanel != null) DisconnecetdPanel.alpha = disconnectedWindowAlpha;
            if (NoiseSignalPanel != null) NoiseSignalPanel.alpha = noiseSingalWindowAlpha;
            if (SensorOffPanel != null) SensorOffPanel.alpha = sensorOffWindowAlpha;

            leftActivity.value = Mathf.Lerp((float)leftActivity.value, (float)leftActivity.target, 0.2f);
            rightActivity.value = Mathf.Lerp((float)rightActivity.value, (float)rightActivity.target, 0.2f);
            attention.value = Mathf.Lerp((float)attention.value, (float)attention.target, 0.2f);

      
           // PeriodTable.isInteractable = attention.value >= 0.4;
            startButton.interactable = attention.value >= 0.4;


            if (ExperimentManager.GetComponent<ExperimentManager>().CurrentExperiment1 == true)
            {
                if (MiniPotassiumStone.activeSelf == false)
                {

                    if (attention.value < 0.4)
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
            if (ExperimentManager.GetComponent<ExperimentManager>().CurrentExperiment2 == true)
            {
                if(Beaker.GetComponent<LiquidContainer>().fillAmountPercent > 0.5f) meshColliderAluminum.enabled = (attention.value >= 0.4f);
                
                if (attention.value < 0.4)
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
            if (ExperimentManager.GetComponent<ExperimentManager>().CurrentExperiment3 == true)
            {

                if (FluidPlate.GetComponent<LiquidContainer>().fillAmountPercent > 0.9f) meshColliderLighter.enabled = (attention.value >= 0.4f);
                if (FluidPlate.GetComponent<LiquidContainer>().fillAmountPercent > 0.9f) boxColliderLighter.enabled = (attention.value >= 0.4f);

                if (attention.value < 0.4)
                {
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
                else if (BurnerFlame.activeSelf == false)
                {
                    if (FluidPlate.GetComponent<LiquidContainer>().fillAmountPercent == 0.0f)
                    {
                        meshColliderNitromethane.enabled = true;
                        meshColliderNitromethaneFluid.enabled = true;
                        capsuleColliderNitromethane.enabled = true;

                        meshColliderMethanol.enabled = false;
                        meshColliderMethanolFluid.enabled = false;
                        capsuleColliderMethanol.enabled = false;

                        meshColliderLighter.enabled = false;
                        boxColliderLighter.enabled = false;
                        Txt_Instruction.text = "Pour the Nitromethane into the glass plate.";
                    }

                    else if (FluidPlate.GetComponent<LiquidContainer>().fillAmountPercent <= 0.3f)
                    {
                        meshColliderNitromethane.enabled = false;
                        meshColliderNitromethaneFluid.enabled = false;
                        capsuleColliderNitromethane.enabled = false;

                        meshColliderMethanol.enabled = true;
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
                    attentionChecking = false;
                    Txt_Instruction.text = "This is a chemical reaction of Nitromethane combnined with Methanol. Well done for completing this experiment. You can now remove the headset.";

                }

            }




            relaxation.value = Mathf.Lerp((float)relaxation.value, (float)relaxation.target, 0.2f);

            delta.value = Mathf.Lerp((float)delta.value, (float)delta.target, 0.2f);
            theta.value = Mathf.Lerp((float)theta.value, (float)theta.target, 0.2f);
            alpha.value = Mathf.Lerp((float)alpha.value, (float)alpha.target, 0.2f);
            beta.value = Mathf.Lerp((float)beta.value, (float)beta.target, 0.2f);
            gamma.value = Mathf.Lerp((float)gamma.value, (float)gamma.target, 0.2f);
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
        



        public void OnSelectChannel(int num)
        {
            this.SelectChannel = (EEGSensorID)num;
        }

        public void ShowMessage(LooxidLinkMessageType messageType)
        {
            this.messageType = messageType;
            if (messageType == LooxidLinkMessageType.CoreDisconnected || messageType == LooxidLinkMessageType.HubDisconnected)
            {
                if (disconnectedWindowAlpha <= 0.0f)
                {
                    disconnectedWindowAlpha = 0.0f;
                    disconnectedWindowTargetAlpha = 0.82f;
                }
            }
            else if (messageType == LooxidLinkMessageType.NoiseSignal)
            {
                if (noiseSingalWindowAlpha <= 0.0f && sensorOffWindowAlpha <= 0.0f)
                {
                    noiseSingalWindowAlpha = 0.0f;
                    noiseSingalWindowTargetAlpha = 0.82f;
                }
            }
            else if (messageType == LooxidLinkMessageType.SensorOff)
            {
                if (sensorOffWindowAlpha <= 0.0f)
                {
                    sensorOffWindowAlpha = 0.0f;
                    sensorOffWindowTargetAlpha = 0.82f;
                    noiseSingalWindowTargetAlpha = -0.02f;
                }
            }
        }

        public void HideMessage(LooxidLinkMessageType messageType)
        {
            this.messageType = messageType;
            if (messageType == LooxidLinkMessageType.CoreDisconnected || messageType == LooxidLinkMessageType.HubDisconnected)
            {
                if (disconnectedWindowAlpha > 0.0f)
                {
                    disconnectedWindowTargetAlpha = -0.02f;
                }
            }
            else if (messageType == LooxidLinkMessageType.NoiseSignal)
            {
                if (noiseSingalWindowAlpha > 0.0f)
                {
                    noiseSingalWindowTargetAlpha = -0.02f;
                }
            }
            else if (messageType == LooxidLinkMessageType.SensorOff)
            {
                if (sensorOffWindowAlpha > 0.0f)
                {
                    sensorOffWindowTargetAlpha = -0.02f;
                }
            }
        }
    }
}