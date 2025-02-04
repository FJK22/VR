using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace PupilLabs
{
    [RequireComponent(typeof(CalibrationController))]
    public class CalibrationStatusText : MonoBehaviour
    {
        public SubscriptionsController subsCtrl;
        public Text statusText;
        public GameObject status;

        private CalibrationController calibrationController;

        public GameObject Scene;
        public GameObject Hand;

        void Awake()
        {
            SetStatusText("Not connected");
            calibrationController = GetComponent<CalibrationController>();
            Hand.SetActive(false);
        }

        void OnEnable()
        {
            subsCtrl.requestCtrl.OnConnected += OnConnected;
            calibrationController.OnCalibrationStarted += OnCalibrationStarted;
            calibrationController.OnCalibrationRoutineDone += OnCalibrationRoutineDone;
            calibrationController.OnCalibrationSucceeded += CalibrationSucceeded;
            calibrationController.OnCalibrationFailed += CalibrationFailed;
        }

        void OnDisable()
        {
            subsCtrl.requestCtrl.OnConnected -= OnConnected;
            calibrationController.OnCalibrationStarted -= OnCalibrationStarted;
            calibrationController.OnCalibrationRoutineDone -= OnCalibrationRoutineDone;
            calibrationController.OnCalibrationSucceeded -= CalibrationSucceeded;
            calibrationController.OnCalibrationFailed -= CalibrationFailed;
        }

        private void OnConnected()
        {
            string text = "Connected";
            text += "\n\nCalibration will start soon. Once started, please follow the circles only with your eyes without moving your head.";
            SetStatusText(text);
        }

        private void OnCalibrationStarted()
        {
            statusText.enabled = false;
        }

        private void OnCalibrationRoutineDone()
        {
            statusText.enabled = true;
            SetStatusText("Calibration is done.");

          
        }
        
        

        private void CalibrationSucceeded()
        {
            statusText.enabled = true;
            SetStatusText("Calibration succeeded.");

            StartCoroutine(DisableTextAfter(1));
            Scene.SetActive(true); //I make it active here. Used to work
            Hand.SetActive(true);
            status.SetActive(false);
        }

        private void CalibrationFailed()
        {
            statusText.enabled = true;
            SetStatusText("Calibration failed.");

            StartCoroutine(DisableTextAfter(1));
        }

        private void SetStatusText(string text)
        {
            if (statusText != null)
            {
                statusText.text = text;
            }
        }

        IEnumerator DisableTextAfter(float delay)
        {
            yield return new WaitForSeconds(delay);
            statusText.enabled = false;
        }
    }
}
