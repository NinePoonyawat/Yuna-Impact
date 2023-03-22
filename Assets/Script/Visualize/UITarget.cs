using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITarget : MonoBehaviour
{
    [SerializeField] private Transform[] enemyCircles;
    [SerializeField] private Transform[] targets;

    void Update()
    {
        for (int i = 0; i < 4; i++)
        {
            if (targets[i] == null) 
            {
                enemyCircles[i].gameObject.SetActive(false);
                return;
            }
            enemyCircles[i].position = Vector3.Lerp(enemyCircles[i].position, targets[i].position, 1f);
        }
        
    }

    public void SetTarget(EnemyController enemy, int idx)
    {
        enemyCircles[idx].gameObject.SetActive(true);
        targets[idx] = enemy.transform.parent;
    }
}
