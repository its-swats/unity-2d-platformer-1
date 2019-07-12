using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActivator : MonoBehaviour
{
    [SerializeField] private LayerMask whatToTrigger;

    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 10f, whatToTrigger);
        if(hit.collider != null){
            hit.collider.GetComponent<EnemySpawner>().SpawnEnemy();
            Destroy(hit.collider.gameObject);
        }
    }
}
