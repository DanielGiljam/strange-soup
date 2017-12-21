using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class HudData : MonoBehaviour
    {

        Rigidbody2D rb;

        // NOTE! Set in Inspector!
        public Text VelocityText;
        public Text AccelerationText;
        public Text HOffsetAngleText;
        public Text ForceText;
        public Text NormalForceText;
        public Text FrictionText;

        float velocity;
        float acceleration;
        float hOffsetAngle;
        float forceText;
        float normalForceText;
        float frictionText;

        string numFormat = "0.00";
        string degreeFormat = "0°";

        float[] deltaVContainer = new float[50];
        int deltaVIterator = 0;

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            for (var i = 0; i < 50; i++)
            {
                deltaVContainer[i] = 0;
            }
        }
	
        void FixedUpdate()
        {

            velocity = Mathf.Sqrt(Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.y, 2));
            acceleration = velocity - deltaVContainer[deltaVIterator];

            deltaVContainer[deltaVIterator] = velocity;
            deltaVIterator++;

            if (deltaVIterator == 50) deltaVIterator = 0;

        }

        void Update()
        {

            VelocityText.text = velocity.ToString(numFormat);

        }

    }
}
