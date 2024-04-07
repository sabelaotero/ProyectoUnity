using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class NavTerrenoLento : MonoBehaviour
{
    private NavMeshModifier mesh_Surface;
    // Start is called before the first frame update
    void Start()
    {
        mesh_Surface = GetComponent<NavMeshModifier>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<NavMeshAgent>(out NavMeshAgent agent))
        {
            if (mesh_Surface.AffectsAgentType(agent.agentTypeID)){
                agent.speed /= NavMesh.GetAreaCost(mesh_Surface.area);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<NavMeshAgent>(out NavMeshAgent agent))
        {
            if (mesh_Surface.AffectsAgentType(agent.agentTypeID)){
                agent.speed *= NavMesh.GetAreaCost(mesh_Surface.area);
            }
        }
    }

}
