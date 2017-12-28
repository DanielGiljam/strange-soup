using Assets.Character_Logic;
using UnityEngine;

namespace Assets.Sound
{
    public class CharacterSounds : MonoBehaviour
    {

        // VARIABLE INITIALIZATIONS

        public AudioSource[] AudioSources;

        GameObject character;
        CharacterMovement cm;

        bool alreadyRunning;
        bool alreadySliding;
        bool alreadyLanded;
        bool alreadyDead;

        // "UNITY FUNCTIONS"

        void Awake () {

            character = GameObject.Find("Character");
            cm = character.GetComponent<CharacterMovement>();

        }
	
        void Update () {

            // loops footstep sounds if moving
            if (cm.Cs.IsMoving && !alreadyRunning)
            {
                AudioSources[0].Play();
                alreadyRunning = true;
            }
            if (!cm.Cs.IsMoving && alreadyRunning)
            {
                if (!alreadySliding) AudioSources[0].Stop();
                alreadyRunning = false;
            }

            // sets footsteps playback rate proportional to how fast character is sprinting
            AudioSources[0].pitch = cm.ActualSprintMultiplier + 1;

            // replaces footstep sound with slide sound if character is sliding
            if (alreadyRunning && cm.Cs.IsSliding && !alreadySliding)
            {
                AudioSources[0].Stop();
                AudioSources[1].Play();
                alreadySliding = true;
            }
            if ((!alreadyRunning && alreadySliding) || (!cm.Cs.IsSliding && alreadySliding))
            {
                AudioSources[1].Stop();
                if (alreadyRunning) AudioSources[0].Play();
                alreadySliding = false;
            }

            // plays "landing sound" when player hits ground after jump (or drop)
            if (cm.Cs.GroundContact && !alreadyLanded && !(character.transform.position.x > 50))
            {
                AudioSources[2].Play();
                alreadyLanded = true;
            }
            if (!cm.Cs.GroundContact && alreadyLanded)
            {
                alreadyLanded = false;
            }

            // plays back sound effect if player falls into void
            if (cm.dead && !alreadyDead)
            {
                AudioSources[3].Play();
                alreadyDead = true;
            }

            // plays back sound effect if this other random condition occurs...
            if (character.transform.position.x > 50 && cm.Cs.GroundContact && !alreadyDead)
            {
                AudioSources[4].Play();
                alreadyDead = true;
            }

        }
    }
}
