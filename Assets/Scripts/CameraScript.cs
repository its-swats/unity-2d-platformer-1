using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
   [SerializeField] private Transform player;
   [SerializeField] private float timeOffset;
   [SerializeField] private Vector2 posOffset;

//    private Vector3 velocity;

    void Start(){

    }

    void Update()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = player.transform.position;

        endPos.x += posOffset.x;
        endPos.y = transform.position.y;
        endPos.z = transform.position.z;

        transform.position = Vector3.Lerp(startPos, endPos, timeOffset * Time.deltaTime);

        // transform.position = Vector3.SmoothDamp(startPos, endPos, ref velocity, timeOffset);
    }
}
