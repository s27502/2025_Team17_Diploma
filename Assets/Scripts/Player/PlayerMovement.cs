using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public float speed = 10f;
    public Animator animator;
    private Vector2 _movement;
    private bool tmp;
    
    void Start()
    {
        rb.drag = 0;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        _movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        rb.velocity = _movement * speed;
        
        if (_movement.x != 0 || _movement.y != 0)
        {
            animator.SetBool("isWalking", true);
            sr.flipX =  _movement.x < 0;
            tmp = sr.flipX;
            
        }
        else
        {
            animator.SetBool("isWalking",false);
            sr.flipX = false;
        }

        if (_movement.x == 0 && _movement.y != 0)
        {
            // Debug.Log(tmp);
            sr.flipX = tmp;
        }
        
    }
}
