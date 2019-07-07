using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour
{
    [SerializeField] GameObject playerRef;

    public void SpawnPlayer(float pos){
        Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y, pos);
        Instantiate(playerRef, spawnPos, transform.rotation);        
    }
}
