using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState { Passive, Alert, Aware, Asleep}

public class EnemyStateMachine
{
    public EnemyState enemyState = EnemyState.Passive;

    public EnemyState GetEnemyState()
    {
        return enemyState;
    }

    public void SetPassiveState()
    {
        enemyState = EnemyState.Passive;
    }

    public void SetAlertState()
    {
        enemyState = EnemyState.Alert;
    }

    public void SetAwareState()
    {
        enemyState = EnemyState.Aware;
    }

    public void SetAsleepState()
    {
        enemyState = EnemyState.Asleep;
    }

}
public class Patrol : MonoBehaviour
{
    public Transform[] waypoints;

    private int destPoint;
    private NavMeshAgent agent;


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
            GotoNextPoint();

    }

    void GotoNextPoint()
    {
        if (waypoints.Length == 0)
            return;

        agent.destination = waypoints[destPoint].position;
        destPoint = (destPoint + 1) % waypoints.Length;

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            print("caught");
        }
    }



}
