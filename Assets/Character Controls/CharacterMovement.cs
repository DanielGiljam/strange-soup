using UnityEngine;

namespace Assets.Character_Controls
{
    public class CharacterMovement : MonoBehaviour {

        public float MovementSpeed = 5;
        public float SprintMultiplier = 2;
        public float JumpHeight = 5;

        private Rigidbody2D _rb;
        private BoxCollider2D _bc;
        private float _hInputValue;
        private float _jumpInputValue;
        private float _hVel;
        private float _vVel;

        // Use this for initialization
        private void Awake ()
        {
            _rb = GetComponent<Rigidbody2D>();
            _bc = GetComponent<BoxCollider2D>();
        }
	
        // Update is called once per frame
        private void FixedUpdate ()
        {
            _hInputValue = Input.GetAxis("Horizontal");
            _hVel = _hInputValue * MovementSpeed;
            _vVel = _rb.velocity.y;
            _rb.velocity = new Vector2(_hVel, _vVel);
            _jumpInputValue = Input.GetAxis("Jump");
            _rb.AddForce(new Vector2(0, _jumpInputValue * JumpHeight));
        }

    }
}
