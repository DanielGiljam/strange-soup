using Assets.Character_Logic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class HudData : MonoBehaviour
    {

        const string NumFormat = "0.00";
        const string DegreeFormat = "0°";
        const float GravitationalAcceleration = -9.81f;

        // NOTE! Set in Inspector!
        public Text VelocityText;
        public Text AccelerationText;
        public Text HorizontalOffsetAngleText;
        public Text ForceText;
        public Text FrictionForceText;

        Rigidbody2D rb;

        float velocity;
        float acceleration;
        float hOffsetAngle;
        float force;
        float frictionForce;

        float previousVelocity;

        Vector2 currentPosition;
        Vector2 previousPosition;
        float a;
        float b;

        float mass;
        float gravity;
        float frictionAmount;

        /* Alternative way of counting acceleration:
                readonly float[] previousVelocityContainer = new float[50];
                int previousVelocityIterator = 0;
        */

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            velocity = 0;
            frictionAmount = 0;
            /*
                for (var i = 0; i < 50; i++)
                {
                    previousVelocityContainer[i] = 0;
                }
            */

        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            frictionAmount = Mathf.Sqrt(collision.collider.friction * collision.otherCollider.friction);
        }

        void OnCollisionExit2D()
        {
            frictionAmount = 0;
        }
	
        void FixedUpdate()
        {

            previousVelocity = velocity;

            previousPosition = currentPosition;
            currentPosition = transform.position;
            a = currentPosition.x - previousPosition.x;
            b = currentPosition.y - previousPosition.y;

            mass = rb.mass;
            gravity = rb.gravityScale * GravitationalAcceleration;

            velocity = Mathf.Sqrt(Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.y, 2));
            acceleration = (velocity - previousVelocity) * 50;
            if (Mathf.Abs(a) > 0 && Mathf.Abs(b) > 0) hOffsetAngle = Mathf.Atan(b / a) * Mathf.Rad2Deg;
            else hOffsetAngle = 0;
            if (velocity > 0)
            {
                frictionForce = -(mass * gravity * frictionAmount);
                force = (mass * acceleration) + frictionForce;
            }
            else
            {
                frictionForce = 0;
                force = mass * acceleration;
            }
            
            /*
                previousVelocityContainer[previousVelocityIterator] = velocity;
                previousVelocityIterator++;

                if (previousVelocityIterator == 50) previousVelocityIterator = 0;
            */

        }

        void Update()
        {

            VelocityText.text = velocity.ToString(NumFormat);
            AccelerationText.text = acceleration.ToString(NumFormat);
            HorizontalOffsetAngleText.text = hOffsetAngle.ToString(DegreeFormat);
            ForceText.text = force.ToString(NumFormat);
            FrictionForceText.text = frictionForce.ToString(NumFormat);

        }

    }
}
