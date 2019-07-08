using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayoutSpawner : MonoBehaviour
{
    [SerializeField] private Transform currentLayout; 
    [SerializeField] private GameObject[] transitionPrefabs;
    [SerializeField] private GameObject[] battlefieldPrefabs;
    [SerializeField] private LayerMask spawnWhenNotFound;
    [Range(0f, 1f)][SerializeField] private float chanceForBattlefield;

    private Transform previousLayout = null;

    void Update(){
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 10f, spawnWhenNotFound);
        if(hit.collider == null && ReachedMaxBounds()){
            SpawnNextLayout();
        }
    }

    void SpawnNextLayout(){
        Transform layoutToDestroy = previousLayout;
        previousLayout = currentLayout;
        currentLayout = Instantiate(SelectNextPrefab(), CalculateNewPosition(), transform.rotation).transform;
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
        return new Vector2(MaxBounds().x + Random.Range(1,3), 0);
    }

    private GameObject SelectNextPrefab(){
        float num = Random.Range(0f, 1f);
        if(num < chanceForBattlefield){
            int rand = Random.Range(0, battlefieldPrefabs.Length);
            return battlefieldPrefabs[rand];
        } else {
            int rand = Random.Range(0, transitionPrefabs.Length);
            return transitionPrefabs[rand];
        }
    }
}
