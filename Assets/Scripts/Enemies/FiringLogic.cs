using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FiringLogic : MonoBehaviour
{
    public abstract GameObject ActiveFiringPoint(GameObject[] firingPoints);
}
