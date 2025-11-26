using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody2D rb;
    public float speed = 10f;
    private Vector2 movement;
    void Start()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
        rb.velocity = new Vector2(movement.x * speed, movement.y * speed);
    }
}
