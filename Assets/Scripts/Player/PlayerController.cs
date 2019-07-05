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
    [SerializeField] Transform groundCheck2;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform firingPoint;
    [SerializeField] GameObject bulletRef;

    private Animator animator;
    private Rigidbody2D rb2d;


    private void Start(){
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update(){
        horizontal = Input.GetAxisRaw("Horizontal");
        ducking = Input.GetAxisRaw("Vertical") == -1;
        if(Input.GetButtonDown("Fire1")){
            Instantiate(bulletRef, firingPoint.position, transform.rotation);
        }
    }

    private void FixedUpdate(){
        if(FacingWrongDirection()){
            Flip();    
        }

        isGrounded = CheckGround();

        if(Input.GetAxisRaw("Jump") == 1 && isGrounded){
            animator.Play("Jump");
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpSpeed);
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
    }

    private void Flip(){

        facingRight = !facingRight;
        transform.Rotate(new Vector2(0, 180));

        // transform.Rotate(0f, 180f, 0f);
    }

    private bool FacingWrongDirection(){
        return (facingRight && horizontal == -1) || (!facingRight && horizontal == 1);
    }

    private bool CheckGround(){
        Debug.DrawRay(transform.position, new Vector2(0, 0.02f), Color.green);
        RaycastHit2D hitOne = Physics2D.Raycast(groundCheck.transform.position, Vector2.down, 0.02f, groundLayer);
        RaycastHit2D hitTwo = Physics2D.Raycast(groundCheck2.transform.position, Vector2.down, 0.02f, groundLayer);
        if (hitOne.collider != null || hitTwo.collider != null){
            return true;
        }        
        return false;
    }

}
