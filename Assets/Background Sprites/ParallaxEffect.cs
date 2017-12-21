using UnityEngine;

namespace Assets.Background_Sprites
{
    public class ParallaxEffect : MonoBehaviour {

        const float OnePixelInUnits = 1f / 272f * 0.0625f;
        const float OnePixelInTextureOffsetUnits = 1f / 272f * 0.0625f;

        public float HorizontalIntensity = 8;
        public float VerticalIntensity = 8;

        Camera cam;
        Material material;


        void Awake()
        {
            cam = Camera.main;
            material = GetComponent<Renderer>().material;
        }
	
        void Update()
        {
            HorizontalParallax();
            VerticalParallax();
        }

        void HorizontalParallax()
        {
            material.SetTextureOffset("_MainTex", new Vector2(cam.transform.position.x * (HorizontalIntensity * OnePixelInTextureOffsetUnits), 0));
        }

        void VerticalParallax()
        {
            transform.position = new Vector2(transform.position.x,
                cam.transform.position.y * (VerticalIntensity * OnePixelInUnits));
        }

    }
}
