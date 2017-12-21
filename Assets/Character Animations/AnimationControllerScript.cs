using Assets.Character_Logic;
using UnityEngine;

namespace Assets.Character_Animations
{
    public class AnimationControllerScript : MonoBehaviour
    {

        Animator animator;
        CharacterMovement cm;
        CharacterState cs;

        void Awake()
        {
            animator = GetComponent<Animator>();
            cm = GetComponent<CharacterMovement>();
            cs = GetComponent<CharacterState>();
        }
	
        void Update() {
		    animator.SetBool("FacingRight", cs.FacingRight);
            animator.SetBool("FacingLeft", cs.FacingLeft);
            animator.SetBool("IsMoving", cs.IsMoving);
            animator.SetBool("IsSprinting", cs.IsSprinting);
            animator.SetBool("IsJumping", cs.IsJumping);
            animator.SetBool("GroundContact", cs.GroundContact);
            animator.speed = cm.ActualSprintMultiplier + 1;
        }

    }
}
