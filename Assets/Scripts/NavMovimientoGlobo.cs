using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMovimientoGlobo : MonoBehaviour
{
    public GameObject waypointParent;
    private NavMeshAgent agent;
    private GameObject[] waypoints;
    private int waypointIndex = 0;
    private int max;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        max = waypointParent.transform.childCount;
        waypoints = new GameObject[max];
        for (int i = 0; i < max; i++)
        {
            waypoints[i] = waypointParent.transform.GetChild(i).gameObject;
        }
        GoToNextWayPoint();
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.remainingDistance < 0.1)
        {
            waypointIndex = (waypointIndex + 1) % max;
            GoToNextWayPoint();
        }
    }

    private void GoToNextWayPoint()
    {
        agent.SetDestination(waypoints[waypointIndex].transform.position);
    }
}
