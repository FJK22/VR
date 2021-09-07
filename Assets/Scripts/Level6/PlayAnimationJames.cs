using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimationJames : MonoBehaviour
{
    public Animator anim;
    public GameObject James;

    void Start()
    {

        anim.GetComponent<Animator>();
        anim.SetBool("Animation1", false);
        anim.SetBool("Animation2", false);
        anim.SetBool("Animation3", false);
        anim.SetBool("Animation4", false);
        anim.SetBool("Animation5", false);
        anim.SetBool("Animation6", false);
        StartCoroutine("PlayAnimPart1");

    }

    IEnumerator PlayAnimPart1()
    {

        yield return new WaitForSeconds(10f);

        anim.SetBool("Animation1", true);

        yield return new WaitForSeconds(2f);

        anim.SetBool("Animation2", true);
        anim.SetBool("Animation1", false);

        yield return new WaitForSeconds(30f);

        anim.SetBool("Animation3", true);
        anim.SetBool("Animation1", false);
        anim.SetBool("Animation2", false);

        yield return new WaitForSeconds(3f);

        anim.SetBool("Animation4", true);
        anim.SetBool("Animation1", false);
        anim.SetBool("Animation2", false);
        anim.SetBool("Animation3", false);

        yield return new WaitForSeconds(8f);

        anim.SetBool("Animation5", true);
        anim.SetBool("Animation1", false);
        anim.SetBool("Animation2", false);
        anim.SetBool("Animation3", false);
        anim.SetBool("Animation4", false);

        yield return new WaitForSeconds(2f);

        anim.SetBool("Animation6", true);
        anim.SetBool("Animation1", false);
        anim.SetBool("Animation2", false);
        anim.SetBool("Animation3", false);
        anim.SetBool("Animation4", false);
        anim.SetBool("Animation5", false);


        yield return new WaitForSeconds(35f);

        anim.SetBool("Animation6", false);
        anim.SetBool("Animation1", false);
        anim.SetBool("Animation2", false);
        anim.SetBool("Animation3", false);
        anim.SetBool("Animation4", false);
        anim.SetBool("Animation5", false);

        James.SetActive(false);

    }
}
