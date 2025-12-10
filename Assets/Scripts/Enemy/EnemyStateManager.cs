using UnityEngine;
using UnityEngine.AI;


public class EnemyStateManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [HideInInspector]
    public Transform player;

    // declare states
    EnemyBaseState currentState;
    public EnemyIdleState idleState = new EnemyIdleState();
    public EnemyPatrolState patrolState = new EnemyPatrolState();
    public EnemyAttackingState attackingState = new EnemyAttackingState();
    public EnemyChaseState chaseState = new EnemyChaseState();
    public EnemyDeadState deadState = new EnemyDeadState();
    public EnemySenseState senseState = new EnemySenseState();
    public EnemyWalkBackState walkBackState = new EnemyWalkBackState();

    // animator reference
    [HideInInspector]
    public Animator animator;

    [Header("Idle State Settings")]
    [Range(.5f, 10f)]
    [SerializeField] public float idleDuration = 2f; // center value for idle duration
    [Range(0f, 1f)]
    [SerializeField] public float idleDurationVariance = 0.2f; // variance percentage for idle duration


    [Header("Patrol State Settings")]
    [SerializeField] public float patrolRadius = 5f;
    [SerializeField] public float patrolSpeed = 2f;
    [HideInInspector] public Vector3 patrolPoint;


    [Header("Combat Settings")]
    public float detectionRange = 200f;
    public float attackRange = 2.5f;
    

    [Header("Health")]
    [SerializeField] public float maxHealth = 10f;
    [HideInInspector] public float currentHealth;
    [SerializeField] public float deathShrinkSpeed = 2f;

    [Header("Cobweb Projectile")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;

    [SerializeField] private float projectileSpeed = 5f;
    [SerializeField] private float projectileMaxDistance = 5f;

    [SerializeField] private Vector3 projectileStartScale = new Vector3(0.1f, 0.1f, 0.1f);
    [SerializeField] private Vector3 projectileEndScale = Vector3.one;
    [SerializeField] private float projectileGrowDuration = 0.3f;

    [HideInInspector]
    public NavMeshAgent agent;


    [Header("Visuals")]
    [SerializeField] private EnemyAura aura;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip attackOne;
    [SerializeField] private AudioClip attackTwo;




    void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        aura = GetComponentInChildren<EnemyAura>();

        // Disable automatic rotation so we can control visual facing
        if (agent != null)
        {
            agent.updateRotation = false;
            agent.updatePosition = true;
            agent.speed = patrolSpeed;
            agent.stoppingDistance = attackRange;
        }

        patrolPoint = transform.position;
        currentHealth = maxHealth;

        // Autoassign player if not set in Inspector
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
            {
                player = p.transform;
            }
            else
            {
                Debug.LogWarning($"[{name}] No object with tag 'Player' found. Enemy will not chase.");
            }
        }
    }
    void Start()
    {
        currentState = idleState;
        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        // State logic
        currentState.UpdateState(this);

        // Smoothly rotate to face movement direction (when using NavMeshAgent.updateRotation = false)
        if (agent != null)
        {
            Vector3 vel = agent.velocity;
            vel.y = 0f;

            if (vel.sqrMagnitude > 0.01f)
            {
                Quaternion targetRot = Quaternion.LookRotation(vel.normalized, Vector3.up);
                float rotSpeed = 10f; 
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * rotSpeed);
            }
        }
    }
    public void SwitchState(EnemyBaseState state)
    {
        if (currentState != null)
            currentState.ExitState(this);

        currentState = state;

        if (aura != null)
            aura.ApplyColorForState(currentState, this);

        currentState.EnterState(this);
    }

    // Animation Event Bridges
    public void OnAttackHit()
    {
        if (currentState is EnemyAttackingState attacking)
        {
            attacking.OnAttackHit(this);
        }
    }
    public void AttackAnimationEnd()
    {
        if (currentState is EnemyAttackingState attacking)
        {
            attacking.AttackAnimationEnd(this);
        }
    }

    public void TakeDamage(int amount)
    {
        if(currentHealth <= 0)
        {
            return; 
        }

        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
        Debug.Log($"Enemy took {amount} damage, current health: {currentHealth}");
    }

    public void Die()
    {
        if (currentState == deadState)
        {
            return; 
        }


        SwitchState(deadState);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (currentState != null)
        {
            currentState.OnColliderEnter(this, other);
        }
    }

    public void Animation_ShootProjectile()
    {
        
        if (projectilePrefab == null || projectileSpawnPoint == null)
        {
            Debug.LogWarning($"{name}: Projectile prefab or spawn point not assigned.");
            return;
        }

        GameObject proj = Instantiate(
            projectilePrefab,
            projectileSpawnPoint.position,
            projectileSpawnPoint.rotation
        );

        // Start small cobweb 
        proj.transform.localScale = projectileStartScale;

        // Direction is whatever the spawn point is facing
        Vector3 dir = player.position - projectileSpawnPoint.position;
        dir.y = .5f;
        dir = dir.normalized;

        StartCoroutine(ProjectileCobwebRoutine(proj.transform, dir));

        audioSource.PlayOneShot(attackOne);
    }

    private System.Collections.IEnumerator ProjectileCobwebRoutine(Transform projectile, Vector3 direction)
    {
        if (projectile == null)
            yield break;

        Vector3 startPos = projectile.position;
        float traveled = 0f;
        float growTime = 0f;

        while (projectile != null)
        {
            float step = projectileSpeed * Time.deltaTime;

            //  forward
            projectile.position += direction * step;
            traveled += step;

            // Grow from startScal to projectileGrowDuration
            if (growTime < projectileGrowDuration)
            {
                growTime += Time.deltaTime;
                float t = Mathf.Clamp01(growTime / projectileGrowDuration);
                projectile.localScale = Vector3.Lerp(projectileStartScale, projectileEndScale, t);
            }

            // Kill after distance
            if (traveled >= projectileMaxDistance)
            {
                Destroy(projectile.gameObject);
                yield break;
            }

            yield return null;
        }
    }
}
