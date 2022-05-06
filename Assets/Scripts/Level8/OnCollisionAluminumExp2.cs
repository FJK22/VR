using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisionAluminumExp2 : MonoBehaviour
{
    public GameObject MiniAluminum;
    public AudioSource Exp2AudioMiniAluminum;

    void Start()
    {
        MiniAluminum.SetActive(false);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "BeakerExp2")
        {
            MiniAluminum.SetActive(true);
            Exp2AudioMiniAluminum.enabled = true;
            Destroy(this.gameObject);
        }
    }
}
