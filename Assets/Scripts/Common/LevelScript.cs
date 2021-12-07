using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelScript : MonoBehaviour
{
    public static string UserName;
    public static string UserGroup;
    public static bool IsVR;
  //  public static bool PlayerFreeze = false;
    //[SerializeField] protected GameObject MainCamera = null;
    [SerializeField] protected GameObject VRCamera;
    //public GameObject StartButton;
    [SerializeField] protected Button StartBTN;
    public Canvas TaskCanvas = null;
    public bool isStarted = false;
    public bool btnIsClicked = false;
    bool TaskLevel = true;
    void Start()
    {
        StartBTN.onClick.AddListener(buttonIsClicked);
        if (TaskLevel)
        {
            AudioListener.volume = 0;
            Time.timeScale = 0;
        }

        //if (VRCamera)
        //{
            //MainCamera.SetActive(!IsVR);
        //    VRCamera.SetActive(IsVR);
        //}
        //if(!IsVR && StartButton)
        //{
            //StartButton.SetActive(false);
        //}
    }
    void buttonIsClicked()
    {
        btnIsClicked = true;
        //Debug.Log("Button is pressed");
    }
    public void StartTask()
    {
        if (TaskCanvas) TaskCanvas.enabled = false;
        isStarted = true;
        AudioListener.volume = 1;
        Time.timeScale = 1;
    }
    public static IEnumerator SetLevel(SceneType sceneType)
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("username", UserName));
        formData.Add(new MultipartFormDataSection("level", sceneType.ToString()));
        UnityWebRequest www = UnityWebRequest.Post(Constant.DOMAIN + Constant.Level, formData);
        yield return www.SendWebRequest();
    }
    public static IEnumerator ClearData(string table)
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        UnityWebRequest www = UnityWebRequest.Delete($"{Constant.DOMAIN}{Constant.Clear}?username={UserName}&table={table}");
        yield return www.SendWebRequest();
    }

    public static void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public static void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
    public static void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
