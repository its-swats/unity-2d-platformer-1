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
    [SerializeField] Transform firingPointIdle;
    [SerializeField] Transform firingPointJump;
    [SerializeField] Transform firingPointDuck;
    [SerializeField] GameObject bulletRef;
    [SerializeField] BoxCollider2D standingCollider;
    [SerializeField] BoxCollider2D duckingCollider;
    [SerializeField] BoxCollider2D jumpingCollider;

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
            Fire();
        }
    }

    private void FixedUpdate(){
        if(FacingWrongDirection()){
            Flip();    
        }

        isGrounded = CheckGround();

        if(Input.GetAxisRaw("Jump") == 1 && isGrounded && !ducking){
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpSpeed);
        }

        if(horizontal != 0 && (!ducking || !isGrounded)){
            rb2d.velocity = new Vector2(horizontal * playerSpeed, rb2d.velocity.y);
        } else if(ducking && isGrounded){
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
        } else {
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
        }

        UpdateAnimations();
        UpdateHitboxes();
    }

    private void Flip(){
        facingRight = !facingRight;
        transform.Rotate(new Vector2(0, 180));
    }

    private void Fire(){
        Instantiate(bulletRef, CurrentFiringPoint().position, transform.rotation);
    }

    private Transform CurrentFiringPoint(){
        if(!isGrounded){
            return firingPointJump;
        } else if(isGrounded && ducking){
            return firingPointDuck;
        } else {
            return firingPointIdle;
        }
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

    private void UpdateAnimations(){
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetBool("IsDucking", ducking);
        animator.SetBool("IsRunning", Mathf.Abs(horizontal) == 1);
    }

    private void UpdateHitboxes(){
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Duck")){
            duckingCollider.enabled = true;
            jumpingCollider.enabled = false;
            standingCollider.enabled = false;
        } else if(animator.GetCurrentAnimatorStateInfo(0).IsName("Jump")){
            duckingCollider.enabled = false;
            jumpingCollider.enabled = true;
            standingCollider.enabled = false;
        } else {
            duckingCollider.enabled = false;
            jumpingCollider.enabled = false;
            standingCollider.enabled = true;
        }
    }

} 
