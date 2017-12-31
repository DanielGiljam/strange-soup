using UnityEngine;

namespace Assets
{
    public class KeepMeAlive2 : MonoBehaviour
    {

        public static KeepMeAlive2 Instance;

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
