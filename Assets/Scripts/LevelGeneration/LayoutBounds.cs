using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LayoutBounds : MonoBehaviour
{
    public Vector3 maxBounds;
    public Vector3 minBounds;

    void Start(){
        Tilemap tilemap = transform.Find("Foreground").GetComponent<Tilemap>();

        foreach(var child in tilemap.cellBounds.allPositionsWithin){
            if(maxBounds == Vector3.zero){
                maxBounds = child;
            } else if(child.x > maxBounds.x){
                maxBounds = child;
            }
            
            if(minBounds == Vector3.zero){
                minBounds = child;
            } else if(child.x < minBounds.x){
                minBounds = child;
            }
        }
    }
}