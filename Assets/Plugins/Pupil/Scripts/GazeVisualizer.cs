﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PupilLabs
{
    public class GazeVisualizer : MonoBehaviour
    {
        public Transform gazeOrigin;
        public GazeController gazeController;

        [Header("Settings")]
        [Range(0f, 1f)]
        public float confidenceThreshold = 0.65f;
        public bool binocularOnly = true;

        [Header("Projected Visualization")]
        public Transform projectionMarker;
        public Transform gazeDirectionMarker;
        [Range(0.01f, 0.1f)]
        public float sphereCastRadius = 0.05f;

        Vector3 localGazeDirection;
        float gazeDistance;
        bool isGazing = false;

        bool errorAngleBasedMarkerRadius = true;
        float angleErrorEstimate = 2f;

        Vector3 origMarkerScale;
        MeshRenderer targetRenderer;
        float minAlpha = 0.2f;
        float maxAlpha = 0.8f;

        float lastConfidence;

        void OnEnable()
        {
            bool allReferencesValid = true;
            if (projectionMarker == null)
            {
                Debug.LogError("ProjectionMarker reference missing!");
                allReferencesValid = false;
            }
            if (gazeDirectionMarker == null)
            {
                Debug.LogError("GazeDirectionMarker reference missing!");
                allReferencesValid = false;
            }
            if (gazeOrigin == null)
            {
                Debug.LogError("GazeOrigin reference missing!");
                allReferencesValid = false;
            }
            if (gazeController == null)
            {
                Debug.LogError("GazeController reference missing!");
                allReferencesValid = false;
            }
            if (!allReferencesValid)
            {
                Debug.LogError("GazeVisualizer is missing required references to other components. Please connect the references, or the component won't work correctly.");
                enabled = false;
                return;
            }

            origMarkerScale = gazeDirectionMarker.localScale;
            targetRenderer = gazeDirectionMarker.GetComponent<MeshRenderer>();

            StartVisualizing();
        }

        void OnDisable()
        {
            if (gazeDirectionMarker != null)
            {
                gazeDirectionMarker.localScale = origMarkerScale;
            }

            StopVisualizing();
        }

        void Update()
        {
            if (!isGazing)
            {
                return;
            }

            VisualizeConfidence();

            ShowProjected();
        }

        public void StartVisualizing()
        {
            if (!enabled)
            {
                Debug.LogWarning("Component not enabled.");
                return;
            }

            if (isGazing)
            {
                Debug.Log("Already gazing!");
                return;
            }

            Debug.Log("Start Visualizing Gaze");

            gazeController.OnReceive3dGaze += ReceiveGaze;

            projectionMarker.gameObject.SetActive(true);
            gazeDirectionMarker.gameObject.SetActive(true);
            isGazing = true;
        }

        public void StopVisualizing()
        {
            if (!isGazing || !enabled)
            {
                Debug.Log("Nothing to stop.");
                return;
            }

            if (projectionMarker != null)
            {
                projectionMarker.gameObject.SetActive(false);
            }
            if (gazeDirectionMarker != null)
            {
                gazeDirectionMarker.gameObject.SetActive(false);
            }

            isGazing = false;

            gazeController.OnReceive3dGaze -= ReceiveGaze;
        }

        void ReceiveGaze(GazeData gazeData)
        {
            if (binocularOnly && gazeData.MappingContext != GazeData.GazeMappingContext.Binocular)
            {
                return;
            }

            lastConfidence = gazeData.Confidence;

            if (gazeData.Confidence < confidenceThreshold)
            {
                return;
            }

            localGazeDirection = gazeData.GazeDirection;
            gazeDistance = gazeData.GazeDistance;
        }

        void VisualizeConfidence()
        {
            if (targetRenderer != null)
            {
                Color c = targetRenderer.material.color;
                c.a = MapConfidence(lastConfidence);
                targetRenderer.material.color = c;
            }
        }

        void ShowProjected()
        {
            gazeDirectionMarker.localScale = origMarkerScale;

            Vector3 origin = gazeOrigin.position;
            Vector3 direction = gazeOrigin.TransformDirection(localGazeDirection);

            if (Physics.SphereCast(origin, sphereCastRadius, direction, out RaycastHit hit, Mathf.Infinity))
            {
                Debug.DrawRay(origin, direction * hit.distance, Color.yellow);

                projectionMarker.position = hit.point;

                gazeDirectionMarker.position = origin + direction * hit.distance;
                gazeDirectionMarker.LookAt(origin);


                if (errorAngleBasedMarkerRadius)
                {
                    gazeDirectionMarker.localScale = GetErrorAngleBasedScale(origMarkerScale, hit.distance, angleErrorEstimate);
                }
            }
            else
            {
                Debug.DrawRay(origin, direction * 10, Color.white);
            }
        }

        Vector3 GetErrorAngleBasedScale(Vector3 origScale, float distance, float errorAngle)
        {
            Vector3 scale = origScale;
            float scaleXY = distance * Mathf.Tan(Mathf.Deg2Rad * angleErrorEstimate) * 2;
            scale.x = scaleXY;
            scale.y = scaleXY;
            return scale;
        }

        float MapConfidence(float confidence)
        {
            return Mathf.Lerp(minAlpha, maxAlpha, confidence);
        }
    }
}