using UnityEngine;

namespace Assets.Background_Sprites
{
    public class RotatedParallaxEffect : MonoBehaviour
    {

        const float OnePixelInUnits = 0.0625f;

        // VARIABLE INITIALIZATIONS

        public int Intensity = 8;
        public float Offset;

        Camera cam;

        // "UNITY FUNCTIONS"

        void Awake()
        {

            // just fetching the corresponding components/objects...
            cam = Camera.main;

        }

        void Update()
        {

            transform.position = new Vector2(-(cam.transform.position.x + 53.3f) * (Intensity * OnePixelInUnits) - Offset - 55f, transform.position.y);

        }

    }
}
