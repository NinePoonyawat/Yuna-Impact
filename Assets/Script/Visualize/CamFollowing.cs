using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollowing : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;

    void Start()
    {
        offset = transform.position - player.position;
    }

    private void LateUpdate()
    {
        if (player == null) return;

        transform.position = player.position + offset;
    }
}
