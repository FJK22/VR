using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniAluminumEffect : MonoBehaviour
{
    public GameObject MiniAluminum;
    public GameObject BeakerExp2;
    public GameObject Fire;
    public Renderer MiniAluminumRenderer;
    public Material NewGlassMaterial;

    void Start()
    {
        MiniAluminumRenderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.activeSelf == true)
        {
            StartCoroutine(StartTiming());
        }
    }

    IEnumerator StartTiming() {

        yield return new WaitForSeconds(12.0f);
        Fire.SetActive(false);
        MiniAluminumRenderer.material.color = Color.black;
        BeakerExp2.GetComponent<MeshRenderer>().material = NewGlassMaterial;
    }
}
