using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Events;

using AMPS_Audio;

public class AnimationManager : MonoBehaviour
{
    public static AnimationManager instance;

    [Header("Public Variables")]
    public Animator UI_animator;
    public Animator StartMenu_animator;
    public Animator MapBase_animator;
    public Animator BottomMenu_animator;
    public bool isOpened = false;
    public bool mapIsVisible = false;

    [Header("Audio Related")]
    public AudioSource UI_audio_source;
    public AudioSource Amps_audio_source;
    public AudioSource Mission_audio_source;

    public UI_Audio UI_Audio;
    public App_Audio App_Audio;
    public Mission_Audio Mission_Audio;

    private Animator currentPlayingAnimator;


    private bool animationTriggered = false;



    #region Events
    [Header("Triggered Events")]
    public UnityEvent OnUIOpened = new UnityEvent();
    public UnityEvent OnUIClosed = new UnityEvent();

    #endregion

    private void Start()
    {
        instance = this;

        Amps_audio_source.PlayOneShot(App_Audio.App_RoomTone);
    }

    public void ShowBottomMenu()
    {
        BottomMenu_animator.SetBool("showBottomMenu", true);
    }


    public void ShowMap()
    {
        MapBase_animator.SetBool("showMap", true);
        Mission_audio_source.PlayOneShot(Mission_Audio.Mission_Loaded);
        //when the audio has finished playing
        animationTriggered = true;
        currentPlayingAnimator = MapBase_animator;
    }

    public void HideMap()
    {
        MapBase_animator.SetBool("showMap", false);
    }


    public void PlayStartMenuClose()
    {
        StartMenu_animator.SetBool("closeMenu", true);
        BottomMenu_animator.SetBool("showBottomMenu", true);
        UI_audio_source.PlayOneShot(UI_Audio.PositiveFeedback);
    }

    public void PlayOpenAnimation()
    {
        UI_animator.SetBool("open", false);
        OnUIOpened.Invoke();
        UI_audio_source.PlayOneShot(UI_Audio.Panel_Open);
        isOpened = true;

    }

    public void PlayCloseAnimation()
    {
        UI_animator.SetBool("open", true);
        OnUIClosed.Invoke();
        UI_audio_source.PlayOneShot(UI_Audio.Panel_Close);
        isOpened = false;

    }

    public void ToggleUIPanel()
    {
        if (isOpened)
            PlayCloseAnimation();
        else
            PlayOpenAnimation();
    }

    private bool AnimatorIsPlaying()
    {
        return currentPlayingAnimator.GetCurrentAnimatorStateInfo(0).length > currentPlayingAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;

    }

    private void Update()
    {
        if (animationTriggered)
        {
            animationTriggered = AnimatorIsPlaying();
            if (!animationTriggered)
            {
                Debug.Log("Animation finished!");
                // do something when animation is finished
                InteractionManager.instance.HandleMissionPoints();
            }
        }
    }

}
