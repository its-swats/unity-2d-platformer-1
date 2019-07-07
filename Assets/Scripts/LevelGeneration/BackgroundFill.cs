using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundFill : MonoBehaviour
{
    [SerializeField] private Transform backgroundPrefab;

    void Start()
    {
        for(int x = 1; x <= 5; x++){
            Vector2 newSpace = new Vector2(transform.position.x, transform.position.y - x);
            Instantiate(backgroundPrefab, newSpace, Quaternion.identity).transform.parent = transform;
        }
    }
}
