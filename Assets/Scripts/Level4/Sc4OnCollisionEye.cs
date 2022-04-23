using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using Valve.VR;
using PupilLabs;
using UnityEngine.UI;
using Looxid.Link;
using System.IO;
using System.Text;
using UnityEngine.Networking;
using SimpleJSON;
using System;

public class Sc4OnCollisionEye : TimeSync
{
    
    [Space]
    [Header("Eye Tracker")]
    public GazeVisualizer gazeVisualizer;
    public GazeData gazeData;
    public Transform gazeOriginCamera;
    public GazeController gazeController;


    private void OnEnable()
    {
        if (gazeController)
        {
            gazeController.OnReceive3dGaze += OnReceive;
        }
    }
    
   
    private void OnReceive(GazeData obj)
    {
        gazeData = obj;
    }

    void Update()
    {

        if (gazeData != null)
        {
            Vector3 origin = gazeOriginCamera.position;
            Vector3 direction = gazeOriginCamera.TransformDirection(gazeData.GazeDirection);

            if (Physics.SphereCast(origin, 0.05f, direction, out RaycastHit hit, Mathf.Infinity))
            {
                if (hit.collider.CompareTag("AvatarsBar"))
                {
                    LookedAt("Avatars");

                }
                else
                {
                    LookedAt("Else");

                }


            }
        }

       
      
    }


    
    void LookedAt(string lookedAtWhat)
    {
        string date = System.DateTime.Now.ToString("yyyy_MM_dd");
        string path = $"{Application.dataPath}/Data/{LevelScript.UserGroup}/{LevelScript.UserName + "_" + date}/Sc6Club/EyeTracking/" + LevelScript.UserName + "_" + "LookedAt.csv";

        double pupilTime = GetPupilTimestamp();
        double unityTime = Time.realtimeSinceStartup;

        if (!File.Exists(path))
        {
            string header = "Pupil Timestamp,Unity Time,Looked At" + Environment.NewLine;

            File.AppendAllText(path, header);
        }

        string values = $"{pupilTime}, {unityTime}, {lookedAtWhat}" + Environment.NewLine;

        File.AppendAllText(path, values);
    }

    
}
