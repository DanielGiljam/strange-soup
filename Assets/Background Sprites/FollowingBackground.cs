using UnityEngine;

namespace Assets.Background_Sprites
{
    public class FollowingBackground : MonoBehaviour {

        Camera cam;

        void Awake()
        {
            cam = Camera.main;
        }

        void Update()
        {
            transform.position = new Vector2(cam.transform.position.x, cam.transform.position.y);
        }

    }
}
