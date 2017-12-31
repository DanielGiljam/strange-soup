using Assets.Sound;
using UnityEngine;
using UnityEngine.Audio;

namespace Assets
{
    public class WorldEvent2 : MonoBehaviour {

        // VARIABLE INITIALIZATIONS

        // NOTE! Set these in inspector!
        public GameObject Cso;
        public Sprite TheGreatNewSprite;
        public GameObject DoublePlatform;
        public GameObject UpsideDownPlatform;
        public GameObject Background;
        public GameObject ParallaxLayer;
        public AudioMixerGroup Clean;
        public AudioMixerGroup Affected;

        Camera cam;
        GameObject am;
        GameObject character;
        CharacterSounds chSounds;
        AudioSource we2Sound;
        AudioSource[] amSources;


        SpriteRenderer chSprite;
        SpriteRenderer[] dpSprites;
        SpriteRenderer[] udpSprites;
        SpriteRenderer[] bgSprites;
        SpriteRenderer[] plSprites;
        SpriteRenderer bgNoSky;

        Sprite[] dpOldSprites;
        Sprite[] udpOldSprites;
        int oldSpriteIterator = 0;

        float[] oldVolumes;
        int oldVolumeIterator = 0;

        float interpolationValue;

        Color thatBackgroundColor;

        readonly Color thatOtherBackgroundColor = new Color(171f / 255f, 106f / 255f, 140 / 255f);
        readonly Color solidBlack = new Color(0, 0, 0);
        readonly Color black = new Color(0, 0, 0, 0);        
        readonly Color white = new Color(1f, 1f, 1f, 1f);

        bool eventTriggered;

        // "UNITY FUNCTIONS"

        void Awake () {

            // just fetching the corresponding gameobjects/components...
            cam = Camera.main.GetComponent<Camera>();
            am = GameObject.Find("AudioManager");
            character = GameObject.Find("Character");
            chSounds = Cso.GetComponent<CharacterSounds>();
            we2Sound = GetComponent<AudioSource>();
            amSources = am.GetComponentsInChildren<AudioSource>();

            chSprite = character.GetComponent<SpriteRenderer>();
            dpSprites = DoublePlatform.GetComponentsInChildren<SpriteRenderer>();
            udpSprites = UpsideDownPlatform.GetComponentsInChildren<SpriteRenderer>();
            bgSprites = Background.GetComponentsInChildren<SpriteRenderer>();
            plSprites = ParallaxLayer.GetComponentsInChildren<SpriteRenderer>();
            bgNoSky = GameObject.Find("parallax-mountain-bg-no-sky").GetComponent<SpriteRenderer>();

            dpOldSprites = new Sprite[dpSprites.Length];
            udpOldSprites = new Sprite[udpSprites.Length];
            oldVolumes = new float[amSources.Length];

        }
	
        void Update ()
        {

            Enter();
            Exit();

            if (!eventTriggered) return;

            interpolationValue = (character.transform.position.x + 40f) / (-59f + 40f);
            if (interpolationValue > 1) interpolationValue = 1;
            if (interpolationValue < 0) interpolationValue = 0;

            cam.backgroundColor = Color.Lerp(solidBlack, thatOtherBackgroundColor, interpolationValue);
            bgNoSky.color = Color.Lerp(black, white, interpolationValue);

            amSources[0].volume = (oldVolumes[0] * interpolationValue) * 0.75f;

        }

        // EVENT FUNCTIONS

        void Enter()
        {            

            if (eventTriggered) return;
            if (!(character.transform.position.x < -30f)) return; // nothing happens unless conditions are met

            eventTriggered = true;

            we2Sound.Play();

            foreach (var audioSource in amSources)
            {
                oldVolumes[oldVolumeIterator] = audioSource.volume;
                oldVolumeIterator++;
                audioSource.volume = 0;
            }
            oldVolumeIterator = 0;

            chSprite.color = white;

            thatBackgroundColor = cam.backgroundColor;
            cam.backgroundColor = solidBlack;

            foreach (var spriteRenderer in dpSprites)
            {                
                dpOldSprites[oldSpriteIterator] = spriteRenderer.sprite;
                oldSpriteIterator++;
                spriteRenderer.sprite = TheGreatNewSprite;                
            }
            oldSpriteIterator = 0;
            foreach (var spriteRenderer in udpSprites)
            {
                udpOldSprites[oldSpriteIterator] = spriteRenderer.sprite;
                oldSpriteIterator++;
                spriteRenderer.sprite = TheGreatNewSprite;
            }
            oldSpriteIterator = 0;

            foreach (var spriteRenderer in bgSprites) spriteRenderer.color = black;
            foreach (var spriteRenderer in plSprites) spriteRenderer.color = black;

            chSounds.AudioSources[0].outputAudioMixerGroup = Affected;
            chSounds.AudioSources[1].outputAudioMixerGroup = Affected;
            chSounds.AudioSources[2].outputAudioMixerGroup = Affected;

        }

        void Exit()
        {

            if (!eventTriggered) return;
            if (!(character.transform.position.x > -28f)) return; // nothing happens unless conditions are met

            eventTriggered = false;

            foreach (var audioSource in amSources)
            {
                audioSource.volume = oldVolumes[oldVolumeIterator];
                oldVolumeIterator++;
            }
            oldVolumeIterator = 0;

            chSprite.color = solidBlack;

            cam.backgroundColor = thatBackgroundColor;

            foreach (var spriteRenderer in dpSprites)
            {
                spriteRenderer.sprite = dpOldSprites[oldSpriteIterator];
                oldSpriteIterator++;
            }
            oldSpriteIterator = 0;
            foreach (var spriteRenderer in udpSprites)
            {
                spriteRenderer.sprite = udpOldSprites[oldSpriteIterator];
                oldSpriteIterator++;
            }
            oldSpriteIterator = 0;

            foreach (var spriteRenderer in bgSprites) spriteRenderer.color = white;
            foreach (var spriteRenderer in plSprites) spriteRenderer.color = white;

            chSounds.AudioSources[0].outputAudioMixerGroup = Clean;
            chSounds.AudioSources[1].outputAudioMixerGroup = Clean;
            chSounds.AudioSources[2].outputAudioMixerGroup = Clean;

        }

    }
}
