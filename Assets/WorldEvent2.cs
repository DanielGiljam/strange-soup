using Assets.Sound;
using UnityEngine;
using UnityEngine.Audio;

namespace Assets
{
    public class WorldEvent2 : MonoBehaviour {

        // VARIABLE INITIALIZATIONS

        // NOTE! Set these in inspector!
        public Sprite TheGreatNewSprite;
        public GameObject DoublePlatform;
        public GameObject UpsideDownPlatform;
        public GameObject Background;
        public GameObject ParallaxLayer;
        public AudioSource We2Sound1;
        public AudioSource We2Sound2;
        public AudioMixerGroup Clean;
        public AudioMixerGroup Affected;

        Camera cam;
        CharacterSounds chSounds;
        GameObject character;
        

        SpriteRenderer chSprite;
        SpriteRenderer[] dpSprites;
        SpriteRenderer[] udpSprites;
        SpriteRenderer[] bgSprites;
        SpriteRenderer[] plSprites;
        SpriteRenderer bgNoSky;

        Sprite[] dpOldSprites;
        Sprite[] udpOldSprites;
        int oldSpriteIterator = 0;

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
            chSounds = GameObject.Find("Sounds").GetComponent<CharacterSounds>();
            character = GameObject.Find("Character");
            bgNoSky = GameObject.Find("parallax-mountain-bg-no-sky").GetComponent<SpriteRenderer>();

            chSprite = character.GetComponent<SpriteRenderer>();
            dpSprites = DoublePlatform.GetComponentsInChildren<SpriteRenderer>();
            udpSprites = UpsideDownPlatform.GetComponentsInChildren<SpriteRenderer>();
            bgSprites = Background.GetComponentsInChildren<SpriteRenderer>();
            plSprites = ParallaxLayer.GetComponentsInChildren<SpriteRenderer>();

            dpOldSprites = new Sprite[dpSprites.Length];
            udpOldSprites = new Sprite[udpSprites.Length];

        }
	
        void Update ()
        {

            Enter();
            Exit();

            if (!eventTriggered) return;

            interpolationValue = (cam.transform.position.x + 36.6f) / (-53.3f + 36.6f);
            if (interpolationValue > 1) interpolationValue = 1;
            if (interpolationValue < 0) interpolationValue = 0;

            cam.backgroundColor = Color.Lerp(solidBlack, thatOtherBackgroundColor, interpolationValue);
            bgNoSky.color = Color.Lerp(black, white, interpolationValue);

        }

        // EVENT FUNCTIONS

        void Enter()
        {            

            if (eventTriggered) return;
            if (!(character.transform.position.x < -30f)) return; // nothing happens unless conditions are met

            eventTriggered = true;

            We2Sound1.Stop();
            We2Sound2.Play();

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

            We2Sound1.Play();

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
