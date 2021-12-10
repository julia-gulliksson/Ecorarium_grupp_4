using UnityEngine;

namespace TheLoneHerder
{
    public class WolfRetreatState : WolfBaseState
    {
        WolfStateManager wolf;
        public override void EnterState(WolfStateManager wolfRef)
        {
            GameEventsManager.current.WolfStopAttacking(wolfRef.id);
            wolfRef.navMeshAgent.SetDestination(wolfRef.spawnPosition);
            wolf = wolfRef;
        }

        public override void UpdateState()
        {
            // Despawn when the wolf (almost) reaches spawn position
            if (Vector3.Distance(wolf.transform.position, wolf.spawnPosition) < 2f)
            {
                Object.Destroy(wolf.gameObject);
            }
        }

        public override void OnDestroy() { }

        public override void ExitState() { }

        public override void OnTriggerEnter(Collider collider) { }
    }
}