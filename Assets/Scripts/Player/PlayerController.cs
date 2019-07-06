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
    private bool aimUp = false;
    [SerializeField] private bool isGrounded = true;

    [SerializeField] Transform groundCheck;
    [SerializeField] Transform groundCheck2;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform firingPointIdle;
    [SerializeField] Transform firingPointJump;
    [SerializeField] Transform firingPointDuck;
    [SerializeField] Transform firingPointUp;
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
        aimUp = Input.GetAxisRaw("Vertical") == 1 && rb2d.velocity.magnitude == 0;
        ducking = Input.GetAxisRaw("Vertical") == -1;
        if(Input.GetButtonDown("Fire1")){
            Fire();
        }
        if(FacingWrongDirection()){
            Flip();    
        }
        // if(aimUp)

        UpdateHitboxes();
        UpdateAnimations();
    }

    private void FixedUpdate(){
        isGrounded = CheckGround();

        if(Input.GetAxisRaw("Jump") == 1 && isGrounded && !ducking){
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpSpeed);
        }

        if(horizontal != 0 && ((!ducking || !isGrounded) && !aimUp)){
            rb2d.velocity = new Vector2(horizontal * playerSpeed, rb2d.velocity.y);
        } else {
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
        }
    }

    private void Flip(){
        facingRight = !facingRight;
        transform.Rotate(new Vector2(0, 180));
    }

    private void Fire(){
        Instantiate(bulletRef, CurrentFiringPoint().position, CurrentFiringPoint().rotation);
    }

    private Transform CurrentFiringPoint(){
        if(!isGrounded){
            return firingPointJump;
        } else if(isGrounded && ducking){
            return firingPointDuck;
        } else if(aimUp){
            return firingPointUp;
        } else {
            return firingPointIdle;
        }
    }

    private bool FacingWrongDirection(){
        return (facingRight && horizontal == -1) || (!facingRight && horizontal == 1);
    }

    private bool CheckGround(){
        RaycastHit2D hitOne = Physics2D.Raycast(groundCheck.transform.position, Vector2.down, 0.02f, groundLayer);
        RaycastHit2D hitTwo = Physics2D.Raycast(groundCheck2.transform.position, Vector2.down, 0.02f, groundLayer);
        if ((hitOne.collider != null || hitTwo.collider != null) && rb2d.velocity.y == 0 ){
            return true;
        }        
        return false;
    }

    private void UpdateAnimations(){
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetBool("IsDucking", ducking);
        animator.SetBool("IsRunning", Mathf.Abs(horizontal) == 1);
        animator.SetBool("IsAimingUp", aimUp);
    }

    private void UpdateHitboxes(){
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Duck")){
            if(!duckingCollider.enabled){
                duckingCollider.enabled = true;
                jumpingCollider.enabled = false;
                standingCollider.enabled = false;
            }
        } else if(animator.GetCurrentAnimatorStateInfo(0).IsName("Jump")){
            if(!jumpingCollider.enabled) {
                duckingCollider.enabled = false;
                jumpingCollider.enabled = true;
                standingCollider.enabled = false;
            }
        } else if(!standingCollider.enabled) {
            duckingCollider.enabled = false;
            jumpingCollider.enabled = false;
            standingCollider.enabled = true;
        }
    }

} 
