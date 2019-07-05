using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private float speed = 4;
    private float bulletLife = 2;
    
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        Fire();
    }

    void Fire(){
        rb2d.velocity = transform.right * speed;
        Invoke("DeleteSelf", bulletLife);
    }

    void OnTriggerEnter2D(Collider2D hitInfo){
        if(hitInfo.gameObject.layer == 8){
            DeleteSelf();
        }
    }

    void DeleteSelf(){
        Destroy(gameObject);
    }
}
