using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Menu
{
    public class Fader : MonoBehaviour
    {

        // VARIABLE INITIALIZATIONS

        public Graphic GraphicSlot;

        [HideInInspector]
        public float TransitionTime;
        [HideInInspector]
        public float DisplayTime;
        [HideInInspector]
        public bool MenuTransition;

        // FADING FUNCTIONS

        public void FadeInAndOut(float transTime, float dispTime)
        {
            StartCoroutine(FadeToFullAlpha(transTime, GraphicSlot));
            TransitionTime = transTime;
            DisplayTime = dispTime;
            Invoke("FadeOut", dispTime);
            MenuTransition = true;
        }

        public void FadeOutWithParams(float transitionTime)
        {
            StartCoroutine(FadeToZeroAlpha(transitionTime, GraphicSlot));
            MenuTransition = true;
            Invoke("MenuTransitionIsFalse", transitionTime);
        }

        // "INVOKED" FUNCTIONS

        void FadeOut()
        {
            StartCoroutine(FadeToZeroAlpha(TransitionTime, GraphicSlot));
        }

        void MenuTransitionIsFalse()
        {
            MenuTransition = false;
        }

        // FADING ALGORITHMS

        static IEnumerator FadeToFullAlpha(float t, Graphic i)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
            while (i.color.a < 1.0f)
            {
                i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
                yield return null;
            }
        }

        public IEnumerator FadeToZeroAlpha(float t, Graphic i)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
            while (i.color.a > 0.0f)
            {
                i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
                yield return null;
            }
        }

    }
}
