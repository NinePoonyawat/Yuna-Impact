using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public Camera cam;

    public Vector3 moveToPos;

    public NavMeshAgent agent;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray,out hit))
            {
                agent.SetDestination(hit.point);
                moveToPos = hit.point;  
            }
        }
    }

    void OnDrawGizmos()
    {
        if (moveToPos == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, moveToPos);
    }
}
