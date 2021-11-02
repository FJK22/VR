using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using SimpleJSON;

public class Loginmanager : MonoBehaviour
{
    [SerializeField] TMP_InputField IFUserName = null;
    [SerializeField] TMP_InputField IFAge = null;
    [SerializeField] TMP_Dropdown DDGender = null;
    [SerializeField] TMP_Dropdown DDHighestEducation = null;
    [SerializeField] TMP_Dropdown DDGroup = null;
    [SerializeField] TMP_Dropdown DDVision = null;
    [SerializeField] TMP_Dropdown DDHearing = null;
    [SerializeField] Button BtSubmit = null;
    [SerializeField] TMP_Text ErrorMessage = null;

    private string UserId;

    string _platform;
    public string Platform {
        get { return _platform; }
        set { _platform = value; }
    }

    private void Start()
    {
        Platform = "VR";
        IFUserName.onValueChanged.AddListener(delegate { Validate(); });
        IFAge.onValueChanged.AddListener(delegate { Validate(); });
        BtSubmit.onClick.AddListener(delegate { StartCoroutine(Login()); });
    }

    public IEnumerator Login() {
        BtSubmit.interactable = false;
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("username", IFUserName.text));
        formData.Add(new MultipartFormDataSection("age", IFAge.text));
        formData.Add(new MultipartFormDataSection("gender", DDGender.captionText.text));
        formData.Add(new MultipartFormDataSection("highest_education", DDHighestEducation.captionText.text));
        formData.Add(new MultipartFormDataSection("group", DDGroup.captionText.text));
        formData.Add(new MultipartFormDataSection("vision", DDVision.captionText.text));
        formData.Add(new MultipartFormDataSection("hearing", DDHearing.captionText.text));
        formData.Add(new MultipartFormDataSection("platform", Platform));

        UnityWebRequest www = UnityWebRequest.Post(Constant.DOMAIN + Constant.USER, formData);
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
        }
        else
        {
            JSONNode data = JSON.Parse(www.downloadHandler.text);
            if(data["status"] == "success")
            {
                LevelScript.UserName = IFUserName.text;
                LevelScript.IsVR = Platform == "VR";
                LevelScript.NextScene();
            }
            else
            {
                Debug.LogError(data["msg"]);
                StartCoroutine(Error(data["msg"]));
            }
        }
    }

    void Validate()
    {
        BtSubmit.interactable = IFUserName.text != "" && IFAge.text != "";
    }
    IEnumerator Error(string message){
        ErrorMessage.transform.parent.parent.gameObject.SetActive(true);
        ErrorMessage.text = message;
        yield return new WaitForSeconds(3);
        ErrorMessage.transform.parent.parent.gameObject.SetActive(false);
    }
}
