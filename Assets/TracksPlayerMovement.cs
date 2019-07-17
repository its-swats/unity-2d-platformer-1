using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracksPlayerMovement : MonoBehaviour
{
    private bool facingLeft = true;
    [SerializeField] Animator anim; 
    [SerializeField] float updateSpeed = 1f;
    [SerializeField] private bool canFlip; 
    [SerializeField] private bool canAim; 
    private float updateValue = 0;

    void Update()
    {
        if(updateValue >= updateSpeed){
            Vector2 playerLoc = Player.Instance.transform.position;
            if(canFlip && ShouldFlip(playerLoc)){
                Flip();
            }

            if(canAim){
                if(Player.Instance.transform.position.y + .5f < transform.position.y){
                    anim.Play("AimDown");
                } else if(Player.Instance.transform.position.y - .5f > transform.position.y){
                    anim.Play("AimUp");
                } else {
                    anim.Play("Idle");
                }
            }

            updateValue = 0;
        } else { 
            updateValue += Time.deltaTime;
        }
    }

    private bool ShouldFlip(Vector2 playerLoc){
        return playerLoc.x > transform.position.x && facingLeft || playerLoc.x < transform.position.x && !facingLeft;
    }

    private void Flip(){
        facingLeft = !facingLeft;
        transform.Rotate(new Vector2(0, 180));
    }
}
