using UnityEngine;

namespace AMPS_Audio
{
    [System.Serializable]
    public class UI_Audio
    {
        public AudioClip Panel_Open;
        public AudioClip Panel_Close;
        public AudioClip PositiveFeedback;
        public AudioClip NegativeFeedback;
    }

    [System.Serializable]
    public class App_Audio
    {
        public AudioClip App_Loaded;
        public AudioClip App_Intro;
        public AudioClip App_Menu;
    }

    [System.Serializable]
    public class Mission_Audio
    {
        public AudioClip Mission_Loaded;
        public AudioClip Mission_UnLoaded;
    }

}