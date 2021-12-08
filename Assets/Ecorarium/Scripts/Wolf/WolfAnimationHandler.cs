using UnityEngine;
using UnityEngine.AI;

public class WolfAnimationHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject effectPrefab;
    Animator wolfAnimation;
    float velocity;
    float normalizedVelocity;
    NavMeshAgent wolfNa;
    WolfStateManager wolfController;
    int wolfId;

    private void OnEnable()
    {
        GameEventsManager.current.onWolfAttacking += AttackStart;
        GameEventsManager.current.onWolfStopAttacking += AttackStop;
    }

    private void OnDisable()
    {
        GameEventsManager.current.onWolfAttacking -= AttackStart;
        GameEventsManager.current.onWolfStopAttacking -= AttackStop;
    }

    void Start()
    {
        wolfAnimation = GetComponent<Animator>();
        wolfNa = GetComponent<NavMeshAgent>();
        wolfController = GetComponent<WolfStateManager>();
        wolfId = wolfController.id;
    }

    void Update()
    {
        wolfAnimation.SetFloat("Velocity", GetNormalVelocity());
    }

    private float GetNormalVelocity()
    {
        velocity = wolfNa.velocity.magnitude;
        normalizedVelocity = velocity / wolfNa.desiredVelocity.magnitude;
        return normalizedVelocity;
    }

    void AttackStart(int id)
    {
        if (id == wolfId)
        {
            wolfAnimation.SetBool("Attack", true);
        }
    }

    void AttackStop(int id)
    {
        if (id == wolfId)
        {
            wolfAnimation.SetBool("Attack", false);
        }
    }

    public void AttackEffect()
    {
        float nosepositionZ = transform.localScale.z - 0.5f;

        Vector3 nosePosition = transform.forward * nosepositionZ;

        Instantiate(effectPrefab, transform.position + nosePosition, transform.rotation);
    }
}
