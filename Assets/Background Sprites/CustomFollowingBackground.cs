using UnityEngine;

namespace Assets.Background_Sprites
{
    public class CustomFollowingBackground : MonoBehaviour
    {


        Camera cam;

        void Awake()
        {
            cam = Camera.main;
        }

        void Update()
        {
            transform.position = new Vector2(cam.transform.position.x, transform.position.y);
        }

    }
}
