using UnityEngine;

public class MinySpiderController : MonoBehaviour
{
    [SerializeField] private string walkStateName = "isWalking"; 

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogWarning($"{name}: No Animator found on mini spider.");
        }
    }

    private void OnEnable()
    {
        if (animator == null) return;

        // Force the walk state every time the projectile is spawned
        animator.Play(walkStateName, 0, 0f);
        animator.speed = 1f;
    }
}
