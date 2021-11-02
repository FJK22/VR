using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OnCollision : MonoBehaviour
{
    public GameObject MiniPotassium;
    public AudioSource exp1Audio;
    public bool CollisionPotassium;
    
   
 
    void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.tag == "WaterLiquid")
        {
            this.gameObject.SetActive(false);
            MiniPotassium.SetActive(true);
            exp1Audio.enabled = true;
            //Destroy(this.gameObject);
            CollisionPotassium = true;



        }
    }
}
