﻿using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Sc5Question : MonoBehaviour
{
    [SerializeField] Button BtSubmit;
    string _q1 = "";
    string _q2 = "";
    string _q3 = "";
    string _q4 = "";
    string _q5 = "";
    string _q6 = "";
    string _q7 = "";
    public string Q1 { set { _q1 = value; Validate(); } }
    public string Q2 { set { _q2 = value; Validate(); } }
    public string Q3 { set { _q3 = value; Validate(); } }
    public string Q4 { set { _q4 = value; Validate(); } }
    public string Q5 { set { _q5 = value; Validate(); } }
    public string Q6 { set { _q6 = value; Validate(); } }
    public string Q7 { set { _q7 = value; Validate(); } }
    void Validate()
    {
        BtSubmit.interactable = _q1 != "" && _q2 != "" && _q3 != "" && _q4 != "" && _q5 != "" && _q6 != "" && _q7 != "";
    }
    public void Submit()
    {
        StartCoroutine(PostData());        
    }
    IEnumerator PostData()
    {
        BtSubmit.interactable = false;
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        LevelScript.UserName = "test";
        formData.Add(new MultipartFormDataSection("username", LevelScript.UserName));
        formData.Add(new MultipartFormDataSection("q1", _q1));
        formData.Add(new MultipartFormDataSection("q2", _q2));
        formData.Add(new MultipartFormDataSection("q3", _q3));
        formData.Add(new MultipartFormDataSection("q4", _q4));
        formData.Add(new MultipartFormDataSection("q5", _q5));
        formData.Add(new MultipartFormDataSection("q6", _q6));
        formData.Add(new MultipartFormDataSection("q7", _q7));
        UnityWebRequest www = UnityWebRequest.Post(Constant.DOMAIN + Constant.SC5QS, formData);
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
        }
        else
        {
            JSONNode data = JSON.Parse(www.downloadHandler.text);
            Debug.Log(www.downloadHandler.text);
            if (data["status"] == "success")
            {
                LevelScript.NextScene();
            }
            else
            {
                Debug.LogError(data["msg"]);
                // ErrorMessage.text = data["msg"];
            }
        }
    }
}
