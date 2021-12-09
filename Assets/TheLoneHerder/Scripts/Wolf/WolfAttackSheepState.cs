using UnityEngine;
using UnityEngine.AI;

namespace TheLoneHerder
{
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

        void ChooseTarget()
        {
            // Calculate path to nearest sheep
            float closestTargetDistance = float.MaxValue;
            NavMeshPath path;
            NavMeshPath shortestPath = null;
            foreach (GameObject target in sheepTargets)
            {
                if (target == null) continue;
                path = new NavMeshPath();
                if (NavMesh.CalculatePath(wolf.transform.position, target.transform.position, wolf.navMeshAgent.areaMask, path))
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
                wolf.navMeshAgent.ResetPath();

                if (!GameEventsManager.current.GameIsOver)
                {
                    // All sheep are dead, game over
                    GameEventsManager.current.GameOver();
                }
            }
        }

        public override void OnTriggerEnter(Collider collider)
        {
            IDestroyable destroyable = collider.gameObject.GetComponent<IDestroyable>();
            // Call the Damage function if the trigger collider enherits IDestroyable (is a sheep)
            destroyable?.Damage();
        }

        public override void ExitState() { }
        public override void OnDestroy() { }
    }
}