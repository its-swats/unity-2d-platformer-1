using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyArray;
    private bool active = true;

    public void SpawnEnemy(){
        if(active){
            int enemyIdx = Random.Range(0, enemyArray.Length);
            Instantiate(enemyArray[enemyIdx], transform.position, transform.rotation);
            active = false;
        }
    }
}
