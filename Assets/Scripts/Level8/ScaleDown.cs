using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScaleDown : MonoBehaviour
{
    public GameObject MiniPotassium;
    


 

    void ScaleToExample()
    {
        
        iTween.ScaleTo(this.gameObject, iTween.Hash("x", 0f, "y", 0f, "z", 0f, "time", 20f));

       

    }

    void Update()
    {
        ScaleToExample();

      

    }

}
