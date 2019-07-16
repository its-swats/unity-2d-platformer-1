using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAtPlayer : MonoBehaviour
{
    [SerializeField] private GameObject[] firingPoints;
    [SerializeField] private Weapon weapon; 
    [SerializeField] private Transform bulletRef;
    [SerializeField] private float shotInterval;
    [SerializeField] private float shotTimer = 0;
    [SerializeField] private Animator anim; 

    void Update(){
        RaycastHit2D hit = Physics2D.Raycast(firingPoints[0].transform.position, Vector2.left, 10f);
        if(hit.collider != null && hit.collider.CompareTag("Player")){
            if(shotTimer >= shotInterval){
                weapon.Fire(bulletRef, firingPoints[0].transform.position, firingPoints[0].transform.rotation);
                shotTimer = 0;
            }
        }
        shotTimer += Time.deltaTime;
    }

    public void AnimationEventListener(){
        anim.Play("Shooting");
    }
}