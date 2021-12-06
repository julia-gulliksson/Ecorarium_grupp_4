using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfAttackSheepState : WolfBaseState
{
    GameObject[] sheepTargets;
    WolfStateManager wolf;

    public override void EnterState(WolfStateManager wolfRef)
    {
        wolf = wolfRef;
        sheepTargets = GameObject.FindGameObjectsWithTag("Sheep");
    }

    public override void UpdateState()
    {
        ChooseTarget();
    }

    public override void OnDestroy()
    {

    }

    public override void ExitState()
    {

    }

    void ChooseTarget()
    {
        // Calculate path to nearest sheep
        float closestTargetDistance = float.MaxValue;
        UnityEngine.AI.NavMeshPath path;
        UnityEngine.AI.NavMeshPath shortestPath = null;
        foreach (GameObject target in sheepTargets)
        {
            if (target == null) continue;
            path = new UnityEngine.AI.NavMeshPath();
            if (UnityEngine.AI.NavMesh.CalculatePath(wolf.transform.position, target.transform.position, wolf.navMeshAgent.areaMask, path))
            {
                float distance = Vector3.Distance(wolf.transform.position, path.corners[0]);
                for (int i = 1; i < path.corners.Length; i++)
                {
                    distance += Vector3.Distance(path.corners[i - 1], path.corners[i]);
                }
                if (distance < closestTargetDistance)
                {
                    closestTargetDistance = distance;
                    shortestPath = path;
                }
            }
        }
        if (shortestPath != null)
        {
            // Attack nearest sheep
            wolf.navMeshAgent.SetPath(shortestPath);
        }
        else
        {
            // All sheep are dead
            wolf.navMeshAgent.ResetPath();
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Sheep"))
        {
            Object.Destroy(collider.gameObject);
        }
    }


}
