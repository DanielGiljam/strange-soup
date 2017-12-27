using UnityEngine;

namespace Assets
{
    public class WorldEvent1 : MonoBehaviour {

        // VARIABLE INITIALIZATIONS

        // NOTE! Set these in inspector!
        public GameObject FakePlatform;
        public GameObject SoupBowl;

        GameObject character;
        AudioSource we1Sound;

        bool eventTriggered;

        // "UNITY FUNCTIONS"

        void Awake () {

            // just fetching the corresponding gameobjects/components...
		    character = GameObject.Find("Character");
            we1Sound = GetComponent<AudioSource>();

        }
	
        void Update ()
        {

            if (eventTriggered) return;
            if (!(character.transform.position.x > 50f)) return; // nothing happens unless conditions are met

            eventTriggered = true;

            FakePlatform.SetActive(false);
            we1Sound.Play();
            SoupBowl.SetActive(true);

        }
    }
}
