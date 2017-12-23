using UnityEngine;

namespace Assets.Background_Sprites
{
    public class CustomParallaxEffect : MonoBehaviour
    {

        const float OnePixelInTextureOffsetUnits = (1f / 16f) * 0.0625f;

        public float HorizontalIntensity = 8;

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
        }

        void HorizontalParallax()
        {
            material.SetTextureOffset("_MainTex", new Vector2(cam.transform.position.x * (HorizontalIntensity * OnePixelInTextureOffsetUnits), 0));
        }

    }
}
