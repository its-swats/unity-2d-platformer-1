using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayoutSpawner : MonoBehaviour
{
    [SerializeField] private Transform currentLayout; 
    [SerializeField] private GameObject[] layoutPrefabs;
    [SerializeField] private LayerMask spawnWhenNotFound;

    private Transform previousLayout = null;

    void Update(){
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 10f, spawnWhenNotFound);
        if(hit.collider == null && ReachedMaxBounds()){
            SpawnNextLayout();
        }
    }

    void SpawnNextLayout(){
        int rand = Random.Range(0, layoutPrefabs.Length);
        Transform layoutToDestroy = previousLayout;
        previousLayout = currentLayout;
        currentLayout = Instantiate(layoutPrefabs[rand], CalculateNewPosition(), transform.rotation).transform;
        if(layoutToDestroy != null) { Destroy(layoutToDestroy.gameObject); }
    }

    private bool ReachedMaxBounds(){
        return transform.position.x >= MaxBounds().x;
    }

    private Vector2 MaxBounds(){
        return currentLayout.GetComponent<LayoutBounds>().maxBounds;
    }

    private Vector2 CalculateNewPosition(){
        float min = currentLayout.GetComponent<LayoutBounds>().minBounds.x;
        return new Vector2(MaxBounds().x + ((MaxBounds().x - min) / 2 + 1), 0);
    }
}
