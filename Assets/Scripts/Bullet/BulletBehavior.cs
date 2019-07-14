using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    [SerializeField] private LayerMask whatToHit;

    private Rigidbody2D rb2d;
    private float speed = 4;
    private float bulletDamage = 1;
    
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        Fire();
    }

    void Fire(){
        rb2d.velocity = transform.right * speed;
        Invoke("DeleteSelf", bulletDamage);
    }

    void OnTriggerEnter2D(Collider2D hitInfo){
        if ((whatToHit & 1 << hitInfo.gameObject.layer) == 1 << hitInfo.gameObject.layer){
            hitInfo.gameObject.GetComponent<Health>().Hit(bulletDamage);
            DeleteSelf();
        }
    }

    void DeleteSelf(){
        Destroy(gameObject);
    }
}
