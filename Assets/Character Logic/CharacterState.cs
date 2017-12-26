using System;
using System.Runtime.InteropServices;
using Assets.Character_Animations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Character_Logic
{
    public class CharacterState
    {

        // VARIABLE INITIALIZATIONS


        readonly GameObject ch; // reference to the character
        readonly CharacterMovement cm; // reference to the character's CharacterMovement -component
        readonly AnimationControllerScript acs; // instance of the AnimationControllerState class


        Vector2 pointA; // corresponding gdrPoint relative to the character's position. Updated every time Unity's physics engine updates
        Vector2 pointB; // corresponding gdrPoint relative to the character's position. Updated every time Unity's physics engine updates


        // CONSTRUCTOR


        public CharacterState(GameObject character)
        {

            // just fetching the corresponding components and gameobjects...
            ch = character;
            cm = ch.GetComponent<CharacterMovement>();
            acs = new AnimationControllerScript(character);

        }


        // CHARACTER STATES


        [HideInInspector]
        public bool GroundContact;
        bool SetGroundContact
        {
            set
            {
                if (GroundContact == value) return;
                switch (value)
                {
                    case true:
                        GroundContact = true;
                        Debug.Log("Character on ground\nGroundContact set to true");
                        break;
                    case false:
                        GroundContact = false;
                        Debug.Log("Character not on ground\nGroundContact set to false");
                        break;
                }
            }
        }

        [HideInInspector]
        public bool Idle;
        bool SetIdle
        {
            set
            {
                if (Idle == value) return;
                switch (value)
                {
                    case true:
                        Idle = true;
                        Debug.Log("Character is idle\nIdle set to true");
                        break;
                    case false:
                        Idle = false;
                        Debug.Log("Character not idle\nIdle set to false");
                        break;
                }
            }
        }

        [HideInInspector]
        public bool IsMoving;
        bool SetIfMoving
        {
            set
            {
                if (IsMoving == value) return;
                switch (value)
                {
                    case true:
                        IsMoving = true;
                        Debug.Log("Character started moving\nIsMoving set to true");
                        break;
                    case false:
                        IsMoving = false;
                        Debug.Log("Character stopped moving\nIsMoving set to false");
                        break;
                }
            }
        }

        [HideInInspector]
        public bool IsSprinting;
        bool SetIfSprinting
        {
            set
            {
                if (IsSprinting == value) return;
                switch (value)
                {
                    case true:
                        IsSprinting = true;
                        Debug.Log("Character started sprinting\nIsSprinting set to true");
                        break;
                    case false:
                        IsSprinting = false;
                        Debug.Log("Character stopped sprinting\nIsSprinting set to false");
                        break;
                }
            }
        }

        [HideInInspector]
        public bool IsSliding;
        bool SetIfSliding
        {
            set
            {
                if (IsSliding == value) return;
                switch (value)
                {
                    case true:
                        IsSliding = true;
                        Debug.Log("Character started sliding\nIsSliding set to true");
                        break;
                    case false:
                        IsSliding = false;
                        Debug.Log("Character stopped sliding\nIsSliding set to false");
                        break;
                }
            }
        }

        [HideInInspector]
        public bool IsJumping;
        bool SetIfJumping
        {
            set
            {
                if (IsJumping == value) return;
                if (value) Debug.Log("Character jumped\nIsJumping set to true 'til character lands again");
                IsJumping = value;
            }
        }

        [HideInInspector]
        public bool FacingRight = true;
        [HideInInspector]
        public bool FacingLeft;

        string SetFacingDirection
        {
            set
            {
                switch (value)
                {
                    case "right":
                        if (FacingRight) break;
                        FacingRight = true;
                        FacingLeft = false;
                        Debug.Log("Character facing right\nMovingRight set to true");
                        break;
                    case "left":
                        if (FacingLeft) break;
                        FacingLeft = true;
                        FacingRight = false;
                        Debug.Log("Character facing left\nMovingLeft set to true");
                        break;
                }
            }
        }


        // CHARACTER STATE FUNCTIONS


        public void CharacterMovementState()
        {
            
            pointA = Vector2Addition(ch.transform.position, cm.GdrPointA);
            pointB = Vector2Addition(ch.transform.position, cm.GdrPointB);

            SetGroundContact = Physics2D.OverlapArea(pointA, pointB, cm.LayerMask);

            if (GroundContact && !(Math.Abs(cm.CurrentVelocity.x) > 0)) SetIdle = true;
            else SetIdle = false;

            if (GroundContact && Math.Abs(cm.CurrentVelocity.x) > cm.MovementSpeed * cm.MovementThreshold) SetIfMoving = true;
            else SetIfMoving = false;

            if (IsMoving && cm.SprintInputValue > 0) SetIfSprinting = true;
            else SetIfSprinting = false;

            if (cm.sliding) SetIfSliding = true;
            else SetIfSliding = false;

            if (GroundContact && cm.JumpInputValue > 0) SetIfJumping = true;
            if (IsJumping && GroundContact) SetIfJumping = false;

            if (cm.CurrentVelocity.x > 0) SetFacingDirection = "right";
            else if (cm.CurrentVelocity.x < 0) SetFacingDirection = "left";

            acs.UpdateAnimations(FacingRight, FacingLeft, IsMoving, IsSprinting, IsSliding, IsJumping, GroundContact, Idle, cm.ActualSprintMultiplier);

        }


        // MISCELLANEOUS


        static Vector2 Vector2Addition(Vector2 firstVector, Vector2 secondVector)
        {

            firstVector.x += secondVector.x;
            firstVector.y += secondVector.y;
            return firstVector;

        }

    }
}
