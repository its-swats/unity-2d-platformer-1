using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActivator : MonoBehaviour
{
    [SerializeField] private LayerMask whatToTrigger;

    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 10f, whatToTrigger);
        Debug.Log(hit.collider);
        if(hit.collider != null){
            Debug.Log("Collider");
            hit.collider.GetComponent<EnemySpawner>().SpawnEnemy();
        }
    }
}
