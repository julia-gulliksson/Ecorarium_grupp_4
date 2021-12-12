using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheLoneHerder
{
    public class WolfAttackFenceState : WolfBaseState
    {
        Vector3 hitPoint;
        WolfStateManager wolf;
        float heightOffset = 1f;
        float heightOffsetLower = 0.2f;
        float range = 4f;
        bool hasFoundTarget = false;
        float rotationSpeed = 5f;
        float growlEffectDistance = 4f;
        ParticleSystem rippleEffect;

        public override void EnterState(WolfStateManager wolfRef)
        {
            wolfRef.navMeshAgent.SetDestination(wolfRef.targetPoint);
            wolf = wolfRef;
            wolf.growling = wolf.StartCoroutine(Growl());
        }

        public override void UpdateState()
        {
            DetectFence();

            RotateRippleEffect();
        }

        void DetectFence()
        {
            Transform transform = wolf.transform;
            Vector3 forward = transform.forward;
            Vector3 right = transform.right;
            Vector3 startPoint = new Vector3(transform.position.x, transform.position.y + heightOffset, transform.position.z);
            Vector3 startPointLower = new Vector3(transform.position.x, transform.position.y + heightOffsetLower, transform.position.z);
            Ray faceForward = new Ray(startPoint, forward * range);
            Ray faceForwardLower = new Ray(startPointLower, forward * range);
            Ray faceRight = new Ray(startPoint, right * range);
            Ray faceRightLower = new Ray(startPointLower, right * range);
            Ray faceLeft = new Ray(startPoint, -right * range);
            Ray faceLeftLower = new Ray(startPointLower, -right * range);

            // Create list of rays facing different directions
            List<RayDirection> rayDirections = new List<RayDirection>() { new RayDirection(faceForward, wolf.targetPoint, wolf.hitMask, range, Direction.Forward), new RayDirection(faceForwardLower, wolf.targetPoint, wolf.hitMask, range, Direction.Forward),
            new RayDirection(faceRight, wolf.targetPoint, wolf.hitMask, range, Direction.Right), new RayDirection(faceRightLower, wolf.targetPoint, wolf.hitMask, range, Direction.Right),
            new RayDirection(faceLeftLower, wolf.targetPoint, wolf.hitMask, range, Direction.Left), new RayDirection(faceLeft, wolf.targetPoint, wolf.hitMask, range, Direction.Left)};

            List<RayDirection> raysFoundTarget = new List<RayDirection>();
            foreach (RayDirection ray in rayDirections)
            {
                if (ray.TargetFound())
                {
                    raysFoundTarget.Add(ray);
                }
            }
            if (raysFoundTarget.Count > 0 && raysFoundTarget[0].TargetFound())
            {
                HandleDetectedFence(raysFoundTarget);
            }
        }

        void HandleDetectedFence(List<RayDirection> rays)
        {
            RayDirection firstRay = rays[0];

            if (Vector3.Distance(wolf.navMeshAgent.destination, wolf.transform.position) < 1f && !hasFoundTarget)
            {
                IFence fence = firstRay.hit.collider.GetComponent<IFence>();
                if (fence != null)
                {
                    fence.WolfHit();
                    wolf.fenceScript = fence;

                    GameEventsManager.current.WolfAttacking(wolf.id);

                    hasFoundTarget = true;
                    hitPoint = -firstRay.hit.normal;
                }
            }

            if (hasFoundTarget)
            {
                Quaternion look = Quaternion.LookRotation(hitPoint);
                if (wolf.transform.rotation == look && rays.Count == 1 && firstRay.direction != Direction.Forward)
                {
                    // Account for wolves at end of fence facing the wrong way
                    float yRotation = -90f;
                    if (firstRay.direction == Direction.Left) yRotation = 90.0f;
                    Vector3 newTarget = new Vector3(0.0f, yRotation, 0.0f);
                    Quaternion newLook = Quaternion.LookRotation(newTarget);
                    wolf.transform.rotation = Quaternion.Slerp(wolf.transform.rotation, newLook, rotationSpeed * Time.deltaTime);
                }
                else
                {
                    wolf.transform.rotation = Quaternion.Slerp(wolf.transform.rotation, look, rotationSpeed * Time.deltaTime);
                }
            }
        }

        public void HandleFenceBreak()
        {
            wolf.StopCoroutine(wolf.growling);
            // Stop attack animation
            GameEventsManager.current.WolfStopAttacking(wolf.id);
            // Register wolf lost fence
            wolf.fenceScript?.WolfLost();
            // Switch to attacking sheep instead
            wolf.SwitchState(wolf.AttackSheepState);
        }

        IEnumerator Growl()
        {
            while (true)
            {
                float randomWait = Random.Range(3, 15);
                yield return new WaitForSeconds(randomWait);

                wolf.growl.Play();

                if (Vector3.Distance(wolf.transform.position, wolf.player.position) > growlEffectDistance)
                {
                    float nosePositionZ = wolf.transform.localScale.z - 0.5f;
                    Vector3 nosePosition = wolf.transform.forward * nosePositionZ;
                    rippleEffect = Object.Instantiate(wolf.ripple, wolf.transform.position + nosePosition, wolf.transform.rotation);
                    rippleEffect.transform.SetParent(wolf.transform);
                }
            }
        }

        void RotateRippleEffect()
        {
            if (rippleEffect != null)
            {
                rippleEffect.transform.LookAt(wolf.player);
            }
        }

        public override void OnDestroy()
        {
            wolf.fenceScript?.WolfLost();
        }

        public override void ExitState()
        {
            wolf.fenceScript?.WolfLost();
            wolf.StopCoroutine(wolf.growling);
        }

        public override void OnTriggerEnter(Collider collider) { }
    }
}