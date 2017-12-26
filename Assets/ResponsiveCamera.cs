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

        // following variables have to do with the sizing of the camera in the Awake -method and following the character in the Update -method
        Vector2 optimalAspectRatio = new Vector2(17, 10);
        Vector2 screenAspectRatio;
        Camera cam;
        GameObject character;
        CharacterMovement cm;
        CharacterDeath cd;
        float optimalArFloat;
        float screenArFloat;
        float tripleCOffset;
	
        // "UNITY FUNCTIONS"

        void Awake()
        {

            // just fetching the corresponding components/values...
            cam = GetComponent<Camera>();
            character = GameObject.Find("Character");
            cm = character.GetComponent<CharacterMovement>();
            cd = character.GetComponent<CharacterDeath>();
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

        }

        void Update()
        {

            // determining the Contextual Character Center Offset
            if (cm.Cs.FacingRight) tripleCOffset = CharacterCenterOffset;
            if (cm.Cs.FacingLeft) tripleCOffset = -CharacterCenterOffset;

            // if -statement prevents camera from following the character into the void
            if (character.transform.position.y < cd.voidBarrier) return;

            // upperBounds, lowerBounds -setup made by Albert Nyberg
            if (character.transform.position.y >= transform.position.y + UpperBounds)
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(character.transform.position.x + tripleCOffset, character.transform.position.y - UpperBounds, -10), CameraFollowSmoothing);
            }
            else if (character.transform.position.y <= transform.position.y - LowerBounds)
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(character.transform.position.x + tripleCOffset, character.transform.position.y + LowerBounds, -10), CameraFollowSmoothing);
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(character.transform.position.x + tripleCOffset, transform.position.y, -10), CameraFollowSmoothing);
            }

        }

    }
}
