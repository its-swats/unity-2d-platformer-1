using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public abstract void Fire(Transform bulletRef, Vector2 spawnPosition, Quaternion rotation);
}
