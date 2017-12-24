using UnityEngine;

namespace Assets.Character_Logic
{
    public class CharacterDeath : MonoBehaviour {
	
        void Update () {
            if (transform.position.y < -9f)
            {
                // kill character, "reload" scene
            }
        }
    }
}
