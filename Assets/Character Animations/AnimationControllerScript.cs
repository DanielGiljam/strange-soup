using UnityEngine;

namespace Assets.Character_Animations
{
    public class AnimationControllerScript
    {

        // VARIABLE INITIALIZATIONS
        readonly Animator animator;

        // CONSTRUCTOR
        public AnimationControllerScript(GameObject character)
        {
            // just fetching the corresponding components...
            animator = character.GetComponent<Animator>();
        }
	
        // MAIN CONTENT
        public void UpdateAnimations(bool facingRight, bool facingLeft, bool isMoving, bool isSprinting, bool isSliding, bool isJumping, bool groundContact, bool idle, float actualSprintMultiplier) {
            // updating all the conditions that the animation state transitions rely upon...
		    animator.SetBool("FacingRight", facingRight);
            animator.SetBool("FacingLeft", facingLeft);
            animator.SetBool("IsMoving", isMoving);
            animator.SetBool("IsSprinting", isSprinting);
            animator.SetBool("IsSliding", isSliding);
            animator.SetBool("IsJumping", isJumping);
            animator.SetBool("GroundContact", groundContact);
            animator.SetBool("Idle", idle);
            animator.speed = actualSprintMultiplier + 1;
        }

    }
}
