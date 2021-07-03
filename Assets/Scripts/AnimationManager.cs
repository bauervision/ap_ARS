using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Events;

using AMPS_Audio;

public class AnimationManager : MonoBehaviour
{
    [Header("Public Variables")]
    public Animator animator;
    public bool isOpened = true;

    [Header("Audio Related")]
    public AudioSource audioSource;

    public UI_Audio UI_Audio;
    public App_Audio App_Audio;
    public Mission_Audio Mission_Audio;





    #region Events
    [Header("Triggered Events")]
    public UnityEvent OnUIOpened = new UnityEvent();
    public UnityEvent OnUIClosed = new UnityEvent();

    #endregion


    public void PlayOpenAnimation()
    {
        animator.SetBool("open", false);
        OnUIOpened.Invoke();
        audioSource.PlayOneShot(UI_Audio.Panel_Open);
        isOpened = true;

    }

    public void PlayCloseAnimation()
    {
        animator.SetBool("open", true);
        OnUIClosed.Invoke();
        audioSource.PlayOneShot(UI_Audio.Panel_Close);
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
