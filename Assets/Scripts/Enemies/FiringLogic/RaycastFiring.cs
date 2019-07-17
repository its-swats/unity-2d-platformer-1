using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastFiring : FiringLogic
{
    [SerializeField] private float checkDistance = 10f;

    public override GameObject ActiveFiringPoint(GameObject[] firingPoints){
        for(int idx = 0; idx < firingPoints.Length; idx++){
            RaycastHit2D hit = Physics2D.Raycast(firingPoints[idx].transform.position, Vector2.left, checkDistance);
            if(hit.collider != null && hit.collider.CompareTag("Player")){
                return firingPoints[idx]; 
            }
        }

        return null;
    }
}
