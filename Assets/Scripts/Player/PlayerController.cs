﻿using System.Collections;
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
    private bool runAimUp = false;
    [SerializeField] private bool isGrounded = true;

    [SerializeField] Transform groundCheck;
    [SerializeField] Transform groundCheck2;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] Transform firingPointIdle;
    [SerializeField] Transform firingPointJump;
    [SerializeField] Transform firingPointDuck;
    [SerializeField] Transform firingPointUp;
    [SerializeField] Transform firingPointUpRight;
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

    private void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.CompareTag("Enemy")){
            StartCoroutine(Die());
        }
    }

    private void OnTriggerEnter2D(Collider2D collider){
        if(collider.gameObject.CompareTag("OutOfBounds")){
            StartCoroutine(Die());
        }
    }

    private void Update(){
        horizontal = Input.GetAxisRaw("Horizontal");
        runAimUp = Input.GetAxisRaw("Vertical") == 1 && horizontal != 0 && isGrounded;
        aimUp = Input.GetAxisRaw("Vertical") == 1 && rb2d.velocity.magnitude == 0 && horizontal == 0;
        ducking = Input.GetAxisRaw("Vertical") == -1;
        UpdateAnimations();
        if(FacingWrongDirection()){
            Flip();    
        }

        UpdateHitboxes();
        if(Input.GetButtonDown("Fire1")){
            Fire();
        }
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
        } else if(runAimUp) {
            return firingPointUpRight;
        } else {
            return firingPointIdle;
        }
    }

    private bool FacingWrongDirection(){
        return (facingRight && horizontal == -1) || (!facingRight && horizontal == 1);
    }

    private bool CheckGround(){
        Debug.DrawRay(groundCheck.transform.position, new Vector2(0, -0.15f), Color.white);
        Debug.DrawRay(groundCheck2.transform.position, new Vector2(0, -0.15f), Color.white);
        RaycastHit2D hitOne = Physics2D.Raycast(groundCheck.transform.position, Vector2.down, 0.15f, groundLayer);
        RaycastHit2D hitTwo = Physics2D.Raycast(groundCheck2.transform.position, Vector2.down, 0.15f, groundLayer);
        if ((hitOne.collider != null || hitTwo.collider != null)){
            if(hitOne.collider != null)
                Debug.Log(hitOne.collider.gameObject.name);
            return true;
        }        
        return false;
    }

    private void UpdateAnimations(){
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetBool("IsDucking", ducking);
        animator.SetBool("IsRunning", Mathf.Abs(horizontal) == 1);
        animator.SetBool("IsAimingUp", aimUp);
        animator.SetBool("IsRunningAndAimingUp", runAimUp);
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

    IEnumerator Die(){
        Physics2D.IgnoreLayerCollision(10, 11, true);
        animator.SetBool("IsDead", true);
        rb2d.velocity = new Vector2(0, 5);
        enabled = false;
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
        Physics2D.IgnoreLayerCollision(10, 11, false);
        SpawnScript.spawner.SpawnPlayer();
    }
} 
