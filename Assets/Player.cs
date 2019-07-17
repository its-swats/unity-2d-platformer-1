using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static Player _instance;
    public static Player Instance { get { return _instance; } }
    public static Vector3 Center { get { return _instance.GetComponent<Renderer>().bounds.center; } }

    private void Awake(){
        _instance = this;
    }
}
