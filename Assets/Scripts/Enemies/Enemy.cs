using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] public float moveSpeed;

    public void Hit(float damage){
        health -= damage;
        if(health <= 0){
            DestroySelf();
        }
    }

    private void DestroySelf(){
        Destroy(gameObject);
    }

}
