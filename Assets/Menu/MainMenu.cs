using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Menu
{
    public class MainMenu : MonoBehaviour {

        public void Play()
        {
            SceneManager.LoadScene("Game");
        }

        public void Quit()
        {
            Debug.Log("QUIT");
            Application.Quit();
        }

    }
}
