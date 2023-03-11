using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    [SerializeField] private GameObject effects;
    [SerializeField] private GameObject effects2;
    [SerializeField] private Transform point;
    [SerializeField] private bool onThis;

    public void Play()
    {
        if (effects == null || transform == null) return;

        if (onThis) Instantiate(effects, point.position, point.rotation, point);
        else Instantiate(effects, point.position, point.rotation);
    }
    
    public void Play2()
    {
        if (effects2 == null || transform == null) return;

        if (onThis) Instantiate(effects2, point.position, point.rotation, point);
        else Instantiate(effects2, point.position, point.rotation);
    }
}
