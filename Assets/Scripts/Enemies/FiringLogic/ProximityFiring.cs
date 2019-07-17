using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityFiring : FiringLogic
{
    [SerializeField] private float checkDistance = 5f;

    public override GameObject ActiveFiringPoint(GameObject[] firingPoints){
        if(Player.Instance == null)
            return null;
            
        float? bestDistance = null;
        GameObject closestPoint = null;

        for(int idx = 0; idx < firingPoints.Length; idx++){            
            float lastDistance = Vector2.Distance(firingPoints[idx].transform.position, Player.Center);
            if(lastDistance < checkDistance){
                if(bestDistance == null)
                    bestDistance = lastDistance;

                if(closestPoint == null)
                    closestPoint = firingPoints[idx];

                if(bestDistance > lastDistance){
                    bestDistance = lastDistance;
                    closestPoint = firingPoints[idx];
                }
            }        
        }

        return closestPoint;
    }
}
