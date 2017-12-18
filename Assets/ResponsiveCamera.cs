using UnityEngine;

namespace Assets
{
    public class ResponsiveCamera : MonoBehaviour
    {

        public float CameraFollowSmoothing = 0.2f;
        public float UpperBounds = 0f;
        public float LowerBounds = 2.3f;

        Vector2 optimalAspectRatio = new Vector2(17, 10);
        Vector2 screenAspectRatio;
        Camera cam;
        GameObject character;
        float optimalArFloat;
        float screenArFloat;
	
        // ReSharper disable once UnusedMember.Local
        void Awake()
        {
            cam  = GetComponent<Camera>();
            character = GameObject.Find("Character");
            screenAspectRatio = new Vector2(Screen.width, Screen.height);
            optimalArFloat = optimalAspectRatio.x / optimalAspectRatio.y;
            screenArFloat = screenAspectRatio.x / screenAspectRatio.y;
            if (screenArFloat > optimalArFloat)
            {
                cam.orthographicSize = (optimalAspectRatio.x / screenArFloat) / 2;
            }
            else
            {
                cam.orthographicSize = optimalAspectRatio.y / 2;
            }
            Debug.Log("Screen width: " + Screen.width + ", Screen height: " + Screen.height + "\nScreen aspect ratio: " + screenArFloat + ", Optimal aspect ratio: " + optimalArFloat);
        }

        // ReSharper disable once UnusedMember.Local
        void Update()
        {
            if (character.transform.position.y >= transform.position.y + UpperBounds)
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(character.transform.position.x, character.transform.position.y - UpperBounds, -10), CameraFollowSmoothing);
            }
            else if (character.transform.position.y <= transform.position.y - LowerBounds)
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(character.transform.position.x, character.transform.position.y + LowerBounds, -10), CameraFollowSmoothing);
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(character.transform.position.x, transform.position.y, -10), CameraFollowSmoothing);
            }
        }

    }
}
