using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LayoutSpawner : MonoBehaviour
{
    [SerializeField] private Transform currentLayout; 
    [SerializeField] private GameObject[] transitionPrefabs;
    [SerializeField] private GameObject[] battlefieldPrefabs;
    [SerializeField] private LayerMask spawnWhenNotFound;
    [SerializeField] private LayerMask spawnWhenFound;
    [Range(0f, 1f)][SerializeField] private float chanceForBattlefield;

    private Transform previousLayout = null;

    void Update(){
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 10f);
        if(hit.collider != null && hit.collider.CompareTag("ConnectionPoint")){
            SpawnNextLayout(hit.collider.transform);
            hit.collider.gameObject.SetActive(false);
        }
    }

    void SpawnNextLayout(Transform connectionPoint){
        Transform layoutToDestroy = previousLayout;
        previousLayout = currentLayout;
        
        GameObject nextPrefab = SelectNextPrefab();
        Tilemap tilemap = nextPrefab.transform.Find("Foreground").GetComponent<Tilemap>();

        Vector2 newPosition = CalculateNewPosition(connectionPoint, tilemap);
        
        currentLayout = Instantiate(nextPrefab, newPosition, transform.rotation).transform;
        if(layoutToDestroy != null) { Destroy(layoutToDestroy.gameObject); }
    }

    private bool ReachedMaxBounds(){
        return transform.position.x >= MaxBounds().x;
    }

    private Vector3 MaxBounds(){
        return currentLayout.GetComponent<LayoutBounds>().maxBounds;
    }

    private Vector2 CalculateNewPosition(Transform connectionPoint, Tilemap tilemap){
        Debug.Log(connectionPoint.position.x);
        Debug.Log($"{tilemap.cellBounds.xMin}, {tilemap.cellBounds.xMax}, {tilemap.cellBounds.size}, {tilemap.cellBounds.position}");

        // return new Vector2(connectionPoint.position.x + .25f + Mathf.Abs(tilemap.cellBounds.xMin) - tilemap.cellBounds.xMax / 2 * .75f, 0);
        return new Vector2(connectionPoint.position.x + Mathf.Abs(tilemap.cellBounds.xMin + 5), 0);
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
