using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Character_Logic
{
    public class CharacterDeath : MonoBehaviour
    {

        // VARIABLE INITIALIZATIONS

        public float voidBarrier = -9f; // y-value below which player will die 
        public float RestartDelay = 2f; // how many seconds the "game over" text will display

        // Set in inspector!
        public GameObject YouFellIntoTheVoid; // reference to the "game over" text
        
        GameObject hud; // reference to the HUD gameobject
        CharacterMovement cm; // reference to the CharacterMovement -component

        // "UNITY FUNCTIONS"

        void Awake()
        {

            // just fetching the corresponding components and gameobjects...
            hud = GameObject.Find("HUD");
            cm = GetComponent<CharacterMovement>();

        }
	
        void Update() {

            if (!(transform.position.y < voidBarrier)) return; // nothing happens unless character is below the "void barrier"

            hud.SetActive(false); // if character is below the "void barrier" HUD is disabled...
            YouFellIntoTheVoid.SetActive(true); // ...and death text is displayed

            Invoke("Restart", RestartDelay); // reloading scene after specified time

            cm.dead = true; // setting this to true interrupts all interaction with the character, which from certain a philosphical viewpoint could be seen as what truly kills the character :'(

        }

        // MISCELLANEOUS

        void Restart()
        {

            SceneManager.LoadScene("Game");

        }

    }
}
