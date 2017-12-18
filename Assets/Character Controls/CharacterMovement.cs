using System;
using UnityEngine;

namespace Assets.Character_Controls
{
    public class CharacterMovement : MonoBehaviour
    {

        private const float OnePixelInUnits = 0.0625f;



        public int MovementSpeed = 5;
        public float MovementThreshold = 0.2f;
        public float SprintMultiplier = 1.5f;
        public int JumpForce = 5;

        public Vector2 GroundDetectionRectPosition = Vector2.zero;
        public Vector2 GroundDetectionRectSize = new Vector2(1, 1);
        public LayerMask LayerMask;


        Rigidbody2D rb;

        Rect groundDetectionRect = Rect.zero;
        RectToLines rtl1;
        Vector2 gdrPointA;
        Vector2 gdrPointB;
     /* Rect jumpLandingDetectionRect = Rect.zero;
        RectToLines rtl2;
        Vector2 jldrPointA;
        Vector2 jldrPointB;  */


        float horizontalInputValue;
        float sprintInputValue;
        bool jumpInputValue;



        float hVelocity;
        float vVelocity;
        float sprintAccelerator;
        float sprintEffect;



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





        // ReSharper disable once UnusedMember.Local
        void OnDrawGizmos()
        {

            Gizmos.color = Color.red;

            var gdrPos = GroundDetectionRectPosition * OnePixelInUnits;
            var gdrSize = GroundDetectionRectSize * OnePixelInUnits;
            groundDetectionRect.size = gdrSize;
            groundDetectionRect.center = gdrPos;

            rtl1 = new RectToLines(groundDetectionRect);
            rtl1.DrawGizmoRect(transform.position);

        }





        // ReSharper disable once UnusedMember.Local
        void Awake()
        {

            rb = GetComponent<Rigidbody2D>();

            var gdrPos = GroundDetectionRectPosition * OnePixelInUnits;
            var gdrSize = GroundDetectionRectSize * OnePixelInUnits;
            groundDetectionRect.size = gdrSize;
            groundDetectionRect.center = gdrPos;

            rtl1 = new RectToLines(groundDetectionRect);
            gdrPointA = rtl1.TopLineFrom;
            gdrPointB = rtl1.RightLineTo;

            Debug.Log("Inital Physics2D.OverlapArea is (" + gdrPointA.x + ", " + gdrPointA.y + ") to (" + gdrPointB.y + ", " + gdrPointB.y + ")");

            sprintAccelerator = 1;
            sprintEffect = JumpForce * 2;

        }

        // ReSharper disable once UnusedMember.Local
        void FixedUpdate()
        {

            horizontalInputValue = Input.GetAxis("Horizontal");
            sprintInputValue = Input.GetAxis("Sprint");
            jumpInputValue = Input.GetButtonDown("Jump");

            CharacterMovementState();

            HorizontalMovement();
            Jump();

        }





        void CharacterMovementState()
        {

            var pointA = Vector2Addition(transform.position, gdrPointA);
            var pointB = Vector2Addition(transform.position, gdrPointB);

            SetGroundContact = Physics2D.OverlapArea(pointA, pointB, LayerMask);

            var vel = rb.velocity;

            if (GroundContact && Math.Abs(vel.x) > MovementSpeed * MovementThreshold) SetIfMoving = true;
            else SetIfMoving = false;

            if (IsMoving && sprintInputValue > 0) SetIfSprinting = true;
            else SetIfSprinting = false;

            if (GroundContact && jumpInputValue) SetIfJumping = true;
            if (IsJumping && GroundContact) SetIfJumping = false;

            if (vel.x > 0) SetFacingDirection = "right";
            else if (vel.x < 0) SetFacingDirection = "left";

        }

        void HorizontalMovement()
        {

            if (sprintInputValue * SprintMultiplier > 1)
            {
                sprintAccelerator = sprintInputValue * SprintMultiplier;
                if (sprintEffect > ((float)JumpForce * 2) / 4) sprintEffect -= (float)JumpForce / 100;
                Debug.Log(sprintEffect);
            }
            else
            {
                sprintAccelerator = 1;
                sprintEffect = JumpForce * 2;
            }

            hVelocity = horizontalInputValue * MovementSpeed * sprintAccelerator;
            vVelocity = rb.velocity.y;
            rb.velocity = new Vector2(hVelocity, vVelocity);

        }

        void Jump()
        {

            if (GroundContact && jumpInputValue)
            {
                rb.AddForce(new Vector2(0, (float)Math.Pow(JumpForce, JumpForce) / sprintEffect));
            }
                
        }





        static Vector2 Vector2Addition(Vector2 firstVector, Vector2 secondVector)
        {

            firstVector.x += secondVector.x;
            firstVector.y += secondVector.y;
            return firstVector;

        }





        class RectToLines {

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
