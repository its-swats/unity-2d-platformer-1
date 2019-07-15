using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float playerSpeed = 2;
    private float jumpSpeed = 6;
    private float horizontal;
    private float vertical;

    private bool dying = false;

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
    [SerializeField] BoxCollider2D positionCollider;

    private Animator animator;
    private Rigidbody2D rb2d;


    private void Start(){
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collider){
            if(collider.gameObject.CompareTag("OutOfBounds")){
                StartCoroutine(Die());
            }

            if(collider.gameObject.CompareTag("Enemy")){
                StartCoroutine(Die());
            }
    }

    private void Update(){
        isGrounded = CheckGround();
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        runAimUp = vertical == 1 && horizontal != 0 && isGrounded;
        aimUp = vertical == 1 && horizontal == 0 && isGrounded;
        ducking = vertical == -1;
        
        if(FacingWrongDirection()){
            Flip();    
        }

        if(Input.GetButtonDown("Fire1")){
            Fire();
        }

        if(Input.GetButtonDown("Jump")  && isGrounded && !ducking){
           Jump();
        }

        UpdateAnimations();
        UpdateHitboxes();
    }

    private void Jump(){
        rb2d.velocity = new Vector2(rb2d.velocity.x, jumpSpeed);
    }

    private void FixedUpdate(){
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
            return true;
        }        
        return false;
    }

    private void UpdateAnimations(){
        animator.SetBool("IsGrounded", isGrounded && Mathf.Abs(rb2d.velocity.x) >= Mathf.Abs(rb2d.velocity.y));
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

    public IEnumerator Die(){
        if(!dying){
            dying = true;
            enabled = false;
            Physics2D.IgnoreLayerCollision(10, 11, true);
            animator.SetBool("IsDead", true);
            rb2d.velocity = new Vector2(0, 5);
            yield return new WaitForSeconds(2f);
            Destroy(gameObject);
            Physics2D.IgnoreLayerCollision(10, 11, false);
            SpawnScript.spawner.SpawnPlayer();
            dying = false;
        }
    }
} 
