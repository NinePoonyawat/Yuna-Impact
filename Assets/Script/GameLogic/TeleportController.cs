using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportController : MonoBehaviour
{
    public int startAreaIdx;
    public int destinateAreaIdx;
    public float checkRadius = 1f;

    public Transform teleportTransform;
    public Transform destinateTransform;

    public bool isHighGround = false;

    public bool Teleport(EntityController entity)
    {
        if (teleportTransform == null || destinateTransform == null) return false;
        if (Vector3.Distance(entity.transform.position,transform.position) > checkRadius)
        {
            return false;
        }
        entity.agent.Warp(destinateTransform.position);
        entity.currentArea = destinateAreaIdx;
        return true;
    }
}