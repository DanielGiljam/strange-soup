using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class HudData : MonoBehaviour
    {

        const string NumFormat = "0.00";
        const string DegreeFormat = "0°";
        const float GravitationalAcceleration = -9.81f;


        // VARIABLE INITIALIZATIONS


        // NOTE! Set in Inspector!
        public Text VelocityText;
        public Text AccelerationText;
        public Text HorizontalOffsetAngleText;
        public Text ForceText;
        public Text FrictionForceText;

        Rigidbody2D rb; // reference to the character's Rigidbody -component

        // variables for the different physical (mechanic) properties calculated in this script
        float velocity;
        float acceleration;
        float hOffsetAngle;
        float force;
        float frictionForce;

        // variables for calculating delta- velocity and position
        float previousVelocity;
        Vector2 currentPosition;
        Vector2 previousPosition;

        // variables for streamlining the code (making it nicer to read)
        float adjacent;
        float opposite;

        // variables for other properties needed to calculate the properties that the HUD displays
        float mass;
        float gravity;
        float frictionCoefficient;

        /* Alternative way of counting acceleration:
                readonly float[] previousVelocityContainer = new float[50];
                int previousVelocityIterator = 0;
        */


        // "UNITY FUNCTIONS"


        void Awake()
        {

            // just fetching the corresponding components...
            rb = GetComponent<Rigidbody2D>();

            // setting 0 values to avoid nulls
            velocity = 0;
            currentPosition = Vector2.zero;
            frictionCoefficient = 0;

            /*
                for (var i = 0; i < 50; i++)
                {
                    previousVelocityContainer[i] = 0;
                }
            */

        }

        void OnCollisionEnter2D(Collision2D collision)
        {

            frictionCoefficient = Mathf.Sqrt(collision.collider.friction * collision.otherCollider.friction); // calculating the friction coefficient between the materials in contact with each other

        }

        void OnCollisionExit2D()
        {

            frictionCoefficient = 0; // not accounting for fluid friction, always assuming that the character's Rigidbody has linear drag set to zero

        }
	
        void FixedUpdate()
        {

            previousVelocity = velocity; // previous/initial current velocity is stored as previous velocity

            // the position -variables use the same strategy as the velocity -variables
            previousPosition = currentPosition;
            currentPosition = transform.position;

            // represent the adjacent and opposite sides of a right triangle and are later used to calculate the angle of the character's motion
            adjacent = currentPosition.x - previousPosition.x;
            opposite = currentPosition.y - previousPosition.y;

            // refreshing other properties needed in calculating the HUD data
            mass = rb.mass;
            gravity = rb.gravityScale * GravitationalAcceleration;

            // calculating velocity
            velocity = Mathf.Sqrt(Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.y, 2));
            // calculating acceleration. The multiplier converts the result from units per fixed timestep to units per seconds (check out Project Settings --> Time in Unity)
            acceleration = (velocity - previousVelocity) * (1 / Time.fixedDeltaTime);
            // calculating the angle of the character's motion (range -90 to 90 degrees) using the inverse tangent function. If/else -statement implemented to prevent calculating with zero (especially if adjacent is zero)
            if (Mathf.Abs(adjacent) > 0 && Mathf.Abs(opposite) > 0) hOffsetAngle = Mathf.Atan(opposite / adjacent) * Mathf.Rad2Deg;
            else hOffsetAngle = 0;
            // calculating the force and friction force. If/else -statement implemented in order to display the friction force as zero when the character isn't moving. Friction force made positive as the name already communicates the direction of the force.
            if (velocity > 0)
            {
                frictionForce = -(mass * gravity * frictionCoefficient);
                force = (mass * acceleration) + frictionForce;
            }
            else
            {
                frictionForce = 0;
                force = (mass * acceleration) + frictionForce;
            }

            /*
                previousVelocityContainer[previousVelocityIterator] = velocity;
                previousVelocityIterator++;

                if (previousVelocityIterator == 50) previousVelocityIterator = 0;
            */

        }

        void Update()
        {

            // refreshing the values that the HUD displays
            VelocityText.text = velocity.ToString(NumFormat);
            AccelerationText.text = acceleration.ToString(NumFormat);
            HorizontalOffsetAngleText.text = hOffsetAngle.ToString(DegreeFormat);
            ForceText.text = force.ToString(NumFormat);
            FrictionForceText.text = frictionForce.ToString(NumFormat);

        }

    }
}
