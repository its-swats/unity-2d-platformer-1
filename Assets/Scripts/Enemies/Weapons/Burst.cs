using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Burst : Weapon
{
    [SerializeField] private int burstSize;
    [SerializeField] private float burstGap;
    public UnityEvent OnFire;

    public override void Fire(Transform bulletRef, Vector2 spawnPosition, Quaternion rotation){
        StartCoroutine(FireBurst(bulletRef, spawnPosition, rotation));
    }

    IEnumerator FireBurst(Transform bulletRef, Vector2 spawnPosition, Quaternion rotation){
        for(int i = 0; i < burstSize; i++){
            OnFire.Invoke();
            Instantiate(bulletRef, spawnPosition, rotation);
            yield return new WaitForSeconds(burstGap);
        }
    }
}
