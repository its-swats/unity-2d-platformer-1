using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private bool isPlayer;
    [SerializeField] private ParticleSystem deathEffect;
    [SerializeField] private float deathDelay = 0f;

    private bool dying = false;
 
    public void Hit(float damage){
        health -= damage;
        if(health <= 0 && !dying){
            dying = true;
            DestroySelf();
        }
    }

    private void DestroySelf(){
        if(isPlayer){
            StartCoroutine(GetComponent<PlayerController>().Die());
        } else {
            if(deathEffect){
                Instantiate(deathEffect, GetComponent<Renderer>().bounds.center, transform.rotation).Play();
            }
            StartCoroutine(Die());
        }
    }

    IEnumerator Die(){
        yield return new WaitForSeconds(deathDelay);
        Destroy(gameObject);
    }

}
