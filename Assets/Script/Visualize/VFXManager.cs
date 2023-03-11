using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    [SerializeField] private GameObject effects;
    [SerializeField] private Transform point;
    [SerializeField] private bool onThis;

    public void Play()
    {
        if (effects == null || transform == null) return;

        if (onThis) Instantiate(effects, point.position, point.rotation, point);
        else Instantiate(effects, point.position, point.rotation);
    }
}
