using UnityEngine;

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

        Camera cam;
        GameObject character;
        AudioSource we2Sound;

        SpriteRenderer[] dpSprites;
        SpriteRenderer[] udpSprites;
        SpriteRenderer[] bgSprites;
        SpriteRenderer[] plSprites;

        Sprite[] dpOldSprites;
        Sprite[] updOldSprites;
        int oldSpriteIterator = 0;

        Color thatBackgroundColor;

        readonly Color black = new Color(0, 0, 0, 0);
        readonly Color white = new Color(255, 255, 255, 255);

        bool eventTriggered;

        // "UNITY FUNCTIONS"

        void Awake () {

            // just fetching the corresponding gameobjects/components...
            cam = Camera.main.GetComponent<Camera>();
            character = GameObject.Find("Character");
            we2Sound = GetComponent<AudioSource>();

            dpSprites = DoublePlatform.GetComponentsInChildren<SpriteRenderer>();
            udpSprites = UpsideDownPlatform.GetComponentsInChildren<SpriteRenderer>();
            bgSprites = Background.GetComponentsInChildren<SpriteRenderer>();
            plSprites = ParallaxLayer.GetComponentsInChildren<SpriteRenderer>();

            dpOldSprites = new Sprite[dpSprites.Length];
            updOldSprites = new Sprite[udpSprites.Length];

        }
	
        void Update ()
        {

            Enter();
            Exit();

        }

        // EVENT FUNCTIONS

        void Enter()
        {

            if (eventTriggered) return;
            if (!(character.transform.position.x < -30f)) return; // nothing happens unless conditions are met

            eventTriggered = true;

            we2Sound.Play();

            thatBackgroundColor = cam.backgroundColor;
            cam.backgroundColor = white;

            foreach (var spriteRenderer in dpSprites)
            {                
                dpOldSprites[oldSpriteIterator] = spriteRenderer.sprite;
                oldSpriteIterator++;
                spriteRenderer.sprite = TheGreatNewSprite;                
            }
            oldSpriteIterator = 0;
            foreach (var spriteRenderer in udpSprites)
            {
                updOldSprites[oldSpriteIterator] = spriteRenderer.sprite;
                oldSpriteIterator++;
                spriteRenderer.sprite = TheGreatNewSprite;
            }
            oldSpriteIterator = 0;

            foreach (var spriteRenderer in bgSprites) spriteRenderer.color = black;
            foreach (var spriteRenderer in plSprites) spriteRenderer.color = black;

        }

        void Exit()
        {

            if (!eventTriggered) return;
            if (!(character.transform.position.x > -28f)) return; // nothing happens unless conditions are met

            eventTriggered = false;

            cam.backgroundColor = thatBackgroundColor;

            foreach (var spriteRenderer in dpSprites)
            {
                spriteRenderer.sprite = dpOldSprites[oldSpriteIterator];
                oldSpriteIterator++;
            }
            oldSpriteIterator = 0;
            foreach (var spriteRenderer in udpSprites)
            {
                spriteRenderer.sprite = updOldSprites[oldSpriteIterator];
                oldSpriteIterator++;
            }
            oldSpriteIterator = 0;

            foreach (var spriteRenderer in bgSprites) spriteRenderer.color = white;
            foreach (var spriteRenderer in plSprites) spriteRenderer.color = white;

        }

    }
}
