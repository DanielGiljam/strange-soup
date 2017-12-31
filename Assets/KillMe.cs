using Assets.Menu;
using Assets.Sound;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class KillMe : MonoBehaviour
    {

        // VARIABLE INITIALIZATIONS

        public GameObject Cso;

        GameObject loadingTextObject;
        CharacterSounds cs;
        Fader loadingTextFader;
        Image image;

        // "UNITY FUNCTIONS"

        void Awake()
        {
            loadingTextObject = GameObject.Find("Loading Text");
            cs = Cso.GetComponent<CharacterSounds>();
            loadingTextFader = loadingTextObject.GetComponent<Fader>();
            image = GetComponent<Image>();
            if (loadingTextFader.MenuTransition) return;
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
        }

        void Start ()
        {
            if (!loadingTextFader.MenuTransition) return;
            cs.AudioSources[2].volume = 0;
            Invoke("WhenEverythingIsReady", (loadingTextFader.DisplayTime + loadingTextFader.TransitionTime) / 2);
        }

        // "INVOKED" FUNCTIONS
	
        void WhenEverythingIsReady ()
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
            loadingTextFader.MenuTransition = false;
            Invoke("SoundBug", ((loadingTextFader.DisplayTime + loadingTextFader.TransitionTime) / 2) + 0.5f);
        }

        void SoundBug()
        {
            cs.AudioSources[2].volume = 0.8f;
        }

    }
}
