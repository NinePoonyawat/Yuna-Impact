using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITarget : MonoBehaviour
{
    [SerializeField] private Transform playerCircle;
    [SerializeField] private Transform player;
    [SerializeField] private Transform[] enemyCircles;
    [SerializeField] private Transform[] targets;

    void Update()
    {
        for (int i = 0; i < 4; i++)
        {
            if (targets[i] == null) 
            {
                enemyCircles[i].gameObject.SetActive(false);
            }
            else
            {
                enemyCircles[i].position = Vector3.Lerp(enemyCircles[i].position, targets[i].position, 1f);
            }
        }
        if (player == null)
        {
            playerCircle.gameObject.SetActive(false);
        }
        else
        {
            playerCircle.position = Vector3.Lerp(playerCircle.position, player.position, 1f);
        }
    }

    public void SetTarget(EnemyController enemy, int idx)
    {
        enemyCircles[idx].gameObject.SetActive(true);
        targets[idx] = enemy.transform.parent;
    }

    public void SetPlayer(PlayerController player)
    {
        playerCircle.gameObject.SetActive(true);
        this.player = player.transform.parent;
    }
}
