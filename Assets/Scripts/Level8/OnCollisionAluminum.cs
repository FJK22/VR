using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisionAluminum : MonoBehaviour
{
   
    public AudioSource bromineAlimunim;
    public GameObject MiniAluminum;
    
  

    void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.tag == "BeakerExam2")
        {
            bromineAlimunim.enabled = true;
            MiniAluminum.SetActive(true);

            this.gameObject.SetActive(false);

            //Destroy(this.gameObject);


        }
    }

    
}
