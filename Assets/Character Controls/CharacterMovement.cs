using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {

    public float movementMultiplier = 5;

    Rigidbody2D rb;
    float hInputValue;
    float hVel;
    float vVel;

	// Use this for initialization
	void Awake ()
    {
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        hInputValue = Input.GetAxis("Horizontal");
        hVel = hInputValue * movementMultiplier;
        vVel = rb.velocity.y;
        rb.velocity = new Vector2(hVel, vVel);
	}

}
