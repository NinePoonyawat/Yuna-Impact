using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollowing : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0.2f, 6f, -5.6f);

    private void LateUpdate()
    {
        if (player == null) return;

        transform.position = player.position + offset;
    }

    public void Follow(Transform player)
    {
        this.player = player;
    }
}
