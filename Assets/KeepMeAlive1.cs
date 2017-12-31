using UnityEngine;

namespace Assets
{
    public class KeepMeAlive1 : MonoBehaviour
    {

        public static KeepMeAlive1 Instance;

        void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

    }
}
