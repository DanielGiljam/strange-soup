using UnityEngine;

namespace Assets
{
    public class ResponsiveCamera : MonoBehaviour
    {

        // adjust amount of smoothing in camera's movements. Scale 0-1. Smaller value means more smoothing. 1 means no smoothing. Do not set to 0.
        public float CameraFollowSmoothing = 0.2f;

        // how much vertical character movement the camera will tolerate until following the movement
        public float UpperBounds = 0;
        public float LowerBounds = 2.3f;

        // following variables have to do with the sizing of the camera in the Awake -method
        Vector2 optimalAspectRatio = new Vector2(17, 10);
        Vector2 screenAspectRatio;
        Camera cam;
        GameObject character;
        float optimalArFloat;
        float screenArFloat;
	
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
