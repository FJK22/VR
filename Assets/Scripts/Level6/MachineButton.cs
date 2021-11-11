using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineButton : MonoBehaviour
{
    [SerializeField] AudioSource audioSouce = null;
    Sc6Train gameManager;
    private void Start()
    {
        gameManager = FindObjectOfType<Sc6Train>();
    }
  //  private void OnMouseUp()
    //{
  //      if (!audioSouce.isPlaying)
   //     {
   //         audioSouce.Play();
   //         gameManager.buttonClickCount++;
   //     }
   // }

    void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.tag == "RightHand")
        {
            if (!audioSouce.isPlaying)
            {
                audioSouce.Play();
                gameManager.buttonClickCount++;
            }


        }
    }
}
