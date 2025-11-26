using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody2D rb;
    public float speed = 10f;
    private Vector2 _movement;
    void Start()
    {
        rb.drag = 0;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        _movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        rb.velocity = _movement * speed;
    }
}
