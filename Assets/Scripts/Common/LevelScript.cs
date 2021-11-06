using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelScript : MonoBehaviour
{
    public static string UserName;
    public static bool IsVR;
    public static bool PlayerFreeze = false;
    //[SerializeField] protected GameObject MainCamera = null;
    [SerializeField] protected GameObject VRCamera;
    //public GameObject StartButton;
    [SerializeField] protected Button StartBTN;
    public Canvas TaskCanvas = null;
    [HideInInspector] public bool isStarted = false;
    [HideInInspector] public bool btnIsClicked = false;
    bool TaskLevel = true;
    void Start()
    {
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
    public void StartTask()
    {
        if (TaskCanvas) TaskCanvas.enabled = false;
        isStarted = true;
        AudioListener.volume = 1;
        Time.timeScale = 1;
    }
    public static void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public static void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
