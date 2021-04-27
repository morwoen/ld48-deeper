using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Patrol : MonoBehaviour
{
    public Transform[] waypoints;

    private int destPoint;
    private NavMeshAgent agent;
    [SerializeField] public Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Checks AI pathfinding for distance to next target
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            GotoNextPoint();
        }
        if(agent.velocity.magnitude > 0)
        {
            animator.SetBool("IsMoving", true);
            Debug.Log("agent.velocity.magnitude > 0");
        }
        else
        {
            animator.SetBool("IsMoving", false);
            Debug.Log("agent.velocity.magnitude = 0");
        }

    }

    void GotoNextPoint()
    {
        if (waypoints.Length == 0)
            return;

        //agent.destination = waypoints[destPoint].position;
        agent.SetDestination(waypoints[destPoint].position);
        destPoint = (destPoint + 1) % waypoints.Length;
        //Debug.Log($"DestinationIndex: {destPoint}");
        //Debug.Log($"waypoint: {waypoints[destPoint].position}");

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            print("caught");
        }
    }
}
