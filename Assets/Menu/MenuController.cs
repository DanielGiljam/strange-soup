using UnityEngine;
using UnityEngine.UI;

namespace Assets.Menu
{
    public class MenuController : MonoBehaviour
    {

        Button[] menuButtons;

        void Awake ()
        {
            menuButtons = GetComponentsInChildren<Button>();
        }
	
        void Update () {
		
        }
    }
}
