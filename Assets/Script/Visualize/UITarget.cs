using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITarget : MonoBehaviour
{
    [SerializeField] private Transform target;

    void Update()
    {
        if (target == null) 
        {
            gameObject.SetActive(false);
            return;
        }
        transform.position = Vector3.Lerp(transform.position, target.position, 0f);
    }

    public void SetTarget(EnemyController enemy)
    {
        gameObject.SetActive(true);
        target = enemy.transform;
    }
}
