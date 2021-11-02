using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniAluminum : MonoBehaviour
{
    public Renderer MiniAluminum_renderer;
    public GameObject SmokeFireExam2;
    public Material NewGlassMaterial;
    public GameObject BeakerExam2;
    public MeshCollider meshColliderMiniAluminum;

    void Start()
    {
        MiniAluminum_renderer = GetComponent<Renderer>();
        meshColliderMiniAluminum.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(this.gameObject.activeSelf == true)
        {
            StartCoroutine("Timing");

        }
    }

    IEnumerator Timing()
    {
        yield return new WaitForSeconds(12);

        MiniAluminum_renderer.material.color = Color.black;
        SmokeFireExam2.SetActive(false);
        BeakerExam2.GetComponent<MeshRenderer>().material = NewGlassMaterial;
        meshColliderMiniAluminum.enabled = true;

    }
}
