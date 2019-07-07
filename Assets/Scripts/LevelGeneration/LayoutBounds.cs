using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayoutBounds : MonoBehaviour
{
    public Vector2 maxBounds;
    public Vector2 minBounds;

    void Start(){   
        foreach(Transform child in transform){
            if(maxBounds == Vector2.zero){
                maxBounds = child.position;
            } else if(child.position.x > maxBounds.x){
                maxBounds = child.position;
            }
            
            if(minBounds == Vector2.zero){
                minBounds = child.position;
            } else if(child.position.x < minBounds.x){
                minBounds = child.position;
            }
        }
    }
}
