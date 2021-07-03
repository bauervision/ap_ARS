using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class AnimationManager : MonoBehaviour
{

    public Animator animator;
    public bool isOpened = false;


    public void PlayOpenAnimation()
    {
        animator.SetBool("open", false);
        isOpened = true;

    }

    public void PlayCloseAnimation()
    {
        animator.SetBool("open", true);
        isOpened = false;

    }

    public void ToggleUIPanel()
    {
        if (isOpened)
            PlayCloseAnimation();
        else
            PlayOpenAnimation();
    }

}
