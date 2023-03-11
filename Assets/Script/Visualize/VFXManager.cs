using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    [SerializeField] private GameObject effects;
    [SerializeField] private Transform point;

    public void Play()
    {
        if (effects == null || transform == null) return;
        Instantiate(effects, point.position, point.rotation);
    }
}
