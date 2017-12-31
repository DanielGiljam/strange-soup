using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Menu
{
    public class MainMenu : MonoBehaviour
    {

        // VARIABLE INITIALIZATIONS

        public float FadingTime = 1f;
        public float TextDisplayTime = 3f;
        public GameObject BlackScreenObject;
        public GameObject MainMenuObject1;
        public GameObject MainMenuObject2;

        GameObject loadingTextObject;
        Image blackScreenImage;
        Fader blackScreen;
        Fader loadingText;

        // "UNITY FUNCTIONS"

        void Awake()
        {
            loadingTextObject = GameObject.Find("Loading Text");
            blackScreenImage = BlackScreenObject.GetComponent<Image>();
            blackScreen = BlackScreenObject.GetComponent<Fader>();
            loadingText = loadingTextObject.GetComponent<Fader>();
        }

        void Start()
        {
            blackScreen.FadeOutWithParams(FadingTime);
        }

        // MENU BUTTON -TRIGGERED FUNCTIONS

        public void Play()
        {
            if (blackScreen.MenuTransition) return;
            MainMenuObject1.SetActive(false);
            MainMenuObject2.SetActive(false);
            blackScreenImage.color = new Color(blackScreenImage.color.r, blackScreenImage.color.g, blackScreenImage.color.b, 1);
            loadingText.FadeInAndOut(FadingTime, TextDisplayTime);
            Invoke("LoadGame", (TextDisplayTime + FadingTime) / 2);
        }

        public void Quit()
        {
            Debug.Log("QUIT");
            Application.Quit();
        }

        // "INVOKED" FUNCTIONS

        void LoadGame()
        {
            SceneManager.LoadScene("Game");
        }

    }
}
