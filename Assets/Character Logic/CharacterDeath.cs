using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Character_Logic
{
    public class CharacterDeath : MonoBehaviour
    {

        public float restartDelay = 2f;

        // Set in inspector!
        public GameObject deathScreen;

        GameObject hudScreen;

        void Awake()
        {
            hudScreen = GameObject.Find("HUD");
        }
	
        void Update() {
            if (transform.position.y < -9f)
            {
                hudScreen.SetActive(false);
                deathScreen.SetActive(true);
                Invoke("Restart", restartDelay);
            }
        }

        void Restart()
        {
            SceneManager.LoadScene("Game");
        }

    }
}
