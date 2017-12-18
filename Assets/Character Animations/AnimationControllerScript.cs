using Assets.Character_Controls;
using UnityEngine;

namespace Assets.Character_Animations
{
    public class AnimationControllerScript : MonoBehaviour
    {

        Animator animator;
        CharacterMovement charMov;

        // ReSharper disable once UnusedMember.Local
        void Awake ()
        {
            animator = GetComponent<Animator>();
            charMov = GetComponent<CharacterMovement>();
        }
	
        // ReSharper disable once UnusedMember.Local
        void Update () {
		    animator.SetBool("FacingRight", charMov.FacingRight);
            animator.SetBool("FacingLeft", charMov.FacingLeft);
            animator.SetBool("IsMoving", charMov.IsMoving);
            animator.SetBool("IsSprinting", charMov.IsSprinting);
            animator.SetBool("IsJumping", charMov.IsJumping);
            animator.SetBool("GroundContact", charMov.GroundContact);
        }

    }
}
