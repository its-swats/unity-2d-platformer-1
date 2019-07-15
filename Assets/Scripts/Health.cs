using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private bool isPlayer;
 
    public void Hit(float damage){
        health -= damage;
        if(health <= 0){
            DestroySelf();
        }
    }

    private void DestroySelf(){
        if(isPlayer){
            StartCoroutine(GetComponent<PlayerController>().Die());
        } else {
            Destroy(gameObject);
        }
    }

}
