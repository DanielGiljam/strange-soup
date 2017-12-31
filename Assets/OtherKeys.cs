using Assets.Character_Logic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets
{
    public class OtherKeys : MonoBehaviour
    {

        // VARIABLE INITIALIZATIONS

        public GameObject Hud; // NOTE! Set in inspector!

        CharacterMovement cm;
        GameObject am;
        AudioSource[] amSources;

        float[] resetVolumes;
        int resetVolumesIterator = 0;

        // "UNITY FUNCTIONS"

        void Awake()
        {
            cm = GameObject.Find("Character").GetComponent<CharacterMovement>();
            am = GameObject.Find("AudioManager");
            amSources = am.GetComponentsInChildren<AudioSource>();
            resetVolumes = new float[amSources.Length];
            foreach (var audioSource in amSources)
            {
                resetVolumes[resetVolumesIterator] = audioSource.volume;
                resetVolumesIterator++;
            }
            resetVolumesIterator = 0;
        }
	
        void Update ()
        {
            if (cm.dead) return;
		    if (Input.GetKeyDown(KeyCode.H)) Hud.SetActive(!Hud.activeSelf);
            if (!Input.GetKeyDown(KeyCode.Escape)) return;
            foreach (var audioSource in amSources)
            {
                audioSource.volume = resetVolumes[resetVolumesIterator];
                resetVolumesIterator++;
            }
            SceneManager.LoadScene("Menu");
        }
    }
}
