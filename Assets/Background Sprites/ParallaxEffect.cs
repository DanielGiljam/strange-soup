using UnityEngine;

namespace Assets.Background_Sprites
{
    public class ParallaxEffect : MonoBehaviour {

        const float OnePixelInUnits = 0.0625f;
        const float OnePixelInTextureOffsetUnits = (1f / 272f) * 0.0625f;

        // VARIABLE INITIALIZATIONS

        public int HorizontalIntensity = 8;
        public int VerticalIntensity = 8;
        public float VerticalOffset;

        Camera cam;
        Material material;

        // "UNITY FUNCTIONS"

        void Awake()
        {

            // just fetching the corresponding components/objects...
            cam = Camera.main;
            material = GetComponent<Renderer>().material;

        }
	
        void Update()
        {

            HorizontalParallax();
            VerticalParallax();

        }

        // FUNCTIONS

        void HorizontalParallax()
        {

            material.SetTextureOffset("_MainTex", new Vector2(cam.transform.position.x * (HorizontalIntensity * OnePixelInTextureOffsetUnits), 0));

        }

        void VerticalParallax()
        {

            if (cam.transform.position.y > 0)
            {
                transform.position = new Vector2(transform.position.x, (cam.transform.position.y * (VerticalIntensity * OnePixelInUnits)) - VerticalOffset);
            }

        }

    }
}
