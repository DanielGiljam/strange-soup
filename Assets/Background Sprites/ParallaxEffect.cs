using UnityEngine;

namespace Assets.Background_Sprites
{
    public class ParallaxEffect : MonoBehaviour {

        const float OnePixelInUnits = 0.0625f;

        public float HorizontalIntensity = 8;
        public float VerticalIntensity = 8;

        Camera cam;
        float camDeltaY;
        float actualHIntensity;
        float actualVIntensity;

        // ReSharper disable once UnusedMember.Local
        void Awake()
        {
            cam = Camera.main;
            actualHIntensity = (HorizontalIntensity * OnePixelInUnits) / 2;
            actualVIntensity = (VerticalIntensity * OnePixelInUnits) / 2;
        }
	
        // ReSharper disable once UnusedMember.Local
        void Update()
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + (camDeltaY * actualVIntensity));
            camDeltaY -= cam.transform.position.y;
        }

    }
}
