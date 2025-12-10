using UnityEngine;
// We no longer need UnityEngine.AI

public class TinySpiderAI : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 3.5f;
    [SerializeField] private float rotationSpeed = 10f; 
    [SerializeField] private float updateRate = 0.2f;   
    [SerializeField] private Transform player;        

    [Header("Lifetime")]
    [SerializeField] private float lifeTime = 3f;     // Destroy after this time

    // Removed NavMeshAgent
    // private NavMeshAgent agent; 
    private float updateTimer;

    void Awake()
    {
        // Removed NavMeshAgent check

        // Auto-find player if not assigned 
        if (player == null)
        {
            Debug.Log($"{name}: TinySpiderAI searching for Player tag");
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
            {
                Debug.Log($"{name}: TinySpiderAI found Player tag");
                player = p.transform;
            }
            else
            {
                Debug.LogWarning($"{name}: No GameObject with tag 'Player' found. Tiny spider will not chase.");
            }
        }
    }

    void Update()
    {
        // ---- movement ----
        // Now checks only if the player is found
        if (player != null)
        {

            MoveTowardsPlayer();
        }

  
        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0f)
        {
            Destroy(gameObject);
        }
    }


    private void MoveTowardsPlayer()
    {
        // 1. alculate direction to the target
        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        // 2. Rotate to face the player
        if (directionToPlayer != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

            
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }


        transform.position += transform.forward * speed * Time.deltaTime;


    }
}