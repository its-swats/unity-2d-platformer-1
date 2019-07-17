using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAtPlayer : MonoBehaviour
{
    [SerializeField] private GameObject[] firingPoints;
    [SerializeField] private Weapon weapon; 
    [SerializeField] private Transform bulletRef;
    [SerializeField] private FiringLogic firingLogic;
    [SerializeField] private float shotInterval;
    [SerializeField] private float shotTimer = 0;
    [SerializeField] private Animator anim; 

    void Update(){
        GameObject firingPoint = firingLogic.ActiveFiringPoint(firingPoints);

        if(firingPoint != null){
            if(shotTimer >= shotInterval){
                weapon.Fire(bulletRef, firingPoint.transform.position, firingPoint.transform.rotation);
                shotTimer = 0;
            }
        }

        shotTimer += Time.deltaTime;
    }

    public void AnimationEventListener(){
        anim.Play("Shooting");
    }
}