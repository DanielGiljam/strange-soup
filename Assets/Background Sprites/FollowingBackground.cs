using UnityEngine;

namespace Assets.Background_Sprites
{
    public class FollowingBackground : MonoBehaviour {

        Camera cam;

        // ReSharper disable once UnusedMember.Local
        void Awake()
        {
            cam = Camera.main;
        }

        // ReSharper disable once UnusedMember.Local
        void Update()
        {
            transform.position = new Vector2(cam.transform.position.x, cam.transform.position.y);
        }

    }
}
