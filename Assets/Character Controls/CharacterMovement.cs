using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Character_Controls
{
    public class CharacterMovement : MonoBehaviour
    {

        private const float OnePixelInUnits = 0.0625f;



        public int MovementSpeed = 5;
        public int MovementSmoothing = 5;
        public float SprintMultiplier = 1.5f;
        public int JumpForce = 5;

        public Vector2 GroundDetectionRectPosition = Vector2.zero;
        public Vector2 GroundDetectionRectSize = new Vector2(1, 1);
        public LayerMask LayerMask;



        private Rigidbody2D _rb;

        private Rect _groundDetectionRect = Rect.zero;
        private RectToLines _rtl;
        private Vector2 _gdrPointA;
        private Vector2 _gdrPointB;



        private float _horizontalInputValue;
        private float _sprintInputValue;
        private float _jumpInputValue;



        private float _hVelocity;
        private float _vVelocity;
        private float _mvmtSmoothener;
        private float _mvmtSmoothenerBase;



        [HideInInspector]
        public bool GroundContact;
        private bool SetGroundContact
        {
            get { return GroundContact; }
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
                    default:
                        break;
                }
            }
        }

        [HideInInspector]
        public bool IsMoving;
        private bool SetIfMoving
        {
            get { return IsMoving; }
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
                    default:
                        break;
                }
            }
        }

        [HideInInspector]
        public bool IsSprinting;
        private bool SetIfSprinting
        {
            get { return IsSprinting; }
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
                    default:
                        break;
                }
            }
        }

        [HideInInspector]
        public bool IsJumping;
        private bool SetIfJumping
        {
            get { return IsJumping; }
            set
            {
                if (IsJumping == value) return;
                if (value == true) Debug.Log("Character jumped\nIsJumping set to true 'til character lands again");
                IsJumping = value;
            }
        }





        private void OnDrawGizmos()
        {

            Gizmos.color = Color.red;

            var gdrPos = GroundDetectionRectPosition * OnePixelInUnits;
            var gdrSize = GroundDetectionRectSize * OnePixelInUnits;
            _groundDetectionRect.size = gdrSize;
            _groundDetectionRect.center = gdrPos;

            _rtl = new RectToLines(_groundDetectionRect);
            _rtl.DrawGizmoRect(transform.position);

        }





        private void Awake()
        {

            _rb = GetComponent<Rigidbody2D>();

            var gdrPos = GroundDetectionRectPosition * OnePixelInUnits;
            var gdrSize = GroundDetectionRectSize * OnePixelInUnits;
            _groundDetectionRect.size = gdrSize;
            _groundDetectionRect.center = gdrPos;

            _rtl = new RectToLines(_groundDetectionRect);
            _gdrPointA = _rtl._topLineFrom;
            _gdrPointB = _rtl._rightLineTo;

            Debug.Log("Inital Physics2D.OverlapArea is (" + _gdrPointA.x + ", " + _gdrPointA.y + ") to (" + _gdrPointB.y + ", " + _gdrPointB.y + ")");

            _mvmtSmoothenerBase = 1f / (MovementSmoothing * 10f);
            _mvmtSmoothener = _mvmtSmoothenerBase;

        }
	
        private void FixedUpdate()
        {

            CharacterMovementState();

            if (GroundContact == false) return;

            HorizontalMovement();

            _jumpInputValue = Input.GetAxisRaw("Jump");
            _rb.AddForce(new Vector2(0, _jumpInputValue * JumpForce * 10));

        }





        private void CharacterMovementState()
        {

            var pointA = Vector2Addition(transform.position, _gdrPointA);
            var pointB = Vector2Addition(transform.position, _gdrPointB);
            SetGroundContact = Physics2D.OverlapArea(pointA, pointB, LayerMask);

        }

        private void HorizontalMovement()
        {
            _horizontalInputValue = Input.GetAxisRaw("Horizontal");
            var vel = _rb.velocity;
            if (!(Math.Abs(_horizontalInputValue) > 0) && !(Math.Abs(vel.x) > 0))
            {
                return;
            }
            else if ((_horizontalInputValue > 0  && vel.x >= 0) || (_horizontalInputValue < 0 && vel.x <= 0))
            {
                _hVelocity = _horizontalInputValue * MovementSpeed * easeInQuart(_mvmtSmoothener);
                _vVelocity = vel.y;
                _rb.velocity = new Vector2(_hVelocity, _vVelocity);
                if (_mvmtSmoothener <= 1) _mvmtSmoothener += _mvmtSmoothenerBase;
            }
            else if ((_horizontalInputValue > 0 && vel.x < 0) || (_horizontalInputValue < 0 && vel.x > 0))
            {
                _hVelocity = MovementSpeed * (1 - easeInQuart(_mvmtSmoothener));
                _vVelocity = vel.y;
                _rb.velocity = new Vector2(_hVelocity, _vVelocity);
                if (_mvmtSmoothener > 0) _mvmtSmoothener -= _mvmtSmoothenerBase;
            }
            else if (Math.Abs(vel.x) > 0)
            {
                _hVelocity = MovementSpeed * (1 - easeInQuart(_mvmtSmoothener));
                _vVelocity = vel.y;
                _rb.velocity = new Vector2(_hVelocity, _vVelocity);
                if (_mvmtSmoothener > 0) _mvmtSmoothener -= _mvmtSmoothenerBase;
            }
            
        }





        private static Vector2 Vector2Addition(Vector2 firstVector, Vector2 secondVector)
        {

            firstVector.x += secondVector.x;
            firstVector.y += secondVector.y;
            return firstVector;

        }

        private static float easeInQuart(float t)
        {
            return t < .5 ? 8 * t * t * t * t : 1 - 8 * (--t) * t * t * t; // check out https://gist.github.com/gre/1650294
        }





        private class RectToLines {

            public readonly Vector2 _topLineFrom;
            public readonly Vector2 _topLineTo;

            public readonly Vector2 _rightLineFrom;
            public readonly Vector2 _rightLineTo;

            public readonly Vector2 _bottomLineFrom;
            public readonly Vector2 _bottomLineTo;

            public readonly Vector2 _leftLineFrom;
            public readonly Vector2 _leftLineTo;

            public RectToLines(Rect rect)
            {
                _topLineFrom = new Vector3(rect.xMin, rect.yMin, 0);
                _topLineTo = new Vector3(rect.xMax, rect.yMin, 0);

                _rightLineFrom = new Vector3(rect.xMax, rect.yMin, 0);
                _rightLineTo = new Vector3(rect.xMax, rect.yMax, 0);

                _bottomLineFrom = new Vector3(rect.xMax, rect.yMax, 0);
                _bottomLineTo = new Vector3(rect.xMin, rect.yMax, 0);

                _leftLineFrom = new Vector3(rect.xMin, rect.yMax, 0);
                _leftLineTo = new Vector3(rect.xMin, rect.yMin, 0);
            }

            public void DrawGizmoRect(Vector2 currentPosition)
            {
                Gizmos.DrawLine(Vector2Addition(currentPosition, _topLineFrom), Vector2Addition(currentPosition, _topLineTo));
                Gizmos.DrawLine(Vector2Addition(currentPosition, _rightLineFrom), Vector2Addition(currentPosition, _rightLineTo));
                Gizmos.DrawLine(Vector2Addition(currentPosition, _bottomLineFrom), Vector2Addition(currentPosition, _bottomLineTo));
                Gizmos.DrawLine(Vector2Addition(currentPosition, _leftLineFrom), Vector2Addition(currentPosition, _leftLineTo));
            }

        }

    }
}
