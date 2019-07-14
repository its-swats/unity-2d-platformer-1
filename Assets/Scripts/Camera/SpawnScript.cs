using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour
{
    [SerializeField] GameObject playerRef;

    public static SpawnScript spawner;

    void Start(){
        if(spawner == null){
            spawner = GetComponent<SpawnScript>();
        }

        SpawnPlayer();
    }

    public void SpawnPlayer(){
        Instantiate(playerRef, transform.position, transform.rotation); 
    }
}
