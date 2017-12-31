using Assets.Sound;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Character_Logic
{
    public class CharacterDeath : MonoBehaviour
    {

        // VARIABLE INITIALIZATIONS

        public float VoidBarrier = -9f; // y-value below which player will die 
        public float RestartDelay = 2f; // how many seconds the "game over" text will display

        // Set in inspector!
        public GameObject YouFellIntoTheVoid; // reference to the "game over" text
        public GameObject ThisOtherRandomText; // reference to this other random text...
        public GameObject Hud;
        public GameObject Cso;

        CharacterMovement cm; // reference to the CharacterMovement -component
        CharacterSounds cs;
        SpriteRenderer sr; // reference to the SpriteRenderer -component

        bool veryDeadNow; // prevents Update() from happening after death
        
        // "UNITY FUNCTIONS"

        void Awake()
        {

            // just fetching the corresponding components and gameobjects...
            cm = GetComponent<CharacterMovement>();
            cs = Cso.GetComponent<CharacterSounds>();
            sr = GetComponent<SpriteRenderer>();

            CancelInvoke();

        }
	
        void Update()
        {

            if (veryDeadNow) return;
            if (!(transform.position.y < VoidBarrier) && !(transform.position.x > 50f && cm.Cs.GroundContact)) return; // nothing happens unless character is below the "void barrier" or this other random condition...

            veryDeadNow = true;

            Hud.SetActive(false); // if character is below the "void barrier" HUD is disabled...
            if (!(transform.position.x > 50f)) YouFellIntoTheVoid.SetActive(true); // ...and death text is displayed
            else ThisOtherRandomText.SetActive(true);

            Invoke("Restart", RestartDelay); // reloading scene after specified time

            cm.dead = true; // setting this to true interrupts all interaction with the character, which from certain a philosphical viewpoint could be seen as what truly kills the character :'(

            if (!(transform.position.x > 50f)) return;
            cm.enabled = !cs.enabled;
            cs.AudioSources[0].Stop();
            cs.AudioSources[1].Stop();
            cs.enabled = !cs.enabled;
            sr.enabled = !sr.enabled;
        }

        // MISCELLANEOUS

        void Restart()
        {

            SceneManager.LoadScene("Game");

        }

    }
}
