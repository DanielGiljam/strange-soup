using System;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

namespace Assets.Character_Logic
{
    public class CharacterMovement : MonoBehaviour
    {

        const float OnePixelInUnits = 0.0625f;


        // VARIABLE INITIALIZATIONS


        public int MovementSpeed = 5; // this times (*) horizontalInputValue is how fast the character moves horizontally
        public float MovementThreshold = 0.2f; // this times MovementSpeed determines how fast the character needs to move horizontally in order for it to "officially" count as moving, eg. for running animations to enable
        public float SprintMultiplier = 0.5f; // this times sprintInputValue is how much faster the character moves horizontally when sprinting
        public int JumpForce = 5; // represents how high the character will be able to jump. Only takes change after game has been restarted. Cannot be zero.
        public float SprintJumpEffectAmount = 0.25f; // value between 0-1 of how much sprinting can affect jumping
        public int SprintJumpEffectChargeTime = 150; // determines how quickly sprinting will affect jumping. Cannot be zero.

        // NOTE! Values have to be set in the Inspector! Any assigned values only serve as placeholders
        public Vector2 GroundDetectionRectPosition = Vector2.zero; // center of the area used by the Physics2D.OverlapArea -function for detecting when the character is grounded. Only takes change in game after game has been restarted.
        public Vector2 GroundDetectionRectSize = new Vector2(1, 1); // width and height of the area used by the Physics2D.OverlapArea -function for detecting when the character is grounded. Only takes change in game after game has been restarted
        public LayerMask LayerMask; // choose "Ground Colliders" for the Physics2D.OverlapArea -function to only detect colliders that have been set to that specific layer

        Rigidbody2D rb; // reference to the character's Rigidbody -component
        CharacterState cs; // reference to the character's CharacterStateCommunication -component

        Rect groundDetectionRect = Rect.zero; // rectangle object that defines the area used by the Physics2D.OverlapArea -function and that contains the data needed to draw a "gizmo" in Unity's scene view for visualization purposes
        RectToLines rtl; // a custom object that breaks down a Rect object into dots and provides a function for drawing the outlines of that Rect object using the Gizmos.DrawLine -function

        // the top-left and bottom-right corner points of the area used by the Physics2D.OverlapArea -function for detecting when the character is grounded
        [HideInInspector]
        public Vector2 GdrPointA;
        [HideInInspector]
        public Vector2 GdrPointB;

        [HideInInspector]
        public float ActualSprintMultiplier; // the actual sprint multiplier
        float actualJumpForce; // the actual jump force, (sort of) relative to jumpForce
        float actualActualJumpForce; // the actual actual jump force, (sort of) relative to jumpForce
        float sprintJumpEffectInitial; // the initial sprintJumpEffect, (sort of) relative to jumpForce

        // references to the virtual axes (see Unity's Input Manager)
        public float HorizontalInputValue
        {
            get { return Input.GetAxis("Horizontal"); }
        } // "Horizontal" (default)
        public float SprintInputValue
        {
            get { return Input.GetAxis("Sprint"); }
        } // "Sprint" (I added it myself, replacing "Fire3")
        public float SlideInputValue
        {
            get { return Input.GetAxis("Vertical"); }
        } // "Vertical" (default)
        public float JumpInputValue
        {
            get { return Input.GetAxis("Jump"); }
        } // "Jump" (default)

        // variables updated every time Unity's physics engine updates
        Vector2 currentVelocity; // self-explanatory (using rb.velocity's get method)
        float gravityScale; // self-explanatory (using rb.gravityScale's get method)
        float sprintJumpEffect; // decreases when sprinting, which in turn causes the character to jump higher, as sprintJumpEffect divides actualJumpForce before it's added as a positive Y force to the "rb"
        bool jumpReset;
        bool otherCollisions;

        [HideInInspector]
        public bool sliding;


        // "UNITY FUNCTIONS"


        void OnDrawGizmos()
        {

            Gizmos.color = Color.red;

            groundDetectionRect.size = GroundDetectionRectSize * OnePixelInUnits;
            groundDetectionRect.center = GroundDetectionRectPosition * OnePixelInUnits;

            rtl = new RectToLines(groundDetectionRect);
            rtl.DrawGizmoRect(transform.position);

        }


        void Awake()
        {

            // just fetching the corresponding components...
            rb = GetComponent<Rigidbody2D>();
            cs = GetComponent<CharacterState>();

            // sizing and positioning the "gdr"
            groundDetectionRect.size = GroundDetectionRectSize * OnePixelInUnits;
            groundDetectionRect.center = GroundDetectionRectPosition * OnePixelInUnits;

            // creating the RectToLines object from the groundDetectionRect and assigning values to the gdrPoints
            rtl = new RectToLines(groundDetectionRect);
            GdrPointA = rtl.TopLineFrom;
            GdrPointB = rtl.RightLineTo;

            Debug.Log("Inital Physics2D.OverlapArea is (" + GdrPointA.x + ", " + GdrPointA.y + ") to (" + GdrPointB.y + ", " + GdrPointB.y + ")");

            // assigning jump related values
            actualJumpForce = ((JumpForce * 0.1f) * 0.25f) + 5;
            actualActualJumpForce = (float)Math.Pow(actualJumpForce, actualJumpForce);
            sprintJumpEffectInitial = actualJumpForce * 2;

        }


        void OnCollisionEnter2D()
        {
            otherCollisions = true;
        }


        void OnCollisionExit2D()
        {
            otherCollisions = false;
        }


        void FixedUpdate()
        {

            // updating current velocity and gravity scale in its own variable, as it is used a lot
            currentVelocity = rb.velocity;
            gravityScale = rb.gravityScale;

            // updating states
            cs.CharacterMovementState(currentVelocity);

            // running character movement functions
            HorizontalMovement();
            Slide();
            Jump();

        }


        // CHARACTER MOVEMENT FUNCTIONS


        void HorizontalMovement()
        {

            if (SprintInputValue > 0 && cs.IsMoving)
            {
                if (sprintJumpEffect > sprintJumpEffectInitial * (1 - SprintJumpEffectAmount)) sprintJumpEffect -= (float)actualJumpForce / SprintJumpEffectChargeTime;
                ActualSprintMultiplier = (1 - ((sprintJumpEffect - (sprintJumpEffectInitial * (1 - SprintJumpEffectAmount))) / (sprintJumpEffectInitial * SprintJumpEffectAmount))) *
                                         SprintMultiplier;
            }
            else if (cs.GroundContact)
            {
                sprintJumpEffect = sprintJumpEffectInitial;
                ActualSprintMultiplier = 0;
            }

            if (otherCollisions == true && !cs.GroundContact) return;
            if (sliding == true) return;

            rb.velocity = new Vector2(HorizontalInputValue * MovementSpeed * (ActualSprintMultiplier + 1), currentVelocity.y);

        }


        void Slide()
        {
            if (SlideInputValue < 0 && cs.IsMoving) sliding = true;
            else sliding = false;
        }


        void Jump()
        {

            if (cs.GroundContact && JumpInputValue > 0.5f && !jumpReset)
            {
                rb.AddForce(new Vector2(0, actualActualJumpForce / sprintJumpEffect));
                jumpReset = true;
            }
            else if (!cs.GroundContact || JumpInputValue < 0.5f)
            {
                jumpReset = false;
            }

            // following code snippet inspired by YouTuber Board to Bits Games video on how to improve jumping in Unity
            if (currentVelocity.y < 0)
            {
                rb.velocity += Vector2.up * ((3 * Physics2D.gravity.y) / 50) * gravityScale;
            }
            if (currentVelocity.y > 0 && JumpInputValue < 0.5f)
            {
                rb.velocity += Vector2.up * ((2 * Physics2D.gravity.y) / 50) * gravityScale;
            }
            else if (currentVelocity.y > 0)
            {
                rb.velocity += Vector2.up * ((1 * Physics2D.gravity.y) / 50) * gravityScale;
            }
                
        }


        // MISCELLANEOUS


        static Vector2 Vector2Addition(Vector2 firstVector, Vector2 secondVector)
        {

            firstVector.x += secondVector.x;
            firstVector.y += secondVector.y;
            return firstVector;

        }


        public class RectToLines {

            public readonly Vector2 TopLineFrom;
            public readonly Vector2 TopLineTo;

            public readonly Vector2 RightLineFrom;
            public readonly Vector2 RightLineTo;

            public readonly Vector2 BottomLineFrom;
            public readonly Vector2 BottomLineTo;

            public readonly Vector2 LeftLineFrom;
            public readonly Vector2 LeftLineTo;

            public RectToLines(Rect rect)
            {
                TopLineFrom = new Vector3(rect.xMin, rect.yMin, 0);
                TopLineTo = new Vector3(rect.xMax, rect.yMin, 0);

                RightLineFrom = new Vector3(rect.xMax, rect.yMin, 0);
                RightLineTo = new Vector3(rect.xMax, rect.yMax, 0);

                BottomLineFrom = new Vector3(rect.xMax, rect.yMax, 0);
                BottomLineTo = new Vector3(rect.xMin, rect.yMax, 0);

                LeftLineFrom = new Vector3(rect.xMin, rect.yMax, 0);
                LeftLineTo = new Vector3(rect.xMin, rect.yMin, 0);
            }

            public void DrawGizmoRect(Vector2 currentPosition)
            {
                Gizmos.DrawLine(Vector2Addition(currentPosition, TopLineFrom), Vector2Addition(currentPosition, TopLineTo));
                Gizmos.DrawLine(Vector2Addition(currentPosition, RightLineFrom), Vector2Addition(currentPosition, RightLineTo));
                Gizmos.DrawLine(Vector2Addition(currentPosition, BottomLineFrom), Vector2Addition(currentPosition, BottomLineTo));
                Gizmos.DrawLine(Vector2Addition(currentPosition, LeftLineFrom), Vector2Addition(currentPosition, LeftLineTo));
            }

        }

    }
}
