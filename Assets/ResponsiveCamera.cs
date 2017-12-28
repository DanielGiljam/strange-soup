using Assets.Character_Logic;
using UnityEngine;

namespace Assets
{
    public class ResponsiveCamera : MonoBehaviour
    {

        // VARIABLE INITIALIZATIONS

        // adjust amount of smoothing in camera's movements. Scale 0-1. Smaller value means more smoothing. 1 means no smoothing. Do not set to 0.
        public float CameraFollowSmoothing = 0.2f;

        // how much vertical character movement the camera will tolerate until following the movement
        public float UpperBounds = -1.15f;
        public float LowerBounds = 2.3f;

        // how far from the center the character will be, in the opposite direction of which the character is facing
        public float CharacterCenterOffset = 3.2f;

        // how much below y-axis camera is allowed to go
        public float MaxCameraY = 5f;
        
        // following variables have to do with the sizing of the camera in the Awake -method and following the character in the Update -method
        Vector2 optimalAspectRatio = new Vector2(17, 10);
        Vector2 screenAspectRatio;
        Camera cam;
        GameObject character;
        GameObject gameWorldBorderObject;
        CharacterMovement cm;
        float optimalArFloat;
        float screenArFloat;
        float tripleCOffset;
        float actualUpperBounds;
        float actualLowerBounds;
        float gameWorldBorder;
	
        // "UNITY FUNCTIONS"

        void Awake()
        {

            // just fetching the corresponding components/values...
            cam = GetComponent<Camera>();
            character = GameObject.Find("Character");
            gameWorldBorderObject = GameObject.Find("Game World Border");
            cm = character.GetComponent<CharacterMovement>();
            screenAspectRatio = new Vector2(Screen.width, Screen.height);

            // determining and setting camera size
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

            // determining game world border
            gameWorldBorder = gameWorldBorderObject.transform.position.x + (gameWorldBorderObject.transform.lossyScale.x / 2) + (cam.orthographicSize * screenArFloat);

        }

        void Update()
        {

            // determining the Contextual Character Center Offset
            if (cm.Cs.FacingRight) tripleCOffset = CharacterCenterOffset;
            if (cm.Cs.FacingLeft) tripleCOffset = -CharacterCenterOffset;

            // refershing "bounds" based on character position
            if (character.transform.position.x > 0)
            {
                actualUpperBounds = UpperBounds + (character.transform.position.x / 10);
                actualLowerBounds = LowerBounds;
            }
            else
            {
                actualUpperBounds = UpperBounds;
                actualLowerBounds = LowerBounds;
            }
            if (character.transform.position.x > 40) actualLowerBounds = LowerBounds / 10;
           
            // if -statement prevents camera from following the character into the void
            if (transform.position.y < -MaxCameraY) return;

            // if -statement prevents camera from crossing the game world border
            if (transform.position.x < gameWorldBorder)
            {
                if (!(character.transform.position.x > gameWorldBorder + CharacterCenterOffset)) return;
            }

            // upperBounds, lowerBounds -setup made by Albert Nyberg
            if (character.transform.position.y >= transform.position.y + actualUpperBounds)
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(character.transform.position.x + tripleCOffset, character.transform.position.y - actualUpperBounds, -10), CameraFollowSmoothing);
            }
            else if (character.transform.position.y <= transform.position.y - actualLowerBounds)
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(character.transform.position.x + tripleCOffset, character.transform.position.y + actualLowerBounds, -10), CameraFollowSmoothing);
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(character.transform.position.x + tripleCOffset, transform.position.y, -10), CameraFollowSmoothing);
            }            

        }

    }
}
