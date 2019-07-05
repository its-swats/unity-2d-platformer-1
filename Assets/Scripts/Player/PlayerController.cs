using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float playerSpeed = 2;
    private float jumpSpeed = 6;
    private float horizontal;

    private bool facingRight = true;
    private bool ducking = false;
    [SerializeField] private bool isGrounded = true;
    [SerializeField] Transform groundCheck;

    private Animator animator;
    private Rigidbody2D rb2d;


    private void Start(){
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update(){
        horizontal = Input.GetAxisRaw("Horizontal");
        ducking = Input.GetAxisRaw("Vertical") == -1;
    }

    private void FixedUpdate(){
        if(FacingWrongDirection()){
            Flip();    
        }

        if(Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"))){
            isGrounded = true;
        } else {
            isGrounded = false;
        }

        if(horizontal != 0){
            rb2d.velocity = new Vector2(horizontal * playerSpeed, rb2d.velocity.y);
            if(isGrounded) { animator.Play("Run"); }
        } else if(ducking && isGrounded){
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
            animator.Play("Duck");
        } else {
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
            if(isGrounded) { animator.Play("Idle"); }
        }

        if(Input.GetButtonDown("Jump") && isGrounded){
            animator.Play("Jump");
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpSpeed);
        }
    }

    private void Flip(){

        facingRight = !facingRight;
        transform.Rotate(new Vector2(0, 180));
    }

    private bool FacingWrongDirection(){
        return (facingRight && horizontal == -1) || (!facingRight && horizontal == 1);
    }

}
